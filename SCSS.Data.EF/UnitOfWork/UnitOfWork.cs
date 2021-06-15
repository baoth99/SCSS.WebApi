using Microsoft.EntityFrameworkCore;
using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.EF.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region AppDbContext

        /// <summary>
        /// The application database context
        /// </summary>
        private readonly AppDbContext AppDbContext;

        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion


        #region Private variable Repositories

        // Create Private variable Repositories here !
        // Example
        // private IRepository<Product> _productRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        public UnitOfWork(AppDbContext appDbContext)
        {
            //Initialize AppDbContext
            AppDbContext = appDbContext;
            //Set Command Timeout
            AppDbContext.Database.SetCommandTimeout(AppSettingValues.CommandTimeout);
        }

        #endregion

        #region Publish Access Repositories

        // Create Publish Access Repositories here
        // Example
        // public IRepository<Product> ProductRepository => _productRepository ??= (_productRepository = new Repository<Product>(AppDbContext));

        #endregion

        #region Commit

        public async Task CommitAsync()
        {
            try
            {
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
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
