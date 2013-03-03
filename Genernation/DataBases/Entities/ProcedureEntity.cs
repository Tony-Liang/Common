using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public class ProcedureEntity:BaseEntity
    {
        public ProcedureEntity(DbConnectionStringBuilder connectionstringbuilder, string name, string description)
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

        private string content;
        public string Content
        {
            get
            {
                if (string.IsNullOrEmpty(content))
                {
                    content = DbSchema.GetInstance(this.connectionstringbuilder).OpenProcedure(this.name);
                }
                return content;
            }
        }
    }
}
