using Microsoft.Extensions.Caching.Distributed;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Constants;
using System;
using System.Threading.Tasks;

namespace SCSS.AWSService.Implementations
{
    public class CacheService : IDisposable, ICacheService
    {
        #region Services

        /// <summary>
        /// The distributed cache
        /// </summary>
        private readonly IDistributedCache _distributedCache;


        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheService"/> class.
        /// </summary>
        /// <param name="distributedCache">The distributed cache.</param>
        public CacheService(IDistributedCache distributedCache)
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
            }
            catch (Exception)
            {
                throw;
            }
        }

        #endregion

        #region Set Cache Data Byte

        public async Task SetCacheData(CacheRedisKey key, byte[] data)
        {
            try
            {
                // Remove old Cache
                await _distributedCache.RemoveAsync(key.ToString());

                // Set new Cache
                await _distributedCache.SetAsync(key.ToString(), data);
            }
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {
                throw;
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
            catch (Exception)
            {

                throw;
            }
        }

        #endregion

        #region Remove Cache Data

        public async Task RemoveCacheData(CacheRedisKey key)
        {
            await _distributedCache.RemoveAsync(key.ToString());
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }
            }
            this.Disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

    }
}
