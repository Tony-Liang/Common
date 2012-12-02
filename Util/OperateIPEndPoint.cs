using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.NetworkInformation;

namespace LCW.Framework.Common.Util
{
    public class OperateIPEndPoint
    {
        /// <summary>  
        /// 获取本机已被使用的网络端点  
        /// </summary>  
        public IList<IPEndPoint> GetUsedIPEndPoint()
        {
            //获取一个对象，该对象提供有关本地计算机的网络连接和通信统计数据的信息。  
            IPGlobalProperties ipGlobalProperties = IPGlobalProperties.GetIPGlobalProperties();

            //获取有关本地计算机上的 Internet 协议版本 4 (IPV4) 传输控制协议 (TCP) 侦听器的终结点信息。  
            IPEndPoint[] ipEndPointTCP = ipGlobalProperties.GetActiveTcpListeners();

            //获取有关本地计算机上的 Internet 协议版本 4 (IPv4) 用户数据报协议 (UDP) 侦听器的信息。  
            IPEndPoint[] ipEndPointUDP = ipGlobalProperties.GetActiveUdpListeners();

            //获取有关本地计算机上的 Internet 协议版本 4 (IPV4) 传输控制协议 (TCP) 连接的信息。  
            TcpConnectionInformation[] tcpConnectionInformation = ipGlobalProperties.GetActiveTcpConnections();

            IList<IPEndPoint> allIPEndPoint = new List<IPEndPoint>();
            foreach (IPEndPoint iep in ipEndPointTCP) allIPEndPoint.Add(iep);
            foreach (IPEndPoint iep in ipEndPointUDP) allIPEndPoint.Add(iep);
            foreach (TcpConnectionInformation tci in tcpConnectionInformation) allIPEndPoint.Add(tci.LocalEndPoint);

            return allIPEndPoint;
        }

        /// <summary>  
        /// 判断指定的网络端点（只判断端口）是否被使用  
        /// </summary>  
        public bool IsUsedIPEndPoint(int port)
        {
            foreach (IPEndPoint iep in GetUsedIPEndPoint())
            {
                if (iep.Port == port)
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>  
        /// 判断指定的网络端点（判断IP和端口）是否被使用  
        /// </summary>  
        public bool IsUsedIPEndPoint(string ip, int port)
        {
            foreach (IPEndPoint iep in GetUsedIPEndPoint())
            {
                if (iep.Address.ToString() == ip && iep.Port == port)
                {
                    return true;
                }
            }
            return false;
        }
    }  
}
