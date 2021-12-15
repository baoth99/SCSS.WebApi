using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Hosting;
using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using SCSS.Utilities.Extensions;
using SCSS.WebApi.SignalR.CollectorHubs.Hubs;
using SCSS.WebApi.SignalR.CollectorHubs.IHubs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.WebApi.BackgroundJobs
{
    public class CollectingRequestRealtimeHostService : IHostedService, IDisposable
    {
        #region Services

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSSubscriberService _SQSSubscriberService;

        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region HubContext

        /// <summary>
        /// The collecing request hub context
        /// </summary>
        private readonly IHubContext<CollectingRequestHub, ICollecingRequestHub> _collecingRequestHubContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestRealtimeHostService"/> class.
        /// </summary>
        /// <param name="sQSSubscriberService">The s qs subscriber service.</param>
        /// <param name="collecingRequestHubContext">The collecing request hub context.</param>
        public CollectingRequestRealtimeHostService(ISQSSubscriberService sQSSubscriberService, IHubContext<CollectingRequestHub, ICollecingRequestHub> collecingRequestHubContext)
        {
            _SQSSubscriberService = sQSSubscriberService;
            _collecingRequestHubContext = collecingRequestHubContext;
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
                    var dataFromQueue = await ProcessQueue();

                    if (dataFromQueue.Any())
                    {
                        foreach (var item in dataFromQueue)
                        {
                            var realtimeModel = new CollectingRequestNoticeModel()
                            {
                                Id = item.CollectingRequestId,
                                RequestType = item.RequestType,
                                Status = item.Status
                            };

                            var jsonModel = realtimeModel.ToJson();

                            await _collecingRequestHubContext.Clients.All.ReceiveCollectingRequest(jsonModel);
                        }
                    }
                }
            }, cancellationToken);

            return Task.CompletedTask;
        }


        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <returns></returns>
        private async Task<List<CollectingRequestRealtimeQueueModel>> ProcessQueue()
        {
            return await _SQSSubscriberService.CollectingRequestRealtimeSubscriber.ReceiveMessageAsync();
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
