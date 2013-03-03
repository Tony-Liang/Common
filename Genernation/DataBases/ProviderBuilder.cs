using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using LCW.Framework.Common.Genernation.DataBases.Entities;
namespace LCW.Framework.Common.Genernation.DataBases
{
    public abstract class ProviderBuilder
    {
        public ProviderBuilder(DbConnectionStringBuilder connectionstringbuilder)
        {
            this.connectionbuilder = connectionstringbuilder;
        }

        private DbConnectionStringBuilder connectionbuilder;
        public DbConnectionStringBuilder DbConnectionStringBuilder 
        {
            get
            {
                return connectionbuilder;
            }
        }

        public abstract bool CheckConnection();
        public abstract string OpenProcedure(string procedure);
        public abstract string OpenTriggers(string triggers);
        public abstract string OpenView(string view);


        public abstract IList<DataBaseEntity> GetDataBases(ServiceSite site);
        public abstract IList<TableEntity> GetTables(DataBaseEntity database);
        public abstract IList<ColumnEntity> GetColumns(TableEntity table);
        public abstract IList<ViewEntity> GetViews(DataBaseEntity database);
        public abstract IList<ProcedureEntity> GetProcedures(DataBaseEntity database);
        public abstract IList<TriggersEntity> GetTriggers(DataBaseEntity database);
    }
}
