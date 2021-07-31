using SCSS.FirebaseService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.FirebaseService.Interfaces
{
    public interface IFCMService
    {
        Task PushNotification(NotificationRequestModel model);
    }
}
