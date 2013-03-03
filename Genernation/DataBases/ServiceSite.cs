using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Genernation.DataBases.Entities;
using System.Data.Common;

namespace LCW.Framework.Common.Genernation.DataBases
{
    public class ServiceSite:BaseEntity
    {
        internal ServiceSite(DbConnectionStringBuilder connectionstringbuilder,string name):base(name,name)
        {
            this.name = name;
            this.connectionstringbuilder = connectionstringbuilder;
        }

        public IList<DataBaseEntity> databases;
        public IList<DataBaseEntity> DataBases
        {
            get
            {
                if (databases == null)
                {
                    databases=DbSchema.GetInstance(this.connectionstringbuilder).GetDataBases(this);
                }
                return databases;
            }
        }
    }
}
