using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace LCW.Framework.Common.Cache
{
    public class ExpirationTask
    {
        private ICacheOperations cacheOperations;
        //private ICachingInstrumentationProvider instrumentationProvider;

        /// <summary>
        /// Initialize an instance of the <see cref="ExpirationTask"/> class with an <see cref="ICacheOperations"/> object.
        /// </summary>
        /// <param name="cacheOperations">An <see cref="ICacheOperations"/> object.</param>
        /// <param name="instrumentationProvider">An instrumentation provider.</param>
        public ExpirationTask(ICacheOperations cacheOperations)
        {
            this.cacheOperations = cacheOperations;
            //this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// Perform the cacheItemExpirations.
        /// </summary>
        public void DoExpirations()
        {
            Hashtable liveCacheRepresentation = cacheOperations.CurrentCacheState;
            MarkAsExpired(liveCacheRepresentation);
            PrepareForSweep();
            int expiredItemsCount = SweepExpiredItemsFromCache(liveCacheRepresentation);

            //if (expiredItemsCount > 0) instrumentationProvider.FireCacheExpired(expiredItemsCount);
        }

        /// <summary>
        /// Mark each <see cref="CacheItem"/> as expired. 
        /// </summary>
        /// <param name="liveCacheRepresentation">The set of <see cref="CacheItem"/> objects to expire.</param>
        /// <returns>
        /// The number of items marked.
        /// </returns>
        public virtual int MarkAsExpired(Hashtable liveCacheRepresentation)
        {
            if (liveCacheRepresentation == null) throw new ArgumentNullException("liveCacheRepresentation");

            int markedCount = 0;
            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                lock (cacheItem)
                {
                    if (cacheItem.HasExpired())
                    {
                        markedCount++;
                        cacheItem.WillBeExpired = true;
                    }
                }
            }

            return markedCount;
        }

        /// <summary>
        /// Sweep and remove the <see cref="CacheItem"/>s.
        /// </summary>
        /// <param name="liveCacheRepresentation">
        /// The set of <see cref="CacheItem"/> objects to remove.
        /// </param>
        public virtual int SweepExpiredItemsFromCache(Hashtable liveCacheRepresentation)
        {
            if (liveCacheRepresentation == null) throw new ArgumentNullException("liveCacheRepresentation");

            int expiredItems = 0;

            foreach (CacheItem cacheItem in liveCacheRepresentation.Values)
            {
                if (RemoveItemFromCache(cacheItem))
                    expiredItems++;
            }

            return expiredItems;
        }

        /// <summary>
        /// Prepare to sweep the <see cref="CacheItem"/>s.
        /// </summary>
        public virtual void PrepareForSweep()
        {
        }

        private bool RemoveItemFromCache(CacheItem itemToRemove)
        {
            bool expired = false;

            lock (itemToRemove)
            {
                if (itemToRemove.WillBeExpired)
                {
                    try
                    {
                        expired = true;
                        cacheOperations.RemoveItemFromCache(itemToRemove.Key, CacheItemRemovedReason.Expired);
                    }
                    catch (Exception e)
                    {
                        //instrumentationProvider.FireCacheFailed(Resources.FailureToRemoveCacheItemInBackground, e);
                    }
                }
            }

            return expired;
        }
    }
}
