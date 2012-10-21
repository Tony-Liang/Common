using System;
using System.Web;
using System.Threading;
using System.Diagnostics;
using Microsoft.Win32;
using System.Runtime.InteropServices;

namespace LCW.Framework.Common.Util
{
    /// <summary>
    /// 系统操作相关的公共类
    /// </summary>    
    public static class SysHelper
    {
        #region 获取文件相对路径映射的物理路径
        /// <summary>
        /// 获取文件相对路径映射的物理路径
        /// </summary>
        /// <param name="virtualPath">文件的相对路径</param>        
        public static string GetPath(string virtualPath)
        {

            return HttpContext.Current.Server.MapPath(virtualPath);

        }
        #endregion

        #region 获取指定调用层级的方法名
        /// <summary>
        /// 获取指定调用层级的方法名
        /// </summary>
        /// <param name="level">调用的层数</param>        
        public static string GetMethodName(int level)
        {
            //创建一个堆栈跟踪
            StackTrace trace = new StackTrace();

            //获取指定调用层级的方法名
            return trace.GetFrame(level).GetMethod().Name;
        }
        #endregion

        #region 获取GUID值
        /// <summary>
        /// 获取GUID值
        /// </summary>
        public static string NewGUID
        {
            get
            {
                return Guid.NewGuid().ToString();
            }
        }
        #endregion

        #region 获取换行字符
        /// <summary>
        /// 获取换行字符
        /// </summary>
        public static string NewLine
        {
            get
            {
                return Environment.NewLine;
            }
        }
        #endregion

        #region 获取当前应用程序域
        /// <summary>
        /// 获取当前应用程序域
        /// </summary>
        public static AppDomain CurrentAppDomain
        {
            get
            {
                return Thread.GetDomain();
            }
        }
        #endregion

        #region 获取操作系统当前版本信息
        /// <summary>
        /// 获取操作系统当前版本信息
        /// </summary>
        public static class OS
        {
            private static RegistryKey rk = Registry.LocalMachine.OpenSubKey("Software\\Microsoft\\Windows NT\\CurrentVersion");

            /// <summary>

            /// 

            /// </summary>

            public static string BuildLab
            {

                get
                {

                    return GetValue("BuildLab");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string CommonFilesDir
            {

                get
                {

                    return GetValue("CommonFilesDir");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string CSDBuildNumber
            {

                get
                {

                    return GetValue("CSDBuildNumber");

                }

            }

            /// <summary>

            /// 获取Windows SP补丁信息

            /// </summary>

            public static string CSDVersion
            {

                get
                {

                    return GetValue("CSDVersion");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string CurrentBuild
            {

                get
                {

                    return GetValue("CurrentBuild");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string CurrentBuildNumber
            {

                get
                {

                    return GetValue("CurrentBuildNumber");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string CurrentType
            {

                get
                {

                    return GetValue("CurrentType");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string CurrentVersion
            {

                get
                {

                    return GetValue("CurrentVersion");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string DevicePath
            {

                get
                {

                    return GetValue("DevicePath");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string MediaPath
            {

                get
                {

                    return GetValue("MediaPath");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string OldWinDir
            {

                get
                {

                    return GetValue("OldWinDir");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string OtherDevicePath
            {

                get
                {

                    return GetValue("OtherDevicePath");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string PathName
            {

                get
                {

                    return GetValue("PathName");

                }

            }

            /// <summary>

            /// 获取Windows ID

            /// </summary>

            public static string ProductId
            {

                get
                {

                    return GetValue("ProductId");

                }

            }

            /// <summary>

            /// 获取Windows版本信息

            /// </summary>

            public static string ProductName
            {

                get
                {

                    return GetValue("ProductName");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string ProgramFilesDir
            {

                get
                {

                    return GetValue("ProgramFilesDir");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string RegDone
            {

                get
                {

                    return GetValue("RegDone");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string RegisteredOrganization
            {

                get
                {

                    return GetValue("RegisteredOrganization");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string RegisteredOwner
            {

                get
                {

                    return GetValue("RegisteredOwner");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string SoftwareType
            {

                get
                {

                    return GetValue("SoftwareType");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string SourcePath
            {

                get
                {

                    return GetValue("SourcePath");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string SystemRoot
            {

                get
                {

                    return GetValue("SystemRoot");

                }

            }

            /// <summary>

            /// 

            /// </summary>

            public static string WallPaperDir
            {

                get
                {

                    return GetValue("WallPaperDir");

                }

            }

            /// <summary>

            /// 获取本机ODBC驱动列表

            /// </summary>

            /// <returns></returns>

            public static string[] GetOdbcList()
            {

                string _root = "SOFTWARE\\ODBC\\ODBCINST.INI\\ODBC Drivers";

                RegistryKey _rk = Registry.LocalMachine.OpenSubKey(_root);

                try
                {

                    string[] tmp_strs = _rk.GetValueNames();

                    string[] list = new string[tmp_strs.Length];



                    return tmp_strs;

                }

                catch
                {

                    string[] list = new string[1];

                    list[0] = "未知";

                    return list;

                }

            }

            /// <summary>

            /// 私有方法

            /// </summary>

            /// <param name="name"></param>

            /// <returns></returns>

            private static string GetValue(string name)
            {

                try
                {

                    return rk.GetValue(name).ToString();

                }

                catch (Exception ex)
                {

                    return "未知 [" + ex.Message + "]";

                }

            }

        }
        #endregion

        #region 获取硬件信息
        /// <summary>
        /// 获取硬件信息
        /// </summary>
        public class HardWare
        {

       /// <summary>

       /// CPU实例

       /// </summary>

       /// <param name="cpuinfo"></param>

       [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]

       public static extern void GetSystemInfo(ref CPU_INFO cpuinfo);



       /// <summary>

       /// 内存实例

       /// </summary>

       /// <param name="meminfo"></param>

       [DllImport("kernel32", CharSet = CharSet.Auto, SetLastError = true)]

       public static extern void GlobalMemoryStatus(ref MEMORY_INFO meminfo);

       /// <summary>

       /// 获取系统CPU信息

       /// </summary>

       [StructLayout(LayoutKind.Sequential)]

       public struct CPU_INFO

       {

           /// <summary>

           /// 

           /// </summary>

           public uint dwOemId;

           /// <summary>

           /// 

           /// </summary>

           public uint dwPageSize;

           /// <summary>

           /// 

           /// </summary>

           public uint lpMinimumApplicationAddress;

           /// <summary>

           /// 

           /// </summary>

           public uint lpMaximumApplicationAddress;

           /// <summary>

           /// 

           /// </summary>

           public uint dwActiveProcessorMask;

           /// <summary>

           /// 

           /// </summary>

           public uint dwNumberOfProcessors;

           /// <summary>

           /// 

           /// </summary>

           public uint dwProcessorType;

           /// <summary>

           /// 

           /// </summary>

           public uint dwAllocationGranularity;

           /// <summary>

           /// 

           /// </summary>

           public uint dwProcessorLevel;

           /// <summary>

           /// 

           /// </summary>

           public uint dwProcessorRevision;

       }



       /// <summary>

       /// 获取系统内存

       /// HardWare.MEMORY_INFO info = new HardWare.MEMORY_INFO();

       /// HardWare.GlobalMemoryStatus(ref info);

       /// </summary>

       [StructLayout(LayoutKind.Sequential)]

       public struct MEMORY_INFO

       {

           /// <summary>

           /// 

           /// </summary>

           public uint dwLength;

           /// <summary>

           /// 正在使用的内存,单位为百分比

           /// </summary>

           public uint dwMemoryLoad;

           /// <summary>

           /// 物理内存总数,单位为M   (dwTotalPhys / 0x100000)

           /// </summary>

           public uint dwTotalPhys;

           /// <summary>

           /// 可用物理内存,单位为M   (dwAvailPhys / 0x100000)

           /// </summary>

           public uint dwAvailPhys;

           /// <summary>

           /// 交换文件大小,单位为M   (dwTotalPageFile / 0x100000)

            /// </summary>

            public uint dwTotalPageFile;

            /// <summary>

            /// 交换文件可用大小,单位为M   (dwAvailPageFile / 0x100000)

            /// </summary>

            public uint dwAvailPageFile;

            /// <summary>

            /// 总虚拟内存,单位为M      (dwTotalVirtual / 0x100000)

            /// </summary>

            public uint dwTotalVirtual;

            /// <summary>

            /// 剩余虚拟内存,单位为M     (dwAvailVirtual / 0x100000)

            /// </summary>

            public uint dwAvailVirtual;

        }

        }
        #endregion

        #region 获取CPU基本信息
        /// <summary>
        /// 获取CPU基本信息
        /// </summary>
        public class CPU
        {

        private static RegistryKey _rk = Registry.LocalMachine.OpenSubKey("HARDWARE\\DESCRIPTION\\System\\CentralProcessor");



        /// <summary>

        /// 

        /// </summary>

        public static string[] Identifiers

        {

            get

            {

                string[] strs = new string[ProcessorCount];

                for (int i = 0; i < strs.Length; i++)

                {

                    strs[i] = GetValue("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + _rk.GetSubKeyNames()[i], "Identifier");

                }

                return strs;

            }

        }



        /// <summary>

        /// 获取CPU名称

        /// </summary>

        public static string[] ProcessorNames

        {

            get

            {

                string[] strs = new string[ProcessorCount];

                for (int i = 0; i < strs.Length; i++)

                {

                    strs[i] = GetValue("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + _rk.GetSubKeyNames()[i], "ProcessorNameString");

                }

                return strs;

            }

        }



        /// <summary>

        /// 

        /// </summary>

        public static string[] VendorIdentifiers

        {

            get

            {

                string[] strs = new string[ProcessorCount];

                for (int i = 0; i < strs.Length; i++)

                {

                    strs[i] = GetValue("HARDWARE\\DESCRIPTION\\System\\CentralProcessor\\" + _rk.GetSubKeyNames()[i], "VendorIdentifier");

                }

                return strs;

            }

        }



        /// <summary>

        /// 获取CPU个数

        /// </summary>

        public static int ProcessorCount

        {

            get

            {

                return _rk.GetSubKeyNames().Length;

            }

        }









        /// <summary>

        /// 私有方法

        /// </summary>

        /// <param name="path"></param>

        /// <param name="name"></param>

        /// <returns></returns>

        private static string GetValue(string path, string name)

        {

            try

            {

                RegistryKey rk = Registry.LocalMachine.OpenSubKey(path);

                return rk.GetValue(name).ToString();

            }

            catch (Exception ex)

            {

                return "未知 [" + ex.Message + "]";

            }

        }





        }
        #endregion
    }
}
