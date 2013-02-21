using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataAccess.Schema.Mysql
{
    public class MySqlProvider:SchemaProviderBase
    {
        internal MySqlProvider()
        {

        }

        public override IList<DataBaseSchema> GetDataBases(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            throw new NotImplementedException();
        }

        public override IList<TableSchema> GetTables(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            throw new NotImplementedException();
        }

        public override IList<ViewSchema> GetViews(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            throw new NotImplementedException();
        }

        public override IList<ProceduresSchema> GetProcedures(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            throw new NotImplementedException();
        }

        public override IList<TriggersSchema> GetTriggers(System.Data.Common.DbConnectionStringBuilder connectionstr)
        {
            throw new NotImplementedException();
        }

        public override IList<ColumnSchema> GetColumns(System.Data.Common.DbConnectionStringBuilder connectionstr, string tablename)
        {
            throw new NotImplementedException();
        }
    }
}
