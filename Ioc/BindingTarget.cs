using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    internal class BindingTarget:IBindingTarget
    {
		public BindingTarget(Type type,InjectProperty[] property,InjectMethod[] method)
        {
            this.type = type;
			this.property = property;
			this.method = method;
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

		private InjectProperty[] property;
		public InjectProperty[] Property
		{
			get
			{
				return property;
			}
		}

		#region IBindingTarget 成员

		private InjectMethod[] method;
		public InjectMethod[] Method
		{
			get
			{
				return method;
			}
		}

		#endregion
	}
}
