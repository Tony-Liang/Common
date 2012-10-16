using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common
{
    internal static class CooperationWrapper
    {
        public static void IsNullOrEmpty(string fullpath)
        {
            if (string.IsNullOrEmpty(fullpath))
                throw new ArgumentNullException("filepath");
        }

        public static void WriteLog(Exception ex)
        {
            WriteLog(ex.ToString());
        }

        public static void WriteLog(string ex)
        {

        }
    }
}
