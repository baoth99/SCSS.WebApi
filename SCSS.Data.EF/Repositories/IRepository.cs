using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.EF.Repositories
{
    public interface IRepository<T> where T : class
    {
        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<T> GetMany(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the many as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        IQueryable<T> GetManyAsNoTracking(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAll();

        /// <summary>
        /// Gets all as no tracking.
        /// </summary>
        /// <returns></returns>
        IQueryable<T> GetAllAsNoTracking();

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        T Get(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        T GetAsNoTracking(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<T> GetAsync(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the asynchronous as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        Task<T> GetAsyncAsNoTracking(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        T GetById(object id);

        /// <summary>
        /// Determines whether the specified predicate is existed.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if the specified predicate is existed; otherwise, <c>false</c>.
        /// </returns>
        bool IsExisted(Expression<Func<T, bool>> predicate);

        /// <summary>
        /// Determines whether [is existed asynchronous] [the specified predicate].
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        ///   <c>true</c> if the specified predicate is existed; otherwise, <c>false</c>.
        /// </returns>
        Task<bool> IsExistedAsync(Expression<Func<T, bool>> predicate);


        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        T Insert(T entity);

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void InsertRange(List<T> entities);

        /// <summary>
        /// Updates the specified enity.
        /// </summary>
        /// <param name="enity">The enity.</param>
        void Update(T entity);

        /// <summary>
        /// Updates the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void UpdateRange(List<T> entities);

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        void Remove(T entity);

        /// <summary>
        /// Removes the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        void RemoveById(object id);

        /// <summary>
        /// Removes the multiple.
        /// </summary>
        /// <param name="entities">The entities.</param>
        void RemoveMultiple(List<T> entities);

        /// <summary>
        /// Removes the multiple.
        /// </summary>
        /// <param name="deleteIds">The delete ids.</param>
        void RemoveMultiple(List<Guid> deleteIds);
    }
}
