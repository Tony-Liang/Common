using System;
using System.Diagnostics;
using System.Management;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace LCW.Framework.Common.WMI
{
    public class SystemInfo
    {
        private IList<DiskInfo> diskinfo;
        public IList<DiskInfo> DiskInfo
        {
            get
            {
                if (diskinfo == null)
                {
                    diskinfo = SystemInfoWrapper.GetDiskDrive();
                }
                return diskinfo;
            }
        }

        private IList<ProcessInfo> processinfo;
        public IList<ProcessInfo> ProcessInfo
        {
            get
            {
                if (processinfo == null)
                {
                    processinfo = SystemInfoWrapper.GetProcess();
                }
                return processinfo;
            }
        }
    }

    internal class SystemInfoWrapper
    {
        public static IList<DiskInfo> GetDiskDrive()
        {
            IList<DiskInfo> drives = new List<DiskInfo>();
            ManagementClass diskClass = new ManagementClass("Win32_LogicalDisk");
            ManagementObjectCollection disks = diskClass.GetInstances();
            foreach (ManagementObject disk in disks)
            {
                // DriveType.Fixed 为固定磁盘(硬盘)
                if (int.Parse(disk["DriveType"].ToString()) == (int)DriveType.Fixed)
                {
                    drives.Add(new DiskInfo(disk["Name"].ToString(), long.Parse(disk["Size"].ToString()), long.Parse(disk["FreeSpace"].ToString())));
                }
            }
            return drives;
        }

        public static IList<ProcessInfo> GetProcess()
        {
            IList<ProcessInfo> list = new List<ProcessInfo>();
            Process[] plist=Process.GetProcesses();
            if (plist != null)
            {
                foreach (Process p in plist)
                {
                    list.Add(new ProcessInfo(p.Id, p.ProcessName, p.TotalProcessorTime.TotalMilliseconds, p.WorkingSet64, p.MainModule.FileName));
                }
            }
            return list;
        }

        public static List<IpInfo> GetIpInfo()
        {
            //定义范型
            List<IpInfo> ipinfos = new List<IpInfo>();
            ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
            ManagementObjectCollection moc = mc.GetInstances();
            foreach (ManagementObject mo in moc)
            {
                try
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        string mac = mo["MacAddress"].ToString().Replace(':', '-');
                        System.Array ar = (System.Array)(mo.Properties["IpAddress"].Value);
                        string ip = ar.GetValue(0).ToString();
                        ipinfos.Add(new IpInfo(ip, mac));
                    }
                }
                catch { }
            }

            return ipinfos;
        }

        public static int GetCpuCount()
        {
            try
            {
                using (ManagementClass mCpu = new ManagementClass("Win32_Processor"))
                {
                    ManagementObjectCollection cpus = mCpu.GetInstances();
                    return cpus.Count;
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            return -1;
        }

        /// <summary>
        /// 获取CPU的频率 这里之所以使用string类型的数组，主要是因为cpu的多核
        /// </summary>
        /// <returns></returns>
        public static string[] GetCpuMHZ()
        {
            ManagementClass mc = new ManagementClass("Win32_Processor");
            ManagementObjectCollection cpus = mc.GetInstances();
            string[] mHz = new string[cpus.Count];
            int c = 0;
            ManagementObjectSearcher mySearch = new ManagementObjectSearcher("select * from Win32_Processor");
            foreach (ManagementObject mo in mySearch.Get())
            {
                mHz[c] = mo.Properties["CurrentClockSpeed"].Value.ToString();
                c++;
            }
            mc.Dispose();
            mySearch.Dispose();
            return mHz;
        }
        /// <summary>
        /// 获取硬盘的大小
        /// </summary>
        /// <returns></returns>
        public static string GetSizeOfDisk()
        {
            ManagementClass mc = new ManagementClass("Win32_DiskDrive");
            ManagementObjectCollection moj = mc.GetInstances();
            foreach (ManagementObject m in moj)
            {
                return m.Properties["Size"].Value.ToString();
            }
            return "-1";
        }
        /// <summary>
        /// 获取内存的大小
        /// </summary>
        /// <returns></returns>
        public static string GetSizeOfMemery()
        {
            ManagementClass mc = new ManagementClass("Win32_OperatingSystem");
            ManagementObjectCollection moc = mc.GetInstances();
            double sizeAll = 0.0;
            foreach (ManagementObject m in moc)
            {
                if (m.Properties["TotalVisibleMemorySize"].Value != null)
                {
                    sizeAll += Convert.ToDouble(m.Properties["TotalVisibleMemorySize"].Value.ToString());
                }
            }
            mc = null;
            moc.Dispose();
            return sizeAll.ToString();
        }
    }
}
