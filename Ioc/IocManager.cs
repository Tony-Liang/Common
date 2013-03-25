using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public class IocManager:IocContainer
    {

        public IContainer Build()
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public IComponent Register<T>()
        {
            throw new NotImplementedException();
        }

        public IComponent Register<TContract, TImplementation>() where TImplementation : TContract
        {
            throw new NotImplementedException();
        }

        public T Resolve<T>()
        {
            throw new NotImplementedException();
        }
    }
}
