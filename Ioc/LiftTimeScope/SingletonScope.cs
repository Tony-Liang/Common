using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc.LiftTimeScope
{
    internal class SingletonScope : ILiftTimeScope
    {       
        private static object obj = new object();
        private object temp=null;
        public object CreateInstance(Func<object> func)
        {
            if (temp == null)
            {
                lock (obj)
                {
                    if (temp == null && func!=null)
                    {
                        temp=func();
                        if (componentexecuted != null)
                            componentexecuted(temp);
                    }
                }
            }
            return temp;
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
