using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class NotificationController : BaseScrapSellerControllers
    {
        #region Services

        /// <summary>
        /// The notification service
        /// </summary>
        private readonly INotificationService _notificationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationController"/> class.
        /// </summary>
        /// <param name="notificationService">The notification service.</param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        #endregion

        #region Get Notifications

        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.NotificationApiUrl.Get)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetNotifications([FromQuery] BaseFilterModel model)
        {
            return await _notificationService.GetNotifications(model);
        }

        #endregion

        #region Reads The Notification

        /// <summary>
        /// Reads the notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.NotificationApiUrl.Read)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReadNotification([FromQuery] Guid id)
        {
            return await _notificationService.ReadNotification(id);
        }

        #endregion

        #region Get Notification Detail

        /// <summary>
        /// Gets the notification detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.NotificationApiUrl.GetDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetNotificationDetail([FromQuery] Guid id)
        {
            return await _notificationService.GetNotificationDetail(id);
        }

        #endregion

        #region Get Number Of UnRead Notifications

        /// <summary>
        /// Gets the number of un read notifications.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.NotificationApiUrl.GetNumberOfUnReadNotifications)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetNumberOfUnReadNotifications()
        {
            return await _notificationService.GetNumberOfUnReadNotifications();
        }

        #endregion
    }
}
