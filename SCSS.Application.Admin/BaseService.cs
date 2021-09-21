using SCSS.Application.Admin.Models.NotificationModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.FirebaseService.Models;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.Admin
{
    public class BaseService
    {
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Gets the user authentication session.
        /// </summary>
        /// <value>
        /// The user authentication session.
        /// </value>
        protected IAuthSession UserAuthSession { get; private set; }

        /// <summary>
        /// Gets the logger service.
        /// </summary>
        /// <value>
        /// The logger service.
        /// </value>
        protected ILoggerService Logger { get; private set; }

        /// <summary>
        /// Gets the FCM service.
        /// </summary>
        /// <value>
        /// The FCM service.
        /// </value>
        protected IFCMService FCMService { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        public BaseService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService)
        {
            UnitOfWork = unitOfWork;
            UserAuthSession = userAuthSession;
            Logger = logger;
            FCMService = fcmService;
        }

        #endregion


        #region Store and Send Many Notification

        /// <summary>
        /// Stores and sends many notifications.
        /// </summary>
        /// <param name="notifications">The notifications.</param>
        protected async Task StoreAndSendManyNotifications(List<NotificationCreateModel> notifications)
        {
            if (!notifications.Any())
            {
                return;
            }

            // Store Notifications into Database
            var notificationEntities = notifications.Select(x => new Notification()
            {
                AccountId = x.AccountId,
                Title = x.Title,
                Body = x.Body,
                DataCustom = x.DataCustom != null ? x.DataCustom.ToJson<string, string>() : string.Empty,
                IsRead = BooleanConstants.FALSE,
            }).ToList();

            UnitOfWork.NotificationRepository.InsertRange(notificationEntities);

            await UnitOfWork.CommitAsync();

            // Send Notification

            var notificationMessages = notifications.Select(x => new NotificationRequestModel()
            {
                Title = x.Title,
                Body = x.Body,
                Data = x.DataCustom,
                DeviceId = x.DeviceId,
            }).ToList();

            await FCMService.PushManyNotifications(notificationMessages);
        }

        #endregion

        #region Store and Send Notification

        /// <summary>
        /// Stores and send notification.
        /// </summary>
        /// <param name="notification">The notification.</param>
        protected async Task StoreAndSendNotification(NotificationCreateModel notification)
        {
            // Store Notifications into Database
            var notificationEntity = new Notification()
            {
                AccountId = notification.AccountId,
                Title = notification.Title,
                Body = notification.Body,
                DataCustom = notification.DataCustom != null ? notification.DataCustom.ToJson<string, string>() : string.Empty,
                IsRead = BooleanConstants.FALSE,
            };

            UnitOfWork.NotificationRepository.Insert(notificationEntity);
            await UnitOfWork.CommitAsync();

            // Send Notification

            var notificationMessage = new NotificationRequestModel()
            {
                Title = notification.Title,
                Body = notification.Body,
                Data = notification.DataCustom,
                DeviceId = notification.DeviceId,
            };

            await FCMService.PushNotification(notificationMessage);
        }

        #endregion
    }
}
