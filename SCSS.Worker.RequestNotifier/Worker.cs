using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Aplication.BackgroundService.Models.RequestNotifierModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.Worker.RequestNotifier
{
    public class Worker : BackgroundService
    {
        #region Services

        /// <summary>
        /// The SQS subscriber service
        /// </summary>
        private readonly ISQSSubscriberService _SQSSubscriberService;

        /// <summary>
        /// The scope factory
        /// </summary>
        private readonly IServiceScopeFactory _scopeFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="SQSSubscriberService">The SQS subscriber service.</param>
        /// <param name="scopeFactory">The scope factory.</param>
        public Worker(ISQSSubscriberService SQSSubscriberService, IServiceScopeFactory scopeFactory)
        {
            _SQSSubscriberService = SQSSubscriberService;
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
            while (!stoppingToken.IsCancellationRequested)
            {
                await DoWork();
            }
        }

        #endregion

        #region Do Work

        /// <summary>
        /// Does the work.
        /// </summary>
        private async Task DoWork()
        {
            var dataFromQueue = await ProcessQueue();

            if (dataFromQueue.Any())
            {
                var models = dataFromQueue.Select(x => new RequestNotifierRequestModel()
                {
                    CollectingRequestId = x.CollectingRequestId,
                    Latitude = x.Latitude,
                    Longitude = x.Longitude,
                    RequestType = x.RequestType
                }).ToList();

                using (var scope = _scopeFactory.CreateScope())
                {
                    var service = scope.ServiceProvider.GetRequiredService<IRequestNotifierService>();
                    await service.HandleRequestNotification(models);
                }
            }
        }

        #endregion

        #region Processes The Queue

        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <returns></returns>
        private async Task<List<CollectingRequestNotiticationQueueModel>> ProcessQueue()
        {
            return await _SQSSubscriberService.CollectingRequestNotiticationSubscriber.ReceiveMessageAsync();
        }

        #endregion
    }
}
