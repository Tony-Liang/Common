using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace LCW.Framework.Common.Cache
{
    public interface ICacheItemExpiration
    {
        bool HasExpired();

        void Notify();

        void Initialize(CacheItem owningCacheItem);
    }
}
