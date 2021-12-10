using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.Worker.CancelCollectingRequest
{
    public class Worker : BackgroundService
    {
        #region Services

        /// <summary>
        /// The scope factory
        /// </summary>
        private readonly IServiceScopeFactory _scopeFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="scopeFactory">The scope factory.</param>
        public Worker(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        #endregion


        #region Execute Async

        /// <summary>
        /// This method is called when the <see cref="T:Microsoft.Extensions.Hosting.IHostedService" /> starts. The implementation should return a task that represents
        /// the lifetime of the long running operation(s) being performed.
        /// </summary>
        /// <param name="stoppingToken">Triggered when <see cref="M:Microsoft.Extensions.Hosting.IHostedService.StopAsync(System.Threading.CancellationToken)" /> is called.</param>
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //!stoppingToken.IsCancellationRequested
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork();
                await Task.Delay(TimeSpan.FromMinutes(AppSettingValues.DelayMinutesSchedule));
            }
        }

        #endregion

        #region Do Work

        /// <summary>
        /// Does the work.
        /// </summary>
        private async Task DoWork()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<ICollectingRequestService>();
                await service.ScanToCancelCollectingRequest();
            }
        }

        #endregion
    }
}
