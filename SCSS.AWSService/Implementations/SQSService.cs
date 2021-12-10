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

        private ISQSPublisher<SMSMessageQueueModel> _SMSMessageQueuePublisher;

        private ISQSPublisher<CollectingRequestNotiticationQueueModel> _collectingRequestNotiticationPublisher;

        private ISQSPublisher<CollectingRequestRealtimeQueueModel> _collectingRequestRealtimePublisher;

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

        /// <summary>
        /// Gets the notification message queue publisher.
        /// </summary>
        /// <value>
        /// The notification message queue publisher.
        /// </value>
        public ISQSPublisher<NotificationMessageQueueModel> NotificationMessageQueuePublisher => _notificationMessageQueuePublisher ??= (_notificationMessageQueuePublisher = new SQSPublisher<NotificationMessageQueueModel>(AmazonSQS, AppSettingValues.NotificationQueueUrl, Logger));

        /// <summary>
        /// Gets the SMS message queue publisher.
        /// </summary>
        /// <value>
        /// The SMS message queue publisher.
        /// </value>
        public ISQSPublisher<SMSMessageQueueModel> SMSMessageQueuePublisher => _SMSMessageQueuePublisher ??= (_SMSMessageQueuePublisher = new SQSPublisher<SMSMessageQueueModel>(AmazonSQS, AppSettingValues.SMSMessageQueueUrl, Logger));

        /// <summary>
        /// Gets the collecting request notitication publisher.
        /// </summary>
        /// <value>
        /// The collecting request notitication publisher.
        /// </value>
        public ISQSPublisher<CollectingRequestNotiticationQueueModel> CollectingRequestNotiticationPublisher => _collectingRequestNotiticationPublisher ??= (_collectingRequestNotiticationPublisher = new SQSPublisher<CollectingRequestNotiticationQueueModel>(AmazonSQS, AppSettingValues.RequestNotifierQueueUrl, Logger));

        /// <summary>
        /// Gets the collecting request realtime publisher.
        /// </summary>
        /// <value>
        /// The collecting request realtime publisher.
        /// </value>
        public ISQSPublisher<CollectingRequestRealtimeQueueModel> CollectingRequestRealtimePublisher => _collectingRequestRealtimePublisher ??= (_collectingRequestRealtimePublisher = new SQSPublisher<CollectingRequestRealtimeQueueModel>(AmazonSQS, AppSettingValues.CollectingRequestRealtimeQueueUrl, Logger));

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

        private ISQSSubscriber<SMSMessageQueueModel> _SMSMessageQueueSubscriber;

        private ISQSSubscriber<CollectingRequestNotiticationQueueModel> _collectingRequestNotiticationSubscriber;

        private ISQSSubscriber<CollectingRequestRealtimeQueueModel> _collectingRequestRealtimeSubscriber;

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

        /// <summary>
        /// Gets the notification message queue subscriber.
        /// </summary>
        /// <value>
        /// The notification message queue subscriber.
        /// </value>
        public ISQSSubscriber<NotificationMessageQueueModel> NotificationMessageQueueSubscriber => _notificationMessageQueueSubscriber ??= (_notificationMessageQueueSubscriber = new SQSSubscriber<NotificationMessageQueueModel>(AmazonSQS, AppSettingValues.NotificationQueueUrl, Logger));

        /// <summary>
        /// Gets the SMS message queue subscriber.
        /// </summary>
        /// <value>
        /// The SMS message queue subscriber.
        /// </value>
        public ISQSSubscriber<SMSMessageQueueModel> SMSMessageQueueSubscriber => _SMSMessageQueueSubscriber ??= (_SMSMessageQueueSubscriber = new SQSSubscriber<SMSMessageQueueModel>(AmazonSQS, AppSettingValues.SMSMessageQueueUrl, Logger));

        /// <summary>
        /// Gets the collecting request notitication subscriber.
        /// </summary>
        /// <value>
        /// The collecting request notitication subscriber.
        /// </value>
        public ISQSSubscriber<CollectingRequestNotiticationQueueModel> CollectingRequestNotiticationSubscriber => _collectingRequestNotiticationSubscriber ??= (_collectingRequestNotiticationSubscriber = new SQSSubscriber<CollectingRequestNotiticationQueueModel>(AmazonSQS, AppSettingValues.RequestNotifierQueueUrl, Logger));


        /// <summary>
        /// Gets the collecting request realtime subscriber.
        /// </summary>
        /// <value>
        /// The collecting request realtime subscriber.
        /// </value>
        public ISQSSubscriber<CollectingRequestRealtimeQueueModel> CollectingRequestRealtimeSubscriber => _collectingRequestRealtimeSubscriber ??= (_collectingRequestRealtimeSubscriber = new SQSSubscriber<CollectingRequestRealtimeQueueModel>(AmazonSQS, AppSettingValues.CollectingRequestRealtimeQueueUrl, Logger));

        #endregion

    }
}
