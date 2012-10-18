using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace LCW.Framework.Common.SysFile
{
    public static class DirectoryHelper
    {

        public static bool IsExistDirectory(string directoryPath)
        {
            CooperationWrapper.IsNullOrEmpty(directoryPath);
            return Directory.Exists(directoryPath);
        }

        public static string[] GetFileNames(string directoryPath)
        {
            return GetFileNames(directoryPath, "*.*", SearchOption.AllDirectories);
        }

        public static string[] GetFileNames(string directoryPath, string searchPattern, SearchOption searchOption)
        {
            if (!IsExistDirectory(directoryPath))
            {
                return new string[] {};
            }
            try
            {
                return Directory.GetFiles(directoryPath, searchPattern, searchOption);
            }
            catch (Exception ex)
            {
                CooperationWrapper.WriteLog(ex);
            }
            return new string[] { };
        }

        public static string[] GetDirectories(string directoryPath)
        {
            return GetDirectories(directoryPath,"*.*",SearchOption.AllDirectories);
        }

        public static string[] GetDirectories(string directoryPath, string searchPattern, SearchOption searchOption)
        {
            if (!IsExistDirectory(directoryPath))
            {
                return new string[] {};
            }
            try
            {
                return Directory.GetDirectories(directoryPath,searchPattern,searchOption);
            }
            catch (Exception ex)
            {
                CooperationWrapper.WriteLog(ex);
            }
            return new string[] { };
        }

        public static bool IsEmptyDirectory(string directoryPath)
        {
            try
            {
                if (GetFileNames(directoryPath).Length > 0)
                {
                    return false;
                }

                if (GetDirectories(directoryPath).Length > 0)
                {
                    return false;
                }
                return true;
            }
            catch (IOException ex)
            {
                CooperationWrapper.WriteLog(ex);            
            }
            return true;
        }

        public static bool CreateDir(string directoryPath)
        {
            CooperationWrapper.IsNullOrEmpty(directoryPath);
            if (Directory.Exists(directoryPath))
            {
                CooperationWrapper.WriteLog(new IOException(string.Format("{0} directory exist",directoryPath)));
                return true;
            }
            else
            {
                DirectoryInfo info = null;
                try
                {
                    info=Directory.CreateDirectory(directoryPath);
                }
                catch (IOException ex)
                {
                    CooperationWrapper.WriteLog(ex);
                }
                return info == null;
            }
        }

        public static bool DeleteDir(string directoryPath)
        {
            CooperationWrapper.IsNullOrEmpty(directoryPath);
            if (!Directory.Exists(directoryPath))
            {
                CooperationWrapper.WriteLog(new IOException(string.Format("{0} directory isn't exist", directoryPath)));
                return true;
            }
            else
            {
                try
                {
                   Directory.Delete(directoryPath,true);
                }
                catch (IOException ex)
                {
                    CooperationWrapper.WriteLog(ex);
                    return false;
                }
                return true;
            }
        }

        public static bool CopyDirectory(string sourcePath, string destinationPath)
        {
            return CopyDirectory(sourcePath, destinationPath, true);
        }

        public static bool CopyDirectory(string sourcePath, string destinationPath, bool overwriteexisting)
        {
            bool ret = false;
            try
            {
                sourcePath = sourcePath.EndsWith(@"\") ? sourcePath : sourcePath + @"\";
                destinationPath = destinationPath.EndsWith(@"\") ? destinationPath : destinationPath + @"\";

                if (IsExistDirectory(sourcePath))
                {
                    if (IsExistDirectory(destinationPath) == false)
                        Directory.CreateDirectory(destinationPath);

                    foreach (string fls in Directory.GetFiles(sourcePath))
                    {
                        FileInfo flinfo = new FileInfo(fls);
                        flinfo.CopyTo(destinationPath + flinfo.Name, overwriteexisting);
                    }
                    foreach (string drs in Directory.GetDirectories(sourcePath))
                    {
                        DirectoryInfo drinfo = new DirectoryInfo(drs);
                        if (CopyDirectory(drs, destinationPath + drinfo.Name, overwriteexisting) == false)
                            ret = false;
                    }
                }
                ret = true;
            }
            catch (Exception ex)
            {
                CooperationWrapper.WriteLog(ex);
                ret = false;
            }
            return ret;
        }
    }
}
