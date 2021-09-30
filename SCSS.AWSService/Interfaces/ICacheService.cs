using SCSS.Utilities.Constants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.Interfaces
{
    public interface IStringCacheService
    {
        /// <summary>
        /// Sets the string cache asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SetStringCacheAsync(CacheRedisKey key, string data);

        /// <summary>
        /// Sets the string cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        void SetStringCache(CacheRedisKey key, string data);

        /// <summary>
        /// Sets the string caches asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        Task SetStringCachesAsync(Dictionary<CacheRedisKey, string> data);

        /// <summary>
        /// Gets the string cache asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task<string> GetStringCacheAsync(CacheRedisKey key);

        /// <summary>
        /// Gets the string cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        string GetStringCache(CacheRedisKey key);

        /// <summary>
        /// Removes the string cache asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        Task RemoveStringCacheAsync(CacheRedisKey key);

        /// <summary>
        /// Removes the string cache.
        /// </summary>
        /// <param name="key">The key.</param>
        void RemoveStringCache(CacheRedisKey key);

        /// <summary>
        /// Removes the string caches asynchronous.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        Task RemoveStringCachesAsync(List<CacheRedisKey> keys);

        /// <summary>
        /// Removes the string caches.
        /// </summary>
        /// <param name="keys">The keys.</param>
        /// <returns></returns>
        void RemoveStringCaches(List<CacheRedisKey> keys);
    }
}
