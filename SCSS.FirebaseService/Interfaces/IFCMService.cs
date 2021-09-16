using SCSS.FirebaseService.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.FirebaseService.Interfaces
{
    public interface IFCMService
    {
        /// <summary>
        /// Pushes the notification.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task PushNotification(NotificationRequestModel model);

        /// <summary>
        /// Pushes the many notifications.
        /// </summary>
        /// <param name="modelList">The model list.</param>
        /// <returns></returns>
        Task PushManyNotifications(List<NotificationRequestModel> modelList);
    }
}
