using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Configuration;

namespace LCW.Framework.Common.Util
{
    public class ConfigurationHelper
    {
        private static IDictionary<string, NameValueCollection> dic;
        static ConfigurationHelper()
        {
            dic = new Dictionary<string, NameValueCollection>();
        }
        /// <summary>
        /// <section name="Setting" type="System.Configuration.NameValueSectionHandler, System, Version=1.0.5000.0,Culture=neutral, PublicKeyToken=b77a5c561934e089" requirePermission="false"></section>
        /// </summary>
        /// <param name="section"></param>
        /// <returns></returns>
        public static NameValueCollection GetSection(string section)
        {
            NameValueCollection collection=null; 
            if(!dic.TryGetValue(section,out collection))
            {
                try
                {
                    collection = ConfigurationManager.GetSection(section) as NameValueCollection;
                    if(collection!=null)
                        dic.Add(section, collection);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return collection;
        }
    }
}
