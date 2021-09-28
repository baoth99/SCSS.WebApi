using Microsoft.Extensions.Caching.Distributed;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.Implementations
{
    public class CacheService : AWSBaseService, ICacheService
    {
        #region Services

        /// <summary>
        /// The distributed cache
        /// </summary>
        private readonly IDistributedCache _distributedCache;


        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="distributedCache">The distributed cache.</param>
        public CacheService(ILoggerService logger, IDistributedCache distributedCache) : base(logger)
        {
            _distributedCache = distributedCache;
        }

        #endregion

        #region Set Cache Data

        /// <summary>
        /// Sets the images slider cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public async Task SetCacheData(CacheRedisKey key, string data)
        {
            try
            {
                // Remove old Cache
                await _distributedCache.RemoveAsync(key.ToString());

                // Set new Cache
                await _distributedCache.SetStringAsync(key.ToString(), data);

                Logger.LogInfo(CacheLoggerMessage.SetCacheSuccess(key));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.SetCacheFail(key));
            }
        }

        #endregion

        #region Set Cache Data (Dic)

        public async Task SetCacheDatas(Dictionary<CacheRedisKey, string> data)
        {
            foreach (var item in data)
            {
                await SetCacheData(item.Key, item.Value);
            }
        }

        #endregion

        #region Set Cache Data Byte

        /// <summary>
        /// Sets the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="data">The data.</param>
        public async Task SetCacheData(CacheRedisKey key, byte[] data)
        {
            try
            {
                // Remove old Cache
                await _distributedCache.RemoveAsync(key.ToString());

                // Set new Cache
                await _distributedCache.SetAsync(key.ToString(), data);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.SetCacheFail(key));
            }
        }

        #endregion

        #region Set Cache Data Byte (Dic)

        /// <summary>
        /// Sets the cache data.
        /// </summary>
        /// <param name="data">The data.</param>
        public async Task SetCacheDatas(Dictionary<CacheRedisKey, byte[]> data)
        {
            foreach (var item in data)
            {
                await SetCacheData(item.Key, item.Value);
            }
        }


        #endregion

        #region Get Cache Data

        /// <summary>
        /// Gets the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<string> GetCacheData(CacheRedisKey key)
        {
            try
            {
                var data = await _distributedCache.GetStringAsync(key.ToString());

                if (data == null)
                {
                    return null;
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

        #region Get Cache Data Byte

        /// <summary>
        /// Gets the cache byte data.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public async Task<byte[]> GetCacheByteData(CacheRedisKey key)
        {
            try
            {
                var data = await _distributedCache.GetAsync(key.ToString());

                if (data == null)
                {
                    return null;
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
        /// Removes the cache data.
        /// </summary>
        /// <param name="key">The key.</param>
        public async Task RemoveCacheData(CacheRedisKey key)
        {
            try
            {
                await _distributedCache.RemoveAsync(key.ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, CacheLoggerMessage.RemoveCacheFail(key));
            }           
        }

        #endregion

        #region Remove Cache Data (List)

        /// <summary>
        /// Removes the cache datas.
        /// </summary>
        /// <param name="keys">The keys.</param>
        public async Task RemoveCacheDatas(List<CacheRedisKey> keys)
        {
            try
            {
                foreach (var item in keys)
                {
                    await RemoveCacheData(item);
                }
            }
            catch (Exception ex)
            {
                //Logger.LogError(ex, CacheLoggerMessage.RemoveCacheFail());
            }
        }

        #endregion

    }
}
