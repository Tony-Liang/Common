using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Cache
{
    public interface ICacheScavenger
    {
        /// <summary>
        /// Starts the scavenging process.
        /// </summary>
        void StartScavenging();
    }
}
