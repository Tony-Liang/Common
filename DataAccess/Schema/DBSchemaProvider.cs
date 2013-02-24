using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Util;

namespace LCW.Framework.Common.DataAccess.Schema
{
    public class DBSchemaProvider
    {
        private static SchemaProviderBase provider;

        public static SchemaProviderBase GetInstance()
        {
            if (provider == null)
            {
                try
                {
                    string typename = AppSettingsHelper.GetString("SchemaProvider", "LCW.Framework.Common.DataAccess.Schema.Sql.SqlProvider");
                    provider = Instance(typename);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return provider;
        }

        private static SchemaProviderBase Instance(string classname)
        {
            try
            {
                Type type = Type.GetType(classname);
                provider = (SchemaProviderBase)Activator.CreateInstance(type, true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return provider;
        }

        public static SchemaProviderBase NewInstance(string classname)
        {
            return Instance(classname);
        }
    }
}
