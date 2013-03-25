using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public interface IocContainer : IDisposable, IComponentRegistry
    {
        IContainer Build();
    }
}
