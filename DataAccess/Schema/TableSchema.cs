using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.DataAccess.Schema
{
    [Serializable]
    public class DataBaseSchema : SchemaBase
    {
        public DataBaseSchema(string name, string description)
            : base(name, description)
        {
        }
    }

    [Serializable]
    public class TableSchema:SchemaBase
    {
        public TableSchema(string name, string description)
            : base(name, description)
        {
        }
    }

    [Serializable]
    public class ViewSchema : SchemaBase
    {
        public ViewSchema(string name,string description)
            : base(name,description)
        {
        }
    }

    [Serializable]
    public class ProceduresSchema : SchemaBase
    {
        public ProceduresSchema(string name, string description)
            : base(name,description)
        {
        }
    }

    [Serializable]
    public class TriggersSchema : SchemaBase
    {
        public TriggersSchema(string name, string description)
            : base(name,description)
        {
        }
    }

    [Serializable]
    public class ColumnSchema : SchemaBase
    {
        public ColumnSchema(string name)
            : base(name,"")
        {
        }
    }
}
