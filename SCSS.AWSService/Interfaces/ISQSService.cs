using SCSS.AWSService.Models.SQSModels;
using SCSS.AWSService.SQSHandlers;


namespace SCSS.AWSService.Interfaces
{
    public interface ISQSPublisherService
    {
        #region Handlers

        /// <summary>
        /// Gets the notification message queue publisher.
        /// </summary>
        /// <value>
        /// The notification message queue publisher.
        /// </value>
        ISQSPublisher<NotificationMessageQueueModel> NotificationMessageQueuePublisher { get; }

        /// <summary>
        /// Gets the SMS message queue publisher.
        /// </summary>
        /// <value>
        /// The SMS message queue publisher.
        /// </value>
        ISQSPublisher<SMSMessageQueueModel> SMSMessageQueuePublisher { get; }

        #endregion
    }

    public interface ISQSSubscriberService
    {
        #region Handlers

        /// <summary>
        /// Gets the notification message queue subscriber.
        /// </summary>
        /// <value>
        /// The notification message queue subscriber.
        /// </value>
        ISQSSubscriber<NotificationMessageQueueModel> NotificationMessageQueueSubscriber { get; }

        /// <summary>
        /// Gets the SMS message queue subscriber.
        /// </summary>
        /// <value>
        /// The SMS message queue subscriber.
        /// </value>
        ISQSSubscriber<SMSMessageQueueModel> SMSMessageQueueSubscriber { get; }

        #endregion
    }
}
