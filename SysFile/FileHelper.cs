using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LCW.Framework.Common.SysFile
{
    public static class FileHelper
    {
        public static bool IsExistFile(string filename)
        {
            CooperationWrapper.IsNullOrEmpty(filename);
            return File.Exists(filename);
        }

        public static bool DeleteFile(string filePath)
        {
            if (IsExistFile(filePath))
            {
                try
                {
                   File.Delete(filePath);
                   return true;
                }
                catch (Exception ex)
                {
                    CooperationWrapper.WriteLog(ex);
                }       
            }
            return false;
        }

        public static void Copy(string sourceFileName, string destFileName, bool overwrite)
        {
            try
            {
                System.IO.File.Copy(sourceFileName, destFileName, overwrite);
            }
            catch (Exception ex)
            {
                CooperationWrapper.WriteLog(ex);
            }
        }

        public static void Copy(string sourceFileName, string destFileName)
        {
            Copy(sourceFileName, destFileName, true);
        }

        public static string ReadFile(string filePath)
        {
            return ReadFile(filePath, Encoding.UTF8);
        }

        public static string ReadFile(string filePath, Encoding encoding)
        {
            if (IsExistFile(filePath))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    using (StreamReader sr = new StreamReader(filePath, encoding))
                    {
                        string temp = string.Empty;
                        while ((temp = sr.ReadLine()) != null)
                        {
                            sb.AppendLine(temp);
                        }
                    }

                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    CooperationWrapper.WriteLog(ex);             
                }
            }
            throw new FileNotFoundException("file not found");
        }

        public static void WriteFile(string filePath, string content)
        {
            WriteFile(filePath, content, FileMode.Append);
        }

        public static void WriteFile(string filePath, string content, FileMode fileModel)
        {
            WriteFileWithEncoding(filePath, content, fileModel, new UTF8Encoding(true));
        }

        public static void WriteFileWithEncoding(string filePath, string content, FileMode fileModel, Encoding encoding)
        {
            try
            {
                if (IsExistFile(filePath))
                {
                    using (StreamWriter sw = System.IO.File.AppendText(filePath))
                    {
                        TextWriter tw = TextWriter.Synchronized(sw);
                        tw.Write(content);
                        tw.Close();
                    }
                }
                else
                {
                    DirectoryHelper.CreateDir(filePath.Substring(0, filePath.LastIndexOf(@"\")));
                    FileStream fs = System.IO.File.Open(filePath, fileModel, FileAccess.Write);
                    StreamWriter sw = new StreamWriter(fs, encoding);
                    sw.Flush();
                    sw.Write(content);
                    sw.Flush();
                    sw.Close();
                }
            }
            catch (Exception ex)
            {
                CooperationWrapper.WriteLog(ex);
            }
        }

        public static string AsyncReadFile(string filePath)
        {
            return AsyncReadFile(filePath, Encoding.UTF8);
        }

        public static string AsyncReadFile(string filePath, Encoding encoding)
        {
            if (IsExistFile(filePath))
            {
                try
                {
                    StringBuilder sb = new StringBuilder();
                    int bufferSize = 1024;
                    byte[] data = new byte[bufferSize];
                    FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 20480, true);//设置异步调用true
                    AsyncCallback callback = null;
                    callback = asyncResult =>
                    {
                        FileStream read = (FileStream)asyncResult.AsyncState;
                        int bytes=read.EndRead(asyncResult);
                        sb.Append(encoding.GetString(data,0,bytes));//Encoding.ASCII.GetString(data,0,bytes)
                        //System.Threading.Thread.Sleep(2000);
                        if (bytes > 0)
                        {
                            read.BeginRead(data, 0, bufferSize, callback, read);                            
                        }
                        else
                        {
                            read.Close(); Console.WriteLine(sb.ToString());
                        }
                    };

                    IAsyncResult async=fs.BeginRead(data, 0, bufferSize, callback, fs);
                    return sb.ToString();
                }
                catch (Exception ex)
                {
                    CooperationWrapper.WriteLog(ex);
                }
            }
            throw new FileNotFoundException("file not found");
        }

        public static void AsyncWriteFile(string filePath, string content)
        {
            AsyncWriteFile(filePath, content, FileMode.Append);
        }

        public static void AsyncWriteFile(string filePath, string content, FileMode fileModel)
        {
            AsyncWriteFileWithEncoding(filePath, content, fileModel, new UTF8Encoding(true));
        }

        public static void AsyncWriteFileWithEncoding(string filePath, string content, FileMode fileModel, Encoding encoding)
        {
            try
            {
                FileStream fs =null;
                int bufferSize = 1024;
                //byte[] data = new byte[bufferSize];
                byte[] data = encoding.GetBytes(content);//Encoding.ASCII.GetBytes(content);
                
                IAsyncResult result = null;
                AsyncCallback callback=null;
                callback=asyncresult=>{
                    int datalength = (int)asyncresult.AsyncState;
                    fs.EndWrite(asyncresult);
                    datalength -=bufferSize;
                    if (datalength > 0)
                        fs.BeginWrite(data, data.Length - datalength, datalength < bufferSize ? datalength : bufferSize, callback, datalength);
                    else
                        fs.Close();
                };
                if (IsExistFile(filePath))
                {
                    fs= new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 20480, true);
                    result = fs.BeginWrite(data, 0, bufferSize, callback, data.Length);
                }
                else
                {
                    DirectoryHelper.CreateDir(filePath.Substring(0, filePath.LastIndexOf(@"\")));
                    fs = new FileStream(filePath, FileMode.Create, FileAccess.ReadWrite, FileShare.None, 20480, true);
                    result = fs.BeginWrite(data, 0, bufferSize, callback, data.Length);
                }
            }
            catch (Exception ex)
            {
                CooperationWrapper.WriteLog(ex);
            }
        }

        public static bool WriteFile(byte[] bytes, string filename)
        {
            FileStream stream = null;
            try
            {
                using (stream = new FileStream(filename, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    stream.Write(bytes, 0, bytes.Length);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (stream != null)
                {
                    stream.Close();
                }
            }
            return true;
        }
    }
}
