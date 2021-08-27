using FirebaseAdmin.Messaging;
using SCSS.AWSService.Interfaces;
using SCSS.FirebaseService.Interfaces;
using SCSS.FirebaseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.FirebaseService.Implementations
{
    public class FCMService : FirebaseBaseService, IFCMService
    {
        public FCMService(ILoggerService logger) : base(logger)
        {
        }

        public async Task PushNotification(NotificationRequestModel model)
        {
            try
            {
                var messaging = FirebaseMessaging.DefaultInstance;

            }
            catch (Exception ex)
            {
                Logger.LogError(ex, "Push Fail !!");
            }
        }
    }
}
