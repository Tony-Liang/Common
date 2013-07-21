using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Caching;
using System.Web;

namespace LCW.Framework.Common.Web
{
    /// <summary>
    /// Cache操作类
    /// </summary>
    public static class Cache
    {
        /// <summary>
        /// 简单创建/修改Cache，前提是这个值是字符串形式的
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <param name="strValue">Cache值</param>
        /// <param name="iExpires">有效期，秒数（使用的是当前时间+秒数得到一个绝对到期值）</param>
        /// <param name="priority">保留优先级，1最不会被清除，6最容易被内存管理清除（1:NotRemovable；2:High；3:AboveNormal；4:Normal；5:BelowNormal；6:Low）</param>
        public static void Insert(string strCacheName, string strValue, int iExpires, int priority)
        {
            TimeSpan ts = new TimeSpan(0, 0, iExpires);
            CacheItemPriority cachePriority;
            switch (priority)
            {
                case 6:
                    cachePriority = CacheItemPriority.Low;
                    break;
                case 5:
                    cachePriority = CacheItemPriority.BelowNormal;
                    break;
                case 4:
                    cachePriority = CacheItemPriority.Normal;
                    break;
                case 3:
                    cachePriority = CacheItemPriority.AboveNormal;
                    break;
                case 2:
                    cachePriority = CacheItemPriority.High;
                    break;
                case 1:
                    cachePriority = CacheItemPriority.NotRemovable;
                    break;
                default:
                    cachePriority = CacheItemPriority.Default;
                    break;
            }
            HttpContext.Current.Cache.Insert(strCacheName, strValue, null, DateTime.Now.Add(ts), System.Web.Caching.Cache.NoSlidingExpiration, cachePriority, null);
        }
        /// <summary>
        /// 简单读Cache对象的值，前提是这个值是字符串形式的
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        /// <returns>Cache字符串值</returns>
        public static string Get(string strCacheName)
        {
            return HttpContext.Current.Cache[strCacheName].ToString();
        }
        /// <summary>
        /// 删除Cache对象
        /// </summary>
        /// <param name="strCacheName">Cache名称</param>
        public static void Del(string strCacheName)
        {
            HttpContext.Current.Cache.Remove(strCacheName);
        }

        /// <summary>
        /// 创建缓存项的文件依赖
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="fileName">文件绝对路径</param>
        public static void Insert(string key, object obj, string fileName)
        {
            //创建缓存依赖项
            CacheDependency dep = new CacheDependency(fileName);
            //创建缓存
            HttpContext.Current.Cache.Insert(key, obj, dep);
        }

        /// <summary>
        /// 创建缓存项过期
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <param name="obj">object对象</param>
        /// <param name="expires">过期时间(分钟)</param>
        public static void Insert(string key, object obj, int expires)
        {
            HttpContext.Current.Cache.Insert(key, obj, null, System.Web.Caching.Cache.NoAbsoluteExpiration, new TimeSpan(0, expires, 0));
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <param name="key">缓存Key</param>
        /// <returns>object对象</returns>
        public static object GetItem(string key)
        {
            if (string.IsNullOrEmpty(key))
            {
                return null;
            }
            return HttpContext.Current.Cache.Get(key);
        }

        /// <summary>
        /// 获取缓存对象
        /// </summary>
        /// <typeparam name="T">T对象</typeparam>
        /// <param name="key">缓存Key</param>
        /// <returns></returns>
        public static T Get<T>(string key)
        {
            object obj = Get(key);
            return obj == null ? default(T) : (T)obj;
        }

    }
}
