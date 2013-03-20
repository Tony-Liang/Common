using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public interface IContainer
    {
        T Resolve<T>();
        void Register<TContract,TImplementation>()where TImplementation:TContract;
        void Register<TContract,TImplementation>(TImplementation instance) where TImplementation : TContract;
    }
}
