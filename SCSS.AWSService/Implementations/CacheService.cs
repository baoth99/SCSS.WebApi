using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.Implementations
{
    public class StringCacheService : AWSBaseService, IStringCacheService
    {
        #region Services

        /// <summary>
        /// The redis database
        /// </summary>
        private readonly IDatabase _redisDB;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StringCacheService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public StringCacheService(ILoggerService logger, IConnectionMultiplexer connection) : base(logger)
        {
            _redisDB = connection.GetDatabase(AppSettingValues.RedisDB00);
        }

        #endregion

        #region Set String Cache Async

        /// <summary>
        /// Sets the string cache asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public async Task SetStringCacheAsync(CacheRedisKey key, string data)
        {
            try
            {
                // Set new Cache
                await _redisDB.StringSetAsync(key.ToString(), data);

                Logger.LogInfo(CacheLoggerMessage.SetCacheSuccess(key));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.SetCacheFail(key));
            }
        }

        #endregion

        #region Set String Cache

        /// <summary>
        /// Sets the string cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public void SetStringCache(CacheRedisKey key, string data)
        {
            try
            {
                // Set new Cache
                _redisDB.StringSet(key.ToString(), data);
                Logger.LogInfo(CacheLoggerMessage.SetCacheSuccess(key));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.SetCacheFail(key));
            }
        }

        #endregion

        #region Set String Caches Async (Dic)

        /// <summary>
        /// Sets the string caches asynchronous.
        /// </summary>
        /// <param name="data">The data.</param>
        public async Task SetStringCachesAsync(Dictionary<CacheRedisKey, string> data)
        {
            foreach (var item in data)
            {
                await SetStringCacheAsync(item.Key, item.Value);
            }
        }

        #endregion

        #region Get String Cache Async

        /// <summary>
        /// Gets the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<string> GetStringCacheAsync(CacheRedisKey key)
        {
            try
            {
                var data = await _redisDB.StringGetAsync(key.ToString());

                if (!data.HasValue)
                {
                    return string.Empty;
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.GetCacheFail(key));
                return null;
            }
        }

        #endregion

        #region Get String Cache

        /// <summary>
        /// Gets the string cache.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public string GetStringCache(CacheRedisKey key)
        {
            try
            {
                var data = _redisDB.StringGet(key.ToString());

                if (!data.HasValue)
                {
                    return string.Empty;
                }

                return data;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.GetCacheFail(key));
                return null;
            }
        }

        #endregion

        #region Remove Cache Data

        /// <summary>
        /// Removes the string cache asynchronous.
        /// </summary>
        /// <param name="key">The key.</param>
        public async Task RemoveStringCacheAsync(CacheRedisKey key)
        {
            try
            {
                await _redisDB.KeyDeleteAsync(key.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.RemoveCacheFail(key));
            }           
        }

        #endregion

        #region Remove String Cache

        /// <summary>
        /// Removes the string cache.
        /// </summary>
        /// <param name="key">The key.</param>
        public void RemoveStringCache(CacheRedisKey key)
        {
            try
            {
                _redisDB.KeyDelete(key.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.RemoveCacheFail(key));
            }
        }

        #endregion

        #region Remove Cache Data (List)

        /// <summary>
        /// Removes the string caches asynchronous.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public async Task RemoveStringCachesAsync(List<CacheRedisKey> keys)
        {
            try
            {
                foreach (var item in keys)
                {
                    await RemoveStringCacheAsync(item);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "");
            }
        }

        #endregion

        #region Remove String Caches

        /// <summary>
        /// Removes the string caches.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public void RemoveStringCaches(List<CacheRedisKey> keys)
        {
            try
            {
                foreach (var item in keys)
                {
                    RemoveStringCache(item);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "");
            }
        }

        #endregion
    }
}
