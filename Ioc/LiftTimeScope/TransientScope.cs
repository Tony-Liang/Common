using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc.LiftTimeScope
{
    internal class TransientScope : ILiftTimeScope
    {       
        private event Executed componentexecuted;
        public event Executed ComponentExecuted
        {
            add
            {
                componentexecuted += value;
            }
            remove
            {
                componentexecuted -= value;
            }
        }
        
        public object CreateInstance(Func<object> func)
        {
            object obj=func();
            if (componentexecuted != null)
                componentexecuted(obj);
            return obj;
        }
    }
}
