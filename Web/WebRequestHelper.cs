using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;
using LCW.Framework.Common.SysFile;

namespace LCW.Framework.Common.Web
{
    public class WebRequestHelper
    {
        public static string GetHtml(string url)
        {
            return GetHtml(url,null);
        }

        public static string GetHtml(string url, string postdata)
        {
            return GetHtml(url, postdata, DownloadMethod.GET);
        }

        public static string GetHtml(string url, string postdata,DownloadMethod method)
        {
            WebDownLoader info = new WebDownLoader();
            info.Url = url;
            info.Method = method;
            info.PostData = postdata;
            info.ConnectionTimeout = new TimeSpan(0, 10, 0);
            return GetHtml(info);
        }

        public static string GetHtml(WebDownLoader info)
        {
            string html=string.Empty;
            RequestState<string> requestState = new RequestState<string>();
            requestState.Retry = 5;
            requestState.Complete = (request) =>
            {
                html = request.RequestData.ToString();
            };
            info.Download<string>(requestState);
            return html;
        }

        public static string GetHtmlAsync(string url)
        {
            return GetHtmlAsync(url, null);
        }

        public static string GetHtmlAsync(string url, string postdata)
        {
            return GetHtmlAsync(url, postdata, DownloadMethod.GET);
        }

        public static string GetHtmlAsync(string url, string postdata, DownloadMethod method)
        {
            WebDownLoader info = new WebDownLoader();
            info.Url = url;
            info.Method = method;
            info.PostData = postdata;
            return GetHtmlAsync(info);
        }

        public static string GetHtmlAsync(WebDownLoader info)
        {
            string html = "";
            RequestState<string> requestState = new RequestState<string>();
            requestState.Complete = (request) =>
            {
                html = requestState.RequestData.ToString();
            };
            info.DownloadAsync<string>(requestState);
            return html;
        }


        public static string GetImage(string url,string path)
        {
            string imagepath="";
            WebDownLoader info = new WebDownLoader();
            info.Url = url;
            info.Method = DownloadMethod.GET;
            RequestState<string> requestState = new RequestState<string>();
            requestState.Retry = 15;
            if(!DirectoryHelper.CreateDir(path))
            {
                return "path create fail";
            }
            requestState.Complete = (request) =>
            {
                byte[] data;
                if (request.StreamResponse != null)
                {
                    using (var stream = request.StreamResponse)
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Byte[] buffer = new Byte[1024];
                            int current = 0;
                            while ((current = stream.Read(buffer, 0, buffer.Length)) != 0)
                            {
                                ms.Write(buffer, 0, current);
                            }
                            data = ms.ToArray();
                        }
                    }
                    string filename = path + "\\" + Guid.NewGuid().ToString() + url.Substring(url.LastIndexOf('.'));
                    if (FileHelper.WriteFile(data, filename))
                    {
                        imagepath = filename;
                    }
                }
            };
            info.DownloadImage<string>(requestState);
            return imagepath;
        }
    }

    public class WebDownLoader
    {
        private static ManualResetEvent allDone = new ManualResetEvent(false);
        private const int BUFFER_SIZE = 1024;
        private const int DefaultTimeout = 2 * 60 * 1000; 
        public TimeSpan? RetryWaitDuration { get; set; }
        public string ProxyAddress { get; set; }
        public TimeSpan? ReadTimeout { get; set; }
        public TimeSpan? ConnectionTimeout { get; set; }
        public bool UseCookies { get; set; }
        public string UserAgent { get; set; }
        public DownloadMethod Method { get; set; }
        public string Url { get; set; }
        public string PostData { get; set; }

        private void SetDefaultRequestProperties(HttpWebRequest request)
        {
            request.AllowAutoRedirect = true;
            request.UserAgent = UserAgent;
            request.Accept = "text/html, application/xhtml+xml, */*";
            request.KeepAlive = true;
            request.Pipelined = true;
            request.Credentials = CredentialCache.DefaultCredentials;
            if (ConnectionTimeout.HasValue)
            {
                request.Timeout = Convert.ToInt32(ConnectionTimeout.Value.TotalMilliseconds);
            }

            if (ReadTimeout.HasValue)
            {
                request.ReadWriteTimeout = Convert.ToInt32(ReadTimeout.Value.TotalMilliseconds);
            }

            if (UseCookies)
            {
                request.CookieContainer = new CookieContainer();
            }

            if (string.IsNullOrEmpty(UserAgent))
            {
                request.UserAgent = "Mozilla/5.0";//Mozilla/5.0 (compatible; MSIE 9.0; Windows NT 6.1; Trident/5.0; BOIE9;ZHCN)
            }
            if (!string.IsNullOrEmpty(ProxyAddress))
            {
                WebProxy Proxy = new WebProxy();
                Proxy.Address = new Uri(ProxyAddress);
                request.Proxy = Proxy;
            }
            if (DownloadMethod.POST == Method)
            {
                request.ContentType = "application/x-www-form-urlencoded";
                byte[] buffer =Encoding.Default.GetBytes(PostData);
                request.ContentLength = buffer.Length;
                using (var stream = request.GetRequestStream())
                {
                    stream.Write(buffer, 0, buffer.Length);
                    stream.Close();
                }
            }           
        }

        internal void DownloadImage<T>(RequestState<T> requestState)
        {
            if (RetryWaitDuration.HasValue)
            {
                Thread.Sleep(RetryWaitDuration.Value);
            }

            if (requestState.Retry-- > 0)
            {
                try
                {
                    //StringBuilder content = new StringBuilder();
                    requestState.Request = (HttpWebRequest)WebRequest.Create(Url);
                    requestState.Request.Method = requestState.Method.ToString();
                    SetDefaultRequestProperties(requestState.Request);
                    HttpWebResponse response = (HttpWebResponse)requestState.Request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        requestState.StreamResponse = response.GetResponseStream();
                        if (requestState.Complete != null)
                        {
                            requestState.Complete(requestState);
                        }
                    }
                    else
                    {
                        requestState.Exception = new Exception("StatusCode:" + response.StatusDescription);
                        DownloadImage(requestState);
                    }                    
                }
                catch (Exception ex)
                {
                    requestState.Exception = ex;
                    if (requestState.Request != null)
                    {
                        requestState.Request.Abort();
                    }
                    DownloadImage(requestState);                    
                }
            }
            else
            {
                if (requestState.Complete != null)
                {
                    requestState.Complete(requestState);
                }
            }
        }
        internal void Download<T>(RequestState<T> requestState)
        {
            if (RetryWaitDuration.HasValue)
            {
                Thread.Sleep(RetryWaitDuration.Value);
            }

            if (requestState.Retry-- > 0)
            {
                try
                {
                    //StringBuilder content = new StringBuilder();
                    requestState.Request = (HttpWebRequest)WebRequest.Create(Url);
                    requestState.Request.Method = requestState.Method.ToString();
                    SetDefaultRequestProperties(requestState.Request);
                    HttpWebResponse response = (HttpWebResponse)requestState.Request.GetResponse();
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        using (StreamReader str = new StreamReader(response.GetResponseStream()))
                        {
                            // 开始读取数据  
                            Char[] sReaderBuffer = new Char[BUFFER_SIZE];
                            int count = str.Read(sReaderBuffer, 0, sReaderBuffer.Length);
                            while (count > 0)  //while ((temp = reader.ReadLine()) != null)
                            {
                                String tempStr = new String(sReaderBuffer, 0, count);
                                requestState.RequestData.Append(tempStr);
                                count = str.Read(sReaderBuffer, 0, sReaderBuffer.Length);
                            }
                            // 读取结束  
                            str.Close();
                        }
                        response.Close();

                        if (requestState.Complete != null)
                        {
                            requestState.Complete(requestState);
                        }
                    }
                    else
                    {
                        requestState.Exception = new Exception("StatusCode:" + response.StatusDescription);
                        Download(requestState);
                    }                    
                }
                catch (Exception ex)
                {
                    requestState.Exception = ex;
                    if (requestState.Request != null)
                    {
                        requestState.Request.Abort();
                    }
                    Download(requestState);
                }
            }
            else
            {
                if (requestState.Complete != null)
                {
                    requestState.Complete(requestState);
                }
            }
        }

        internal void DownloadAsync<T>(RequestState<T> requestState)
        {
            if (RetryWaitDuration.HasValue)
            {
                Thread.Sleep(RetryWaitDuration.Value);
            }
            try
            {
                requestState.Request = (HttpWebRequest)WebRequest.Create(Url);
                requestState.Request.Method = requestState.Method.ToString();
                SetDefaultRequestProperties(requestState.Request);
                IAsyncResult result = (IAsyncResult)requestState.Request.BeginGetResponse(new AsyncCallback(RespCallback<T>), requestState);

                ThreadPool.RegisterWaitForSingleObject(result.AsyncWaitHandle, new WaitOrTimerCallback(TimeoutCallback), requestState, DefaultTimeout, true);
                allDone.WaitOne();
            }
            catch (Exception ex)
            {
                if (requestState.Request != null)
                    requestState.Request.Abort();
                throw ex;
            }          
        }
        private static void RespCallback<T>(IAsyncResult asynchronousResult)
        {
            try
            {
                // State of request is asynchronous.
                RequestState<T> myRequestState = (RequestState<T>)asynchronousResult.AsyncState;
                HttpWebRequest myHttpWebRequest = myRequestState.Request;
                HttpWebResponse response = (HttpWebResponse)myHttpWebRequest.EndGetResponse(asynchronousResult);

                // Read the response into a Stream object.
                Stream responseStream = response.GetResponseStream();
                myRequestState.StreamResponse = responseStream;

                // Begin the Reading of the contents of the HTML page and print it to the console.
                IAsyncResult asynchronousInputRead = responseStream.BeginRead(new byte[BUFFER_SIZE], 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack<T>), myRequestState);
                return;
            }
            catch (WebException e)
            {
                throw e;
            }
            allDone.Set();
        }
        private static void ReadCallBack<T>(IAsyncResult asyncResult)
        {
            RequestState<T> myRequestState = (RequestState<T>)asyncResult.AsyncState;
            try
            {                
                Stream responseStream = myRequestState.StreamResponse;
                int read = responseStream.EndRead(asyncResult);
                // Read the HTML page and then print it to the console.
                if (read > 0)
                {
                    myRequestState.RequestData.Append(Encoding.ASCII.GetString(new byte[BUFFER_SIZE], 0, read));
                    IAsyncResult asynchronousResult = responseStream.BeginRead(new byte[BUFFER_SIZE], 0, BUFFER_SIZE, new AsyncCallback(ReadCallBack<T>), myRequestState);
                    return;
                }
                else
                {                   
                    responseStream.Close();                   
                }
            }
            catch (WebException ex)
            {
                myRequestState.Exception = ex;
            }
            allDone.Set();

            if (myRequestState.Complete != null)
            {
                myRequestState.Complete(myRequestState);
            }
        }
        private static void TimeoutCallback(object state, bool timedOut)
        {
            if (timedOut)
            {
                HttpWebRequest request = state as HttpWebRequest;
                if (request != null)
                {
                    request.Abort();
                }
            }
        }
    }

    public enum DownloadMethod
    {
        GET,
        POST,
        HEAD,
    }

    public class RequestState<T>
    {
        public RequestState()
        {
            RequestData = new StringBuilder();
        }
        public Exception Exception { get; set; }
        public Stopwatch DownloadTimer { get; set; }
        public HttpWebRequest Request { get; set; }
        public Action<RequestState<T>> Complete { get; set; }
        public DownloadMethod Method { get; set; }
        public StringBuilder RequestData { internal set; get; }
        public Stream StreamResponse { get; set; }
        public int Retry { get; set; }
    }
}
