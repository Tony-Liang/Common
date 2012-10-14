using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.WMI
{
    public class DiskInfo
    {
        public DiskInfo(string DiskName, long Size, long FreeSpace)
        {
            this.DiskName = DiskName;
            this.Size = Size;
            this.FreeSpace = FreeSpace;
        }

        private String m_DiskName;
        public String DiskName
        {
            get { return m_DiskName; }
            set { m_DiskName = value; }
        }

        private long m_Size;
        public long Size
        {
            get { return m_Size; }
            set { m_Size = value; }
        }

        private long m_FreeSpace;
        public long FreeSpace
        {
            get { return m_FreeSpace; }
            set { m_FreeSpace = value; }
        }
    }
}
