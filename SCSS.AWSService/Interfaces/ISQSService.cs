using SCSS.AWSService.Models.SQSModels;
using SCSS.AWSService.SQSHandlers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        #endregion
    }
}
