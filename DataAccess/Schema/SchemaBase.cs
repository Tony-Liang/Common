using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataAccess.Schema
{
    public abstract class SchemaBase
    {
        public SchemaBase(string name,string description)
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
        }
    }
}
