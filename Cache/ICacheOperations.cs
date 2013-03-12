using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LCW.Framework.Common.Cache
{
    public interface ICacheOperations
    {
        /// <summary>
        /// Gets the current cache state.
        /// </summary>
        /// <returns></returns>
        Hashtable CurrentCacheState { get; }

        /// <summary>
        /// Removes a <see cref="CacheItem"/>.
        /// </summary>
        /// <param name="key">The key of the item to remove.</param>
        /// <param name="removalReason">One of the <see cref="CacheItemRemovedReason"/> values.</param>
        void RemoveItemFromCache(string key, CacheItemRemovedReason removalReason);

        /// <summary>
        /// Returns the number of items contained in the cache.
        /// </summary>
        int Count { get; }
    }
}
