using Newtonsoft.Json;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.AWSService.RedisCacheHandler
{
    public class CacheListHandler<T> : IDisposable, ICacheListHandler<T> where T : class
    {
        #region Services

        /// <summary>
        /// The redis database
        /// </summary>
        private readonly IDatabase redisDB;

        #endregion

        #region Redis Key

        /// <summary>
        /// The cache redis key
        /// </summary>
        private readonly CacheRedisKey cacheRedisKey;

        #endregion

        #region Constructor 

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheListHandler{T}"/> class.
        /// </summary>
        /// <param name="RedisDB">The redis database.</param>
        /// <param name="CacheRedisKey">The cache redis key.</param>
        public CacheListHandler(IConnectionMultiplexer Connection, int db, CacheRedisKey CacheRedisKey)
        {
            redisDB = Connection.GetDatabase(db);
            cacheRedisKey = CacheRedisKey;
        }

        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region Get All Async

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public async Task<List<T>> GetAllAsync()
        {
            var cache = await redisDB.ListRangeAsync(cacheRedisKey.ToString());
            var res = cache.Select(x => JsonConvert.DeserializeObject<T>(x)).ToList();
            return res;
        }

        #endregion

        #region Get All

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public List<T> GetAll()
        {
            var cache = redisDB.ListRange(cacheRedisKey.ToString());
            var res = cache.Select(x => JsonConvert.DeserializeObject<T>(x)).ToList();
            return res;
        }

        #endregion

        #region Get Async

        /// <summary>
        /// Gets the many asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public async Task<List<T>> GetManyAsync(Func<T, bool> predicate)
        {
            var data = await GetAllAsync();
            var res = data.Where(predicate).ToList();
            return res;
        }

        #endregion

        #region Get Many

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public List<T> GetMany(Func<T, bool> predicate)
        {
            var data = GetAll();
            var res = data.Where(predicate).ToList();
            return res;
        }

        #endregion

        #region Push Async

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task PushAsync(T model)
        {
            await redisDB.ListLeftPushAsync(cacheRedisKey.ToString(), model.ToJson());
        }

        #endregion

        #region Push Many Async

        /// <summary>
        /// Pushes the many asynchronous.
        /// </summary>
        /// <param name="models">The models.</param>
        public async Task PushManyAsync(List<T> models)
        {
            foreach (var item in models)
            {
                await PushAsync(item);
            }
        }

        #endregion

        #region Push

        /// <summary>
        /// Pushes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Push(T model)
        {
            redisDB.ListLeftPush(cacheRedisKey.ToString(), model.ToJson());
        }

        #endregion

        #region Push Many

        /// <summary>
        /// Pushes the many.
        /// </summary>
        /// <param name="models">The models.</param>
        public void PushMany(List<T> models)
        {
            foreach (var item in models)
            {
                Push(item);
            }
        }

        #endregion

        #region RemoveAsync

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task RemoveAsync(T model)
        {
            await redisDB.ListRemoveAsync(cacheRedisKey.ToString(), model.ToJson());
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Remove(T model)
        {
            redisDB.ListRemove(cacheRedisKey.ToString(), model.ToJson());
        }

        #endregion

        #region Remove Redis Key Async

        /// <summary>
        /// Removes the redis key asynchronous.
        /// </summary>
        public async Task RemoveRedisKeyAsync()
        {
             await redisDB.KeyDeleteAsync(cacheRedisKey.ToString());
        }

        #endregion

        #region Remove Redis Key

        /// <summary>
        /// Removes the redis key.
        /// </summary>
        public void RemoveRedisKey()
        {
            redisDB.KeyDelete(cacheRedisKey.ToString());
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
