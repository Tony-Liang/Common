using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public class ViewEntity : BaseEntity, IScript
    {
        public ViewEntity(DbConnectionStringBuilder connectionstringbuilder,string name, string description)
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

        private string script;
        public string Script
        {
            get
            {
                if (string.IsNullOrEmpty(script))
                {
                    script = DbSchema.GetInstance(this.connectionstringbuilder).OpenView(this.name);
                }
                return script;
            }
        }

        public override void Refresh()
        {
            this.script = null;
            base.Refresh();
        }
    }
}
