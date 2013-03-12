using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Cache
{
    public enum CacheItemRemovedReason
    {
        /// <summary>
        /// The item has expired.
        /// </summary>
        Expired,

        /// <summary>
        /// The item was manually removed from the cache.
        /// </summary>
        Removed,

        /// <summary>
        /// The item was removed by the scavenger because it had a lower priority that any other item in the cache.
        /// </summary>
        Scavenged,

        /// <summary>
        /// Reserved. Do not use.
        /// </summary>
        Unknown = 9999
    }
}
