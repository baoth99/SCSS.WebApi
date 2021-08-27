using SCSS.Utilities.Configurations;
using System;

namespace SCSS.ORM.Dapper
{
    public class DapperBaseService : IDapperBaseService
    {
        #region SQL Connection String

        /// <summary>
        /// Gets the SQL connection string.
        /// </summary>
        /// <value>
        /// The SQL connection string.
        /// </value>
        protected string SqlConnectionString { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DapperBaseService"/> class.
        /// </summary>
        public DapperBaseService()
        {
            SqlConnectionString = AppSettingValues.SqlConnectionString;
        }

        #endregion

        #region Disponse

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }
            }
            this.Disposed = true;
        }

        #endregion

    }
}
