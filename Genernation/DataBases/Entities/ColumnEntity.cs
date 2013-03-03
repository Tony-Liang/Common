using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public class ColumnEntity:BaseEntity
    {
        public ColumnEntity(DbConnectionStringBuilder connectionstringbuilder,string name)
            : base(name, name)
        {
            this.connectionstringbuilder = connectionstringbuilder;
        }

        private TableEntity table;
        public TableEntity Table
        {
            get
            {
                return table;
            }
            set
            {
                table = value;
            }
        }
    }
}
