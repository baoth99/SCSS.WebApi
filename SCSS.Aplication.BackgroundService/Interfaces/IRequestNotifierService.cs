using SCSS.Aplication.BackgroundService.Models.RequestNotifierModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface IRequestNotifierService
    {
        /// <summary>
        /// Handles the request notification.
        /// </summary>
        /// <param name="models">The models.</param>
        /// <returns></returns>
        Task HandleRequestNotification(List<RequestNotifierRequestModel> models);
    }
}
