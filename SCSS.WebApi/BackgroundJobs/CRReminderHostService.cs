using Microsoft.Extensions.Hosting;
using SCSS.Aplication.BackgroundService.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.WebApi.BackgroundJobs
{
    public class CRReminderHostService : IHostedService, IDisposable
    {
        #region Services

        /// <summary>
        /// The queue handling service
        /// </summary>
        private readonly IQueueHandlingService _queueHandlingService;

        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CRReminderHostService"/> class.
        /// </summary>
        /// <param name="queueHandlingService">The queue handling service.</param>
        public CRReminderHostService(IQueueHandlingService queueHandlingService)
        {
            _queueHandlingService = queueHandlingService;
        }

        #endregion

        #region Start Async

        /// <summary>
        /// Triggered when the application host is ready to start the service.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the start process has been aborted.</param>
        /// <returns></returns>
        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(async () =>
            {
                while (!cancellationToken.IsCancellationRequested)
                {
                    await _queueHandlingService.HandleCollectingRequestReminderQueue();
                    await Task.Delay(1000);
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }

        #endregion

        #region Stop Async

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// </summary>
        /// <param name="cancellationToken">Indicates that the shutdown process should no longer be graceful.</param>
        /// <returns></returns>
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
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
