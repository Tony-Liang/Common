using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using LCW.Framework.Common.Cache.Expirations;
namespace LCW.Framework.Common.Cache
{
    public class CacheItem
    {
        private string key;
        private object data;

        private ICacheItemExpiration[] expirations;
        private CacheItemPriority scavengingPriority;

        private DateTime lastAccessedTime;
        private bool willBeExpired;
        private bool eligibleForScavenging;

        public CacheItem(string key, object value, CacheItemPriority scavengingPriority,params ICacheItemExpiration[] expirations)
        {
            Initialize(key, value, scavengingPriority, expirations);

            TouchedByUserAction(false);
            InitializeExpirations();
        }

        public CacheItem(DateTime lastAccessedTime, string key, object value, CacheItemPriority scavengingPriority,params ICacheItemExpiration[] expirations)
        {
            Initialize(key, value, scavengingPriority, expirations);

            TouchedByUserAction(false, lastAccessedTime);
            InitializeExpirations();
        }

        /// <summary>
        /// Replaces the internals of the current cache item with the given new values. This is strictly used in the Cache
        /// class when adding a new item into the cache. By replacing the item's contents, rather than replacing the item
        /// itself, it allows us to keep a single reference in the cache, simplifying locking.
        /// </summary>
        /// <param name="cacheItemData">Value to be stored. May be null.</param>
        /// <param name="cacheItemPriority">Scavenging priority of CacheItem. See <see cref="CacheItemPriority" /> for values.</param>
        /// <param name="cacheItemRefreshAction">Object supplied by caller that will be invoked upon expiration of the CacheItem. May be null.</param>
        /// <param name="cacheItemExpirations">Param array of ICacheItemExpiration objects. May provide 0 or more of these.</param>
        internal void Replace(object cacheItemData,CacheItemPriority cacheItemPriority, params ICacheItemExpiration[] cacheItemExpirations)
        {
            Initialize(this.key, cacheItemData, cacheItemPriority, cacheItemExpirations);
            TouchedByUserAction(false);
        }

        /// <summary>
        /// Returns the <see cref="CacheItemPriority" /> assigned to this CacheItem
        /// </summary>
        public CacheItemPriority ScavengingPriority
        {
            get { return scavengingPriority; }
        }

        /// <summary>
        /// Returns the last accessed time.
        /// </summary>
        /// <value>
        /// Gets the last accessed time.
        /// </value>
        /// <remarks>
        /// The set is present for testing purposes only. Should not be called by application code 
        /// </remarks>
        public DateTime LastAccessedTime
        {
            get { return lastAccessedTime; }
        }

        /// <summary>
        /// Intended to be used internally only. The value should be true when an item is eligible to be expired.
        /// </summary>
        public bool WillBeExpired
        {
            get { return willBeExpired; }
            set { willBeExpired = value; }
        }

        /// <summary>
        /// Intended to be used internally only. The value should be true when an item is eligible for scavenging.
        /// </summary>
        public bool EligibleForScavenging
        {
            get { return eligibleForScavenging && ScavengingPriority != CacheItemPriority.NotRemovable; }
        }

        /// <summary>
        /// Returns the cached value of this CacheItem
        /// </summary>
        public object Value
        {
            get { return data; }
        }

        /// <summary>
        /// Returns the key associated with this CacheItem
        /// </summary>
        public string Key
        {
            get { return key; }
        }

        /// <summary>
        /// Intended to be used internally only. Returns object used to refresh expired CacheItems.
        /// </summary>
        //public ICacheItemRefreshAction RefreshAction
        //{
        //    get { return refreshAction; }
        //}

        /// <summary>
        /// Returns array of <see cref="ICacheItemExpiration"/> objects for this instance.
        /// </summary>
        /// <returns>
        /// An array of <see cref="ICacheItemExpiration"/> objects.
        /// </returns>
        public ICacheItemExpiration[] GetExpirations()
        {
            return (ICacheItemExpiration[])expirations.Clone();
        }

        /// <summary>
        /// Evaluates all cacheItemExpirations associated with this cache item to determine if it 
        /// should be considered expired. Evaluation stops as soon as any expiration returns true. 
        /// </summary>
        /// <returns>True if item should be considered expired, according to policies
        /// defined in this item's cacheItemExpirations.</returns>
        public bool HasExpired()
        {
            foreach (ICacheItemExpiration expiration in expirations)
            {
                if (expiration.HasExpired())
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Intended to be used internally only. This method is called whenever a CacheItem is touched through the action of a user. It
        /// prevents this CacheItem from being expired or scavenged during an in-progress expiration or scavenging process. It has no effect
        /// on subsequent expiration or scavenging processes.
        /// </summary>
        public void TouchedByUserAction(bool objectRemovedFromCache)
        {
            TouchedByUserAction(objectRemovedFromCache, DateTime.Now);
        }

        /// <summary>
        /// Intended to be used internally only. This method is called whenever a CacheItem is touched through the action of a user. It
        /// prevents this CacheItem from being expired or scavenged during an in-progress expiration or scavenging process. It has no effect
        /// on subsequent expiration or scavenging processes.
        /// </summary>
        internal void TouchedByUserAction(bool objectRemovedFromCache, DateTime timestamp)
        {
            lastAccessedTime = timestamp;
            eligibleForScavenging = false;

            foreach (ICacheItemExpiration expiration in expirations)
            {
                expiration.Notify();
            }

            willBeExpired = objectRemovedFromCache ? false : HasExpired();
        }

        /// <summary>
        /// Makes the cache item eligible for scavenging.
        /// </summary>
        public void MakeEligibleForScavenging()
        {
            eligibleForScavenging = true;
        }

        /// <summary>
        /// Makes the cache item not eligible for scavenging.
        /// </summary>
        public void MakeNotEligibleForScavenging()
        {
            eligibleForScavenging = false;
        }

        private void InitializeExpirations()
        {
            foreach (ICacheItemExpiration expiration in expirations)
            {
                expiration.Initialize(this);
            }
        }

        private void Initialize(string cacheItemKey, object cacheItemData,CacheItemPriority cacheItemPriority,params ICacheItemExpiration[] cacheItemExpirations)
        {
            key = cacheItemKey;
            data = cacheItemData;
            //refreshAction = cacheItemRefreshAction;
            scavengingPriority = cacheItemPriority;
            if (cacheItemExpirations == null)
            {
                expirations = new ICacheItemExpiration[1] { new NeverExpired() };
            }
            else
            {
                expirations = cacheItemExpirations;
            }
        }


        public void SetLastAccessedTime(DateTime specificAccessedTime)
        {
            lastAccessedTime = specificAccessedTime;
        }
    }
}
