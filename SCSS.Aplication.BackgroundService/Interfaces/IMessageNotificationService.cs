using SCSS.Aplication.BackgroundService.Models.MessageNotificationModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface IMessageNotificationService
    {
        /// <summary>
        /// Pushes the notification.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task PushNotification(MessageNotificationPushModel model);

        /// <summary>
        /// Pushes the many notifications.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns></returns>
        Task PushManyNotifications(List<MessageNotificationPushModel> models);

    }
}
