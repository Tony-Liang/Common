using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Ioc
{
    public class IocManager
    {
        private IContainer container;
        public IContainer Container
        {
            get
            {
                return container;
            }
        }
        public static IocManager Instance
        {
            get
            {
                return IocWrapper.ioc;
            }
        }

        #region #Nested
        private class IocWrapper
        {
            internal static IocManager ioc = new IocManager();
            static IocWrapper()
            {
                ioc.container = new Container();
            }           
        }
        #endregion
    }
}
