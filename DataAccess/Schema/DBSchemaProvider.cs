using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataAccess.Schema
{
    public class DBSchemaProvider<T> where T : SchemaProviderBase
    {
        private static T provider;

        public static T GetInstance()
        {
            if (provider == null)
            {
                try
                {
                    provider =(T)Activator.CreateInstance(typeof(T),true);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return provider;
        }
    }


}
