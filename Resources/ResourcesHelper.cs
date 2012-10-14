using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace LCW.Framework.Common.Resources
{
    public class ResourcesHelper
    {
        /// <summary>
        /// 获取所有资源的名称
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static string[] ReadResources<T>()
        {
            return typeof(T).Assembly.GetManifestResourceNames();
        }

        /// <summary>
        /// 获取指定程序集的嵌入资源
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filename"></param>
        /// <returns></returns>
        public static Stream ReadResources<T>(string filename)
        {
            return typeof(T).Assembly.GetManifestResourceStream(typeof(T).Assembly.GetName().Name + "." + filename);
        }
    }
}
