using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public interface IComponentRegistry
    {
        IComponent Register<T>();
        IComponent Register<TContract, TImplementation>() where TImplementation : TContract;
    }
}
