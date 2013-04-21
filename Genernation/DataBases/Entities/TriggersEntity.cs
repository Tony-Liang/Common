using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public class TriggersEntity : BaseEntity, IScript
    {
        public TriggersEntity(DbConnectionStringBuilder connectionstringbuilder,string name, string description)
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
                    script = DbSchema.GetInstance(this.connectionstringbuilder).OpenTriggers(this.name);
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
