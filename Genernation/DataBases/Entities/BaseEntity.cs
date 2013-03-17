using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity(string name, string description)
        {
            this.name = name;
            this.description = description;
        }
        protected string name;
        public virtual string Name
        {
            get
            {
                return this.name;
            }
        }

        protected string description;
        public virtual string Description
        {
            get
            {
                return this.description;
            }
            internal set
            {
                description = value;
            }
        }

        protected DbConnectionStringBuilder connectionstringbuilder;
        public virtual DbConnectionStringBuilder DbConnectionStringBuilder
        {
            get
            {
                return connectionstringbuilder;
            }
        }
    }
}
