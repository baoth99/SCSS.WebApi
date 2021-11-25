using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.WebApi.BackgroundJobs
{
    public class ServiceTransactionHostedService : IHostedService, IDisposable
    {
        #region Services

        /// <summary>
        /// The timer
        /// </summary>
        private Timer _timer;

        /// <summary>
        /// The logger service
        /// </summary>
        private readonly ILoggerService _loggerService;

        /// <summary>
        /// The scope factory
        /// </summary>
        private readonly IServiceScopeFactory _scopeFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceTransactionHostedService"/> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        /// <param name="scopeFactory">The scope factory.</param>
        public ServiceTransactionHostedService(ILoggerService loggerService, IServiceScopeFactory scopeFactory)
        {
            _loggerService = loggerService;
            _scopeFactory = scopeFactory;
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
            TimeSpan intervalTime = TimeSpan.Parse(AppSettingValues.SummaryServiceTransaction);
            var totalMinutes = TimeSpan.Parse(AppSettingValues.SummaryServiceTransaction).Subtract(DateTimeVN.TIMESPAN_NOW).TotalMinutes;
            _timer = new Timer(callback: async o => await DoWork(), state: null, dueTime: TimeSpan.FromMinutes(totalMinutes), period: intervalTime);

            return Task.CompletedTask;
        }

        #endregion

        #region Do Work

        /// <summary>
        /// Does the work.
        /// </summary>
        public async Task DoWork()
        {
            using (var scope = _scopeFactory.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IServiceTransactionService>();
                await service.SummarySeviceFee();
            }
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
            _loggerService.LogInfo("Scans Expired Promotion Hosted Service is stopping.");
            return Task.CompletedTask;
        }

        #endregion

        #region Dispose

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion
    }
}
