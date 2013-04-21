using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases
{
    public class DataBaseSchemaBuilder
    {
        private static ServiceSite site;

        public static ServiceSite NewInstance(DbConnectionStringBuilder connectionstringbuilder, string service)
        {
            site = new ServiceSite(connectionstringbuilder, service);
            return site;
        }
        
        public static ServiceSite GetInstance()
        {
            if (site != null)
            {
                return site;
            }
            return null;
        }
        //connection.GetSchema("resource")
    }
}
