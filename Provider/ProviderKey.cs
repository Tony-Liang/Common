using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Provider
{
    /// <summary>
    /// ProviderKey
    /// </summary>
    [Serializable]
    public class ProviderKey
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; private set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public Type Type { get; private set; }

        private ProviderKey()
        {

        }

        static ProviderKey()
        {
            //ServerType = new ProviderKey { Name = "ServerType", Type = typeof(IAppServer) };
        }
    }
}
