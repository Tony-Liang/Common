using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Driver;

namespace LCW.Framework.Common.Genernation.DataBases
{
    public sealed class DbType
    {
        public static Type SqlParse(string type)
        {
            var list = DBDriverProvice.GetInstance.Languages.ContainsKey("SQL", "C# System Types");
            if (list != null)
            {
                foreach (var item in list.Languages)
                {
                    if (item.Form == type)
                        return item.To;
                }
            }
            return null;
        }

        public static Type MySqlParse(string type)
        {
            var list = DBDriverProvice.GetInstance.Languages.ContainsKey("MYSQL2", "MySQL Connector/Net (C#)");
            if (list != null)
            {
                foreach (var item in list.Languages)
                {
                    if (item.Form == type)
                        return item.To;
                }
            }
            return null;
        }
    }
}
