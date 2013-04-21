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

        private Type type;
        public Type DataType
        {
            get
            {
                return type;
            }
            internal set
            {
                type = value;
            }
        }

        private bool isPrimaryKey;
        public bool IsPrimaryKey
        {
            get
            {
                return isPrimaryKey;
            }
            internal set
            {
                isPrimaryKey = value;
            }
        }

        private bool isForeignKey;
        public bool IsForeignKey
        {
            get
            {
                return isForeignKey;
            }
            internal set
            {
                isForeignKey = value;
            }
        }

        private bool isReadOnly;
        public bool IsReadOnly
        {
            get
            {
                return isReadOnly;
            }
            internal set
            {
                isReadOnly = value;
            }
        }

        private bool isIdentity;
        public bool IsIdentity
        {
            get
            {
                return isIdentity;
            }
            internal set
            {
                isIdentity = value;
            }
        }

        private bool allowDBNull;
        public bool AllowDBNull
        {
            get
            {
                return allowDBNull;
            }
            internal set
            {
                allowDBNull = value;
            }
        }

        private bool isUnique;
        public bool IsUnique
        {
            get
            {
                return isUnique;
            }
            internal set
            {
                isUnique = value;
            }
        }
    }
}
