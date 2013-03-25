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
        object Resolve(Type type);
    }
}
