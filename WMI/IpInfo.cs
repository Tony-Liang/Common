using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.WMI
{
    public class IpInfo
    {
        public IpInfo()
        {
        }

        public IpInfo(string ip, string mac)
        {
            m_MACAddress = mac;
            m_IPAddress = ip;
        }

        private String m_MACAddress;
        /// <summary>
        /// 物理地址
        /// </summary>
        public String MACAddress
        {
            get { return m_MACAddress; }
            set { m_MACAddress = value; }
        }

        private String m_IPAddress;
        /// <summary>
        /// IP 地址
        /// </summary>
        public String IPAddress
        {
            get { return m_IPAddress; }
            set { m_IPAddress = value; }
        }
    }
}
