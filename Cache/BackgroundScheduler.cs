using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace LCW.Framework.Common.Cache
{
    public class BackgroundScheduler : ICacheScavenger
    {
        private ExpirationTask expirationTask;
        //private ScavengerTask scavengerTask;
        //private ICachingInstrumentationProvider instrumentationProvider;

        private object scavengeExpireLock = new object();
        private int scavengePending = 0;

        /// <summary>
        /// Initialize a new instance of the <see cref="BackgroundScheduler"/> with a <see cref="ExpirationTask"/> and 
        /// a <see cref="ScavengerTask"/>.
        /// </summary>
        /// <param name="expirationTask">The expiration task to use.</param>
        /// <param name="scavengerTask">The scavenger task to use.</param>
        /// <param name="instrumentationProvider">The instrumentation provider to use.</param>
        public BackgroundScheduler(ExpirationTask expirationTask)
        {
            this.expirationTask = expirationTask;
            //this.scavengerTask = scavengerTask;
            //this.instrumentationProvider = instrumentationProvider;
        }

        /// <summary>
        /// Queues a message that the expiration timeout has expired.
        /// </summary>
        /// <param name="notUsed">Ignored.</param>
        public void ExpirationTimeoutExpired(object notUsed)
        {
            ThreadPool.QueueUserWorkItem(unused => BackgroundWork(Expire));
        }

        /// <summary>
        /// Starts the scavenging process.
        /// </summary>
        public void StartScavenging()
        {
            int pendingScavengings = Interlocked.Increment(ref scavengePending);
            if (pendingScavengings == 1)
            {
                ThreadPool.QueueUserWorkItem(unused => BackgroundWork(Scavenge));
            }
        }

        internal void StartScavengingIfNeeded()
        {
            //if (scavengerTask.IsScavengingNeeded())
            //{
            //    StartScavenging();
            //}
        }

        internal void BackgroundWork(Action work)
        {
            try
            {
                lock (scavengeExpireLock)
                {
                    work();
                }
            }
            catch (Exception e)
            {
                //instrumentationProvider.FireCacheFailed(Resources.BackgroundSchedulerProducerConsumerQueueFailure, e);
            }
        }

        internal void Scavenge()
        {
            //int pendingScavengings = Interlocked.Exchange(ref scavengePending, 0);
            //int timesToScavenge = ((pendingScavengings - 1) / scavengerTask.NumberOfItemsToBeScavenged) + 1;
            //while (timesToScavenge > 0)
            //{
            //    scavengerTask.DoScavenging();
            //    --timesToScavenge;
            //}
        }

        internal void Expire()
        {
            expirationTask.DoExpirations();
        }

    }
}
