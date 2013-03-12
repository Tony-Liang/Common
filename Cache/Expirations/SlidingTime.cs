﻿using System;

namespace LCW.Framework.Common.Cache.Expirations
{
    /// <summary>
    ///	This provider tests if a item was expired using a time slice schema.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
#endif
    public class SlidingTime : ICacheItemExpiration
    {
        private DateTime timeLastUsed;
        private TimeSpan itemSlidingExpiration;

        /// <summary>
        ///	Create an instance of this class with the timespan for expiration.
        /// </summary>
        /// <param name="slidingExpiration">
        ///	Expiration time span
        /// </param>
        public SlidingTime(TimeSpan slidingExpiration)
        {
            // Check that expiration is a valid numeric value
            if (!(slidingExpiration.TotalSeconds >= 1))
            {
                //throw new ArgumentOutOfRangeException("slidingExpiration",
                //                                      Resources.ExceptionRangeSlidingExpiration);
            }

            this.itemSlidingExpiration = slidingExpiration;
        }


        /// <summary>
        /// For internal use only.
        /// </summary>
        /// <param name="slidingExpiration"/>
        /// <param name="originalTimeStamp"/>
        /// <remarks>
        /// This constructor is for testing purposes only. Never, ever call it in a real program
        /// </remarks>
        public SlidingTime(TimeSpan slidingExpiration, DateTime originalTimeStamp)
            : this(slidingExpiration)
        {
            timeLastUsed = originalTimeStamp;
        }

#if SILVERLIGHT
        public SlidingTime()
        { }
#endif

        /// <summary>
        /// Returns sliding time window that must be exceeded for expiration to occur
        /// </summary>
        public TimeSpan ItemSlidingExpiration
        {
            get { return itemSlidingExpiration; }
#if SILVERLIGHT
            set { itemSlidingExpiration = value; }
#endif
        }

        /// <summary>
        /// Returns time that this object was last touched
        /// </summary>
        public DateTime TimeLastUsed
        {
            get { return timeLastUsed; }
#if SILVERLIGHT
            set { timeLastUsed = value; }
#endif
        }

        /// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <returns>Returns true if the item has expired otherwise false.</returns>
        public bool HasExpired()
        {
            bool expired = CheckSlidingExpiration(DateTime.Now,
                                                  this.timeLastUsed,
                                                  this.itemSlidingExpiration);
            return expired;
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
            this.timeLastUsed = DateTime.Now;
        }

        /// <summary>
        /// Used to set the initial value of TimeLastUsed. This method is invoked during the reinstantiation of
        /// an instance from a persistent store. 
        /// </summary>
        /// <param name="owningCacheItem">CacheItem to which this expiration belongs.</param>
        public void Initialize(CacheItem owningCacheItem)
        {
            if (owningCacheItem == null) throw new ArgumentNullException("owningCacheItem");

            timeLastUsed = owningCacheItem.LastAccessedTime;
        }

        /// <summary>
        ///	Check whether the sliding time has expired.
        /// </summary>
        /// <param name="nowDateTime">Current time </param>
        /// <param name="lastUsed">The last time when the item has been used</param>
        /// <param name="slidingExpiration">The span of sliding expiration</param>
        /// <returns>True if the item was expired, otherwise false</returns>
        private static bool CheckSlidingExpiration(DateTime nowDateTime,
                                                   DateTime lastUsed,
                                                   TimeSpan slidingExpiration)
        {
            // Convert to UTC in order to compensate for time zones
            DateTime tmpNowDateTime = nowDateTime.ToUniversalTime();

            // Convert to UTC in order to compensate for time zones
            DateTime tmpLastUsed = lastUsed.ToUniversalTime();

            long expirationTicks = tmpLastUsed.Ticks + slidingExpiration.Ticks;

            bool expired = (tmpNowDateTime.Ticks >= expirationTicks) ? true : false;

            return expired;
        }
    }
}
