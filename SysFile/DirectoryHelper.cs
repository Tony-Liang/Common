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
    }
}
