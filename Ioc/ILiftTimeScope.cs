using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public interface ILiftTimeScope
    {
        event Executed ComponentExecuted; 
        object CreateInstance(Func<object> func);
    }

}
