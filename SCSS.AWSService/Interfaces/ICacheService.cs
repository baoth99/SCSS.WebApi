using SCSS.Utilities.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.Interfaces
{
    public interface ICacheService
    {
        /// <summary>
        /// Sets the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SetCacheData(CacheRedisKey key, string data);

        /// <summary>
        /// Sets the cache data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SetCacheDatas(Dictionary<CacheRedisKey, string> data);

        /// <summary>
        /// Sets the cache data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SetCacheDatas(Dictionary<CacheRedisKey, byte[]> data);

        /// <summary>
        /// Sets the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SetCacheData(CacheRedisKey key, byte[] data);

        /// <summary>
        /// Gets the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<string> GetCacheData(CacheRedisKey key);

        /// <summary>
        /// Gets the cache byte data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<byte[]> GetCacheByteData(CacheRedisKey key);

        /// <summary>
        /// Removes the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveCacheData(CacheRedisKey key);

        /// <summary>
        /// Removes the cache datas.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        Task RemoveCacheDatas(List<CacheRedisKey> keys);
    }
}
