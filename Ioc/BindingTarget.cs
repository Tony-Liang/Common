using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    internal class BindingTarget:IBindingTarget
    {
        public BindingTarget(Type type)
        {
            this.type = type;
        }
        private Type type;
        public Type Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;
            }
        }
    }
}
