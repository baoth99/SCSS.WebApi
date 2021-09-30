using Amazon;
using Amazon.SQS;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using SCSS.AWSService.SQSHandlers;
using SCSS.Utilities.Configurations;

namespace SCSS.AWSService.Implementations
{
    public class SQSPublisherService : AWSBaseService, ISQSPublisherService
    {
        #region Service

        /// <summary>
        /// The amazon SQS
        /// </summary>
        private readonly IAmazonSQS AmazonSQS;

        #endregion

        #region Private variable

        private ISQSPublisher<NotificationMessageQueueModel> _notificationMessageQueuePublisher;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SQSService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SQSPublisherService(ILoggerService logger) : base(logger)
        {
            AmazonSQS = new AmazonSQSClient(AppSettingValues.AWSAccessKey, AppSettingValues.AWSSecrectKey, RegionEndpoint.APSoutheast1);
        }

        #endregion

        #region Publish Access

        public ISQSPublisher<NotificationMessageQueueModel> NotificationMessageQueuePublisher => _notificationMessageQueuePublisher ??= (_notificationMessageQueuePublisher = new SQSPublisher<NotificationMessageQueueModel>(AmazonSQS, AppSettingValues.NotificationQueueUrl, Logger));

        #endregion

    }

    public class SQSSubscriberService : AWSBaseService, ISQSSubscriberService
    {

        #region Service

        /// <summary>
        /// The amazon SQS
        /// </summary>
        private readonly IAmazonSQS AmazonSQS;

        #endregion

        #region Private variable

        private ISQSSubscriber<NotificationMessageQueueModel> _notificationMessageQueueSubscriber;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SQSSubscriberService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public SQSSubscriberService(ILoggerService logger) : base(logger)
        {
            AmazonSQS = new AmazonSQSClient(AppSettingValues.AWSAccessKey, AppSettingValues.AWSSecrectKey, RegionEndpoint.APSoutheast1);
        }

        #endregion

        #region  Publish Access

        public ISQSSubscriber<NotificationMessageQueueModel> NotificationMessageQueueSubscriber => _notificationMessageQueueSubscriber ??= (_notificationMessageQueueSubscriber = new SQSSubscriber<NotificationMessageQueueModel>(AmazonSQS, AppSettingValues.NotificationQueueUrl, Logger));

        #endregion

    }
}
