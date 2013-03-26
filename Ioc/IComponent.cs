using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public interface IComponent
    {
        IComponent Implement<T>();
        IComponent Name(string name);
		IComponent To<T>();
		IComponent To(params Type[] types);
		IComponent Property(params InjectProperty[] property);
		IComponent Method(params InjectMethod[] method);
		void Singleton();
    }

}
