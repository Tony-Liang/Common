using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LCW.Framework.Common.Cache
{
    public sealed class ExpirationPollTimer : IDisposable
    {
        private Timer pollTimer;
        private int expirationPollFrequencyInMilliSeconds;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="expirationPollFrequencyInMilliSeconds"></param>
        public ExpirationPollTimer(int expirationPollFrequencyInMilliSeconds)
        {
            if (expirationPollFrequencyInMilliSeconds <= 0)
            {
                //throw new ArgumentException(Resources.InvalidExpirationPollFrequencyInMilliSeconds, "expirationPollFrequencyInMilliSeconds");
            }

            this.expirationPollFrequencyInMilliSeconds = expirationPollFrequencyInMilliSeconds;
        }

        /// <summary>
        /// Start the polling process.
        /// </summary>
        /// <param name="callbackMethod">The method to callback when a cycle has completed.</param>
        public void StartPolling(TimerCallback callbackMethod)
        {
            if (callbackMethod == null)
            {
                throw new ArgumentNullException("callbackMethod");
            }

            pollTimer = new Timer(callbackMethod, null, expirationPollFrequencyInMilliSeconds, expirationPollFrequencyInMilliSeconds);
        }

        /// <summary>
        /// Stop the polling process.
        /// </summary>
        public void StopPolling()
        {
            if (pollTimer == null)
            {
               // throw new InvalidOperationException(Resources.InvalidPollingStopOperation);
            }

            pollTimer.Dispose();
            pollTimer = null;
        }

        void IDisposable.Dispose()
        {
            if (pollTimer != null) pollTimer.Dispose();
        }
    }
}
