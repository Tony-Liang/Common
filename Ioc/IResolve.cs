using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public interface IResolve
    {
        T Resolve<T>();
        T Resolve<T>(string name);
		K Resolve<V,K>() where K: V;
		object Resolve(Type V,Type K);
        object Resolve(Type type);
		object Resolve(Type type,string name);
    }
}
