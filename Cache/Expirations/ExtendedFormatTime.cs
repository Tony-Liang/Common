﻿//===============================================================================
// Microsoft patterns & practices Enterprise Library
// Caching Application Block
//===============================================================================
// Copyright © Microsoft Corporation.  All rights reserved.
// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY
// OF ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT
// LIMITED TO THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
// FITNESS FOR A PARTICULAR PURPOSE.
//===============================================================================

using System;

namespace LCW.Framework.Common.Cache.Expirations
{
    /// <summary>
    ///	This provider tests if a item was expired using a extended format.
    /// </summary>
#if !SILVERLIGHT
    [Serializable]
    [System.Runtime.InteropServices.ComVisible(false)]
#endif
    public class ExtendedFormatTime : ICacheItemExpiration
    {
        private string extendedFormat;
        private DateTime lastUsedTime;

        /// <summary>
        ///	Convert the input format to the extented time format.
        /// </summary>
        /// <param name="timeFormat">
        ///	This contains the expiration information
        /// </param>
        public ExtendedFormatTime(string timeFormat)
        {
            // check arguments
            if (string.IsNullOrEmpty(timeFormat))
            {
                //throw new ArgumentException(Resources.ExceptionNullTimeFormat, "timeFormat");
            }

            ExtendedFormat.Validate(timeFormat);

            // Get the modified extended format
            this.extendedFormat = timeFormat;

            // Convert to UTC in order to compensate for time zones		
            this.lastUsedTime = DateTime.Now.ToUniversalTime();
        }

#if SILVERLIGHT
        public ExtendedFormatTime()
        { }

        public DateTime LastUsedTime
        {
            get { return lastUsedTime; }
            set { lastUsedTime = value; }
        }
#endif

        /// <summary>
        /// Gets the extended time format.
        /// </summary>
        /// <value>
        /// The extended time format.
        /// </value>
        public string TimeFormat
        {
            get { return extendedFormat; }
#if SILVERLIGHT
            set { extendedFormat = value; }
#endif
        }

        /// <summary>
        ///	Specifies if item has expired or not.
        /// </summary>
        /// <returns>
        ///	Returns true if the data is expired otherwise false
        /// </returns>
        public bool HasExpired()
        {
            // Convert to UTC in order to compensate for time zones		
            DateTime nowDateTime = DateTime.Now.ToUniversalTime();

            ExtendedFormat format = new ExtendedFormat(extendedFormat);
            // Check expiration
            if (format.IsExpired(lastUsedTime, nowDateTime))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///	Notifies that the item was recently used.
        /// </summary>
        public void Notify()
        {
        }

        /// <summary>
        /// Not used
        /// </summary>
        /// <param name="owningCacheItem">Not used</param>
        public void Initialize(CacheItem owningCacheItem)
        {
        }
    }
}
