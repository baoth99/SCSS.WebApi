using FirebaseAdmin.Messaging;
using SCSS.AWSService.Interfaces;
using SCSS.FirebaseService.Interfaces;
using SCSS.FirebaseService.Models;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.FirebaseService.Implementations
{
    public class FCMService : FirebaseBaseService, IFCMService
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FCMService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        public FCMService(ILoggerService logger) : base(logger)
        {
        }

        #endregion

        #region Push Notification to FCM

        /// <summary>
        /// Pushes the notification.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task PushNotification(NotificationRequestModel model)
        {
            try
            {
                var notitfication = new Notification()
                {
                    Title = model.Title,
                    Body = model.Body
                };

                var message = new Message()
                {
                    Notification = notitfication,
                    Data = model.Data,
                    Token = model.DeviceId
                };
                await FirebaseMessaging.DefaultInstance.SendAsync(message);
                Logger.LogInfo(FirebaseLoggerMessage.PushNotificationSuccess);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, FirebaseLoggerMessage.PushNotificationFail);
            }
        }

        #endregion Push Notification to FCM

        #region Push Many Notifications to FCM

        /// <summary>
        /// Pushes the many notifications.
        /// </summary>
        /// <param name="modelList">The model list.</param>
        public async Task PushManyNotifications(List<NotificationRequestModel> modelList)
        {
            try
            {
                var messageList = modelList.Select(x => new Message()
                {
                    Notification = new Notification()
                    {
                        Title = x.Title,
                        Body = x.Body
                    },
                    Data = x.Data,
                    Token = x.DeviceId,
                });

                await FirebaseMessaging.DefaultInstance.SendAllAsync(messageList);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, FirebaseLoggerMessage.PushNotificationFail);
            }
        }

        #endregion Push Many Notifications to FCM
    }
}
