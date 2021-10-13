using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Aplication.BackgroundService.Models.SMSMessageModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.Worker.SMSMessage
{
    public class Worker : BackgroundService
    {
        #region Services

        private readonly ILogger<Worker> _logger;

        /// <summary>
        /// The SQS subscriber service
        /// </summary>
        private readonly ISQSSubscriberService _SQSSubscriberService;

        /// <summary>
        /// The SMS message service
        /// </summary>
        private readonly ISMSMessageService _SMSMessageService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="Worker"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="SQSSubscriberService">The SQS subscriber service.</param>
        /// <param name="SMSMessageService">The SMS message service.</param>
        public Worker(ILogger<Worker> logger, ISQSSubscriberService SQSSubscriberService, ISMSMessageService SMSMessageService)
        {
            _logger = logger;
            _SQSSubscriberService = SQSSubscriberService;
            _SMSMessageService = SMSMessageService;
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

        private async Task DoWork()
        {
            var dataFromQueue = await ProcessQueue();

            if (dataFromQueue.Any())
            {
                var smsModels = dataFromQueue.Select(x => new SMSMessagePushModel()
                {
                    Content = x.Content,
                    Phone = x.Phone
                }).ToList();

                await _SMSMessageService.SendManySMS(smsModels);
            }

        }

        #endregion

        #region Processes the queue

        /// <summary>
        /// Processes the queue.
        /// </summary>
        /// <returns></returns>
        private async Task<List<SMSMessageQueueModel>> ProcessQueue()
        {
            return await _SQSSubscriberService.SMSMessageQueueSubscriber.ReceiveMessageAsync();
        }

        #endregion
    }
}
