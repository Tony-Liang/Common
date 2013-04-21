using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public class TableEntity : BaseEntity, IScript
    {
        public TableEntity(DbConnectionStringBuilder connectionstringbuilder,string name, string description)
            : base(name, description)
        {
            this.connectionstringbuilder = connectionstringbuilder;
        }

        private DataBaseEntity database;
        public DataBaseEntity DataBase
        {
            get
            {
                return database;
            }
            set
            {
                database = value;
            }
        }

        private IList<ColumnEntity> columns;
        public IList<ColumnEntity> Columns
        {
            get
            {
                if (columns == null)
                {
                    columns = DbSchema.GetInstance(this.connectionstringbuilder).GetColumns(this);
                }
                return columns;
            }
        }

        private string script;
        public string Script
        {
            get
            {
                if (string.IsNullOrEmpty(script))
                {
                    script = DbSchema.GetInstance(this.connectionstringbuilder).OpenTable(this.DataBase.Name, this.Name);
                }
                return script;
            }
        }

        public override void Refresh()
        {
            this.script = null;
            this.columns = null;
            base.Refresh();
        }
    }
}
