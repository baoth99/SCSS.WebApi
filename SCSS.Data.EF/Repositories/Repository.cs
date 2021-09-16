using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace SCSS.Data.EF.Repositories
{
    public class Repository<T> : IDisposable, IRepository<T> where T : class
    {
        #region DbSet

        /// <summary>
        /// The database set
        /// </summary>
        protected readonly DbSet<T> DbSet;

        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Repository{T}"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public Repository(AppDbContext context)
        {
            DbSet = context.Set<T>();
        }

        #endregion

        #region GetMany

        /// <summary>
        /// Gets the many.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetMany(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate);
        }

        #endregion

        #region GetManyAsNoTracking

        /// <summary>
        /// Gets the many as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual IQueryable<T> GetManyAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Where(predicate).AsNoTracking();
        }

        #endregion

        #region GetAll

        /// <summary>
        /// Gets all.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAll()
        {
            return DbSet.AsQueryable();
        }

        #endregion

        #region GetAllAsNoTracking

        /// <summary>
        /// Gets all as no tracking.
        /// </summary>
        /// <returns></returns>
        public virtual IQueryable<T> GetAllAsNoTracking()
        {
            return DbSet.AsNoTracking();
        }

        #endregion

        #region Get

        /// <summary>
        /// Gets the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> predicate)
        {
            return DbSet.FirstOrDefault(predicate);
        }

        #endregion

        #region GetAsNoTracking

        /// <summary>
        /// Gets as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual T GetAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return DbSet.AsNoTracking().FirstOrDefault(predicate);
        }

        #endregion

        #region GetAsync

        /// <summary>
        /// Gets the asynchronous.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region GetAsyncAsNoTracking

        /// <summary>
        /// Gets the asynchronous as no tracking.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns></returns>
        public virtual async Task<T> GetAsyncAsNoTracking(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AsNoTracking().FirstOrDefaultAsync(predicate);
        }

        #endregion

        #region GetById

        /// <summary>
        /// Gets the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual T GetById(object id)
        {
            return DbSet.Find(id);
        }

        #endregion

        #region GetByIdAsync

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public virtual async Task<T> GetByIdAsync(object id)
        {
            return await DbSet.FindAsync(id);
        }

        #endregion

        #region IsExisted

        /// <summary>
        /// Determines whether the specified predicate is existed.
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// <c>true</c> if the specified predicate is existed; otherwise, <c>false</c>.
        /// </returns>
        public virtual bool IsExisted(Expression<Func<T, bool>> predicate)
        {
            return DbSet.Any(predicate);
        }

        #endregion

        #region IsExistedAsync

        /// <summary>
        /// Determines whether [is existed asynchronous] [the specified predicate].
        /// </summary>
        /// <param name="predicate">The predicate.</param>
        /// <returns>
        /// <c>true</c> if the specified predicate is existed; otherwise, <c>false</c>.
        /// </returns>
        public async Task<bool> IsExistedAsync(Expression<Func<T, bool>> predicate)
        {
            return await DbSet.AnyAsync(predicate);
        }

        #endregion

        #region Insert

        /// <summary>
        /// Inserts the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual T Insert(T entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        #endregion

        #region InsertAsync

        /// <summary>
        /// Inserts the asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public virtual async Task<T> InsertAsync(T entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        #endregion

        #region InsertRange

        /// <summary>
        /// Inserts the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void InsertRange(List<T> entities)
        {
            DbSet.AddRange(entities);
        }

        #endregion

        #region InsertRangeAsync

        /// <summary>
        /// Inserts the range asynchronous.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual async Task InsertRangeAsync(List<T> entities)
        {
            await DbSet.AddRangeAsync(entities);
        }

        #endregion

        #region Update

        /// <summary>
        /// Updates the specified enity.
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity)
        {
            DbSet.Update(entity);
        }

        #endregion

        #region UpdateRange

        /// <summary>
        /// Updates the range.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void UpdateRange(List<T> entities)
        {
            if (entities.Count > 0)
            {
                DbSet.UpdateRange(entities);
            }
        }

        #endregion

        #region Remove

        /// <summary>
        /// Removes the specified entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public virtual void Remove(T entity)
        {
            if (entity != null)
            {
                DbSet.Remove(entity);
            }
        }

        #endregion

        #region RemoveById

        /// <summary>
        /// Removes the by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public virtual void RemoveById(object id)
        {
            var entityRemove = GetById(id);
            if (entityRemove != null)
            {
                Remove(entityRemove);
            }
        }

        #endregion

        #region RemoveMultiple

        /// <summary>
        /// Removes the multiple.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public virtual void RemoveMultiple(List<T> entities)
        {
            if (entities != null && entities.Count > 0)
            {
                DbSet.RemoveRange(entities);
            }
        }

        #endregion

        #region RemoveMultiple by Id

        /// <summary>
        /// Removes the multiple.
        /// </summary>
        /// <param name="deleteIds">The delete ids.</param>
        public virtual void RemoveMultiple(List<Guid> deleteIds)
        {
            if (deleteIds.Count > 0)
            {
                deleteIds.ForEach(id =>
                {
                    var entityRemove = GetById(id);
                    if (entityRemove != null)
                    {
                        DbSet.Remove(entityRemove);
                    }
                });
            }
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
