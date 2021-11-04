using SCSS.Application.ScrapCollector.Models;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetNotifications(BaseFilterModel model);

        /// <summary>
        /// Reads the notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ReadNotification(Guid id);

        /// <summary>
        /// Gets the number of un read notifications.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetNumberOfUnReadNotifications();

        /// <summary>
        /// Reads all notifications.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> ReadAllNotifications();

    }
}
