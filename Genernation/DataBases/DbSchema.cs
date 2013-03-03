using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using System.Data.SqlClient;
using MySql.Data.MySqlClient;
namespace LCW.Framework.Common.Genernation.DataBases
{
    internal class DbSchema
    {
        public static ProviderBuilder GetInstance(DbConnectionStringBuilder connectionstringbuilder)
        {
            if (connectionstringbuilder is SqlConnectionStringBuilder)
                return new SqlProvider(connectionstringbuilder);
            else if (connectionstringbuilder is MySqlConnectionStringBuilder)
                return new MysqlProvider(connectionstringbuilder);
            return null;
        }
    }
}
