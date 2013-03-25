using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    internal interface IComponentInterpreter
    {
        object Execute(Component component);
    }
}
