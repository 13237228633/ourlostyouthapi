namespace Common
{
    using System;
    using System.Web;

    /// <summary>
    /// 数据缓存操作类

    /// </summary>
    public class CacheHelper
    {
        /// <summary>
        /// 全局缓存时间
        /// </summary>
        public static int CacheTime = 5; //ConfigHelper.GetConfigInt("CacheTime");

        /// <summary>
        /// 构造

        /// </summary>
        public CacheHelper(){}

        /// <summary>
        /// 获取缓存
        /// </summary>
        /// <param name="CacheKey"></param>
        /// <returns></returns>
        public static object GetCache(string CacheKey)
        {
            return HttpRuntime.Cache[CacheKey];
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="CacheKey">key</param>
        /// <param name="objObject">值</param>
        public static void SetCache(string CacheKey, object objObject)
        {
            HttpRuntime.Cache.Insert(CacheKey, objObject);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="CacheKey">key</param>
        /// <param name="objObject">值</param>
        /// <param name="Timeout">过期时间（分钟）</param>
        public static void SetCache(string CacheKey, object objObject,int Timeout)
        {
            HttpRuntime.Cache.Insert(CacheKey, objObject, null, DateTime.Now.AddMinutes(Timeout),System.Web.Caching.Cache.NoSlidingExpiration);
        }

        /// <summary>
        /// 设置缓存
        /// </summary>
        /// <param name="CacheKey">key</param>
        /// <param name="objObject">值</param>
        /// <param name="absoluteExpiration">过期时间</param>
        /// <param name="slidingExpiration">过期时间间隔</param>
        public static void SetCache(string CacheKey, object objObject, DateTime absoluteExpiration, TimeSpan slidingExpiration)
        {
            HttpRuntime.Cache.Insert(CacheKey, objObject, null, absoluteExpiration, slidingExpiration);
        }

        /// <summary>
        /// 删除缓存
        /// </summary>
        /// <param name="key">key</param>
        public static void Delete(string key)
        {
            HttpRuntime.Cache.Remove(key);
        }
    }
}

