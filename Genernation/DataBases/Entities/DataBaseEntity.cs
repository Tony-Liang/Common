using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public class DataBaseEntity:BaseEntity
    {
        public DataBaseEntity(DbConnectionStringBuilder connectionstringbuilder,string name)
            : base(name, name)
        {
            this.connectionstringbuilder = connectionstringbuilder;
        }

        private ServiceSite service;
        public ServiceSite Service
        {
            get
            {
                return service;
            }
            set
            {
                service = value;
            }
        }

        private IList<TableEntity> tables;
        public IList<TableEntity> Tables
        {
            get
            {
                if (tables == null)
                {
                    tables = DbSchema.GetInstance(this.connectionstringbuilder).GetTables(this);
                }
                return tables;
            }
        }

        private IList<ProcedureEntity> procedures;
        public IList<ProcedureEntity> Procedures
        {
            get
            {
                if (procedures == null)
                {
                    procedures = DbSchema.GetInstance(this.connectionstringbuilder).GetProcedures(this);
                }
                return procedures;
            }
        }

        private IList<TriggersEntity> triggers;
        public IList<TriggersEntity> Triggers
        {
            get
            {
                if (triggers == null)
                {
                    triggers = DbSchema.GetInstance(this.connectionstringbuilder).GetTriggers(this);
                }
                return triggers;
            }
        }

        private IList<ViewEntity> views;
        public IList<ViewEntity> Views
        {
            get
            {
                if (views == null)
                {
                    views = DbSchema.GetInstance(this.connectionstringbuilder).GetViews(this);
                }
                return views;
            }
        }

        public override void Refresh()
        {
            this.tables = null;
            this.triggers = null;
            this.views = null;
            this.procedures = null;
        }
    }
}
