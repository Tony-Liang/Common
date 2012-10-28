using System;
using System.Collections.Generic;
using System.Net;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.IO;

namespace LCW.Framework.Common.Web
{
    public class WebRequestHelper
    {
        private static uint DefaultDownloadBufferSize = 50 * 1024;

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
            return GetHtml(info);
        }

        public static string GetHtml(WebDownLoader info)
        {
            RequestState<string> requestState = new RequestState<string>();
            
            info.DownloadAsync<string>(requestState, null);
            return html;
        }
    }

    public class WebDownLoader
    {
        public TimeSpan? RetryWaitDuration { get; set; }
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

        internal void DownloadAsync<T>(RequestState<T> requestState, Exception exception)
        {
            if (exception!=null && RetryWaitDuration.HasValue)
            {
                Thread.Sleep(RetryWaitDuration.Value);
            }
            
            try
            {
                StringBuilder content = new StringBuilder();
                requestState.Request = (HttpWebRequest)WebRequest.Create(Url);
                requestState.Request.Method = requestState.Method.ToString();
                SetDefaultRequestProperties(requestState.Request);
                HttpWebResponse response = (HttpWebResponse)requestState.Request.GetResponse();
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using(StreamReader str = new StreamReader(response.GetResponseStream()))
                    {
                        // 开始读取数据  
                        Char[] sReaderBuffer = new Char[512];
                        int count = str.Read(sReaderBuffer, 0, sReaderBuffer.Length);
                        while (count > 0)  //while ((temp = reader.ReadLine()) != null)
                        {
                            String tempStr = new String(sReaderBuffer, 0, count);
                            content.Append(tempStr);
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
                    throw new Exception("StatusCode:" + response.StatusDescription);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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
        public Stopwatch DownloadTimer { get; set; }
        public HttpWebRequest Request { get; set; }
        public Action<RequestState<T>> Complete { private get; set; }
        public DownloadMethod Method { get; set; }
    }
}
