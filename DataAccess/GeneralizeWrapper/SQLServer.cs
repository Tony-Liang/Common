using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataAccess.GeneralizeWrapper
{
    public static class SQLServer
    {
        public static string DoesDatabaseExist(string Database, string ConnectionString)
        {
            return "SELECT * FROM Master.sys.Databases WHERE name=@Name";
        }

        public static string DoesTableExist(string Table, string ConnectionString)
        {
            return "SELECT * FROM sys.Tables WHERE name=@Name";
        }

        public static string DoesViewExist(string View, string ConnectionString)
        {
            return "SELECT * FROM sys.views WHERE name=@Name";
        }

        public static string DoesStoredProcedureExist(string StoredProcedure, string ConnectionString)
        {
            return "SELECT * FROM sys.Procedures WHERE name=@Name";
        }

        public static string DoesTriggerExist(string Trigger, string ConnectionString)
        {
            return "SELECT * FROM sys.triggers WHERE name=@Name";
        }


    }
}
