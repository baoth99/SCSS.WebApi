using FirebaseAdmin.Messaging;
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
        public async Task PushNotification(NotificationRequestModel model)
        {
            var messaging = FirebaseMessaging.DefaultInstance;

        }
    }
}
