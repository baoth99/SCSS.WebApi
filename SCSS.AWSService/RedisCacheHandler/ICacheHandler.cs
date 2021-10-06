using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.AWSService.RedisCacheHandler
{
    public interface ICacheListHandler<T> where T : class
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns></returns>
        Task<List<T>> GetAllAsync();

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        List<T> GetAll();

        /// <summary>
        /// Gets the many asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<List<T>> GetManyAsync(Func<T, bool> predicate);

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        List<T> GetMany(Func<T, bool> predicate);

        /// <summary>
        /// Pushes the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task PushAsync(T model);


        /// <summary>
        /// Pushes the many asynchronous.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns></returns>
        Task PushManyAsync(List<T> models);

        /// <summary>
        /// Pushes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Push(T model);

        /// <summary>
        /// Pushes the many.
        /// </summary>
        /// <param name="models">The models.</param>
        void PushMany(List<T> models);

        /// <summary>
        /// Removes the asynchronous.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task RemoveAsync(T model);

        /// <summary>
        /// Removes the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        void Remove(T model);

        /// <summary>
        /// Removes the redis key asynchronous.
        /// </summary>
        /// <returns></returns>
        Task RemoveRedisKeyAsync();

        /// <summary>
        /// Removes the redis key.
        /// </summary>
        void RemoveRedisKey();
    }
}
