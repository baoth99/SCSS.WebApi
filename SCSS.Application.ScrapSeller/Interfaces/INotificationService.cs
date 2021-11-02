using SCSS.Application.ScrapSeller.Models;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface INotificationService
    {
        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetNotifications(BaseFilterModel model);

        /// <summary>
        /// Gets the notification detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetNotificationDetail(Guid id);

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
