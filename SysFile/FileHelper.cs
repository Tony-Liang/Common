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
    }
}
