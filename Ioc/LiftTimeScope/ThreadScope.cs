using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc.LiftTimeScope
{
    internal class ThreadScope : ILiftTimeScope
    {

        public object CreateInstance(Func<object> func)
        {
            throw new NotImplementedException();
        }

        private event Executed componentexecuted;
        public event Executed ComponentExecuted
        {
            add
            {
                componentexecuted+=value;
            }
            remove
            {
                componentexecuted-=value;
            }
        }
    }
}
