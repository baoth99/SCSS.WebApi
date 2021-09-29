using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Aplication.BackgroundService.Models.MessageNotificationModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.FirebaseService.Models;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class MessageNotificationService : BaseService, IMessageNotificationService
    {

        #region Repositoris

        /// <summary>
        /// The notification repository
        /// </summary>
        private readonly IRepository<Notification> _notificationRepository;

        #endregion

        #region Services

        /// <summary>
        /// The FCM service
        /// </summary>
        private readonly IFCMService _FCMService;

        #endregion


        #region Constructor

        public MessageNotificationService(IUnitOfWork unitOfWork, IFCMService FCMService) : base(unitOfWork)
        {
            _notificationRepository = unitOfWork.NotificationRepository;
            _FCMService = FCMService;
        }

        #endregion

        #region Push Notification

        /// <summary>
        /// Pushes the notification.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task PushNotification(MessageNotificationPushModel model)
        {
            // Store Notifications into Database
            var notificationEntity = new Notification()
            {
                AccountId = model.AccountId,
                Title = model.Title,
                Body = model.Body,
                DataCustom = model.DataCustom != null ? model.DataCustom.ToJson<string, string>() : string.Empty,
                IsRead = BooleanConstants.FALSE,
            };

            _notificationRepository.Insert(notificationEntity);

            await UnitOfWork.CommitAsync();

            var notificationMessage = new NotificationRequestModel()
            {
                Title = model.Title,
                Body = model.Body,
                Data = model.DataCustom,
                DeviceId = model.DeviceId,
            };
            await _FCMService.PushNotification(notificationMessage);

        }

        #endregion

        #region Push Many Notifications

        /// <summary>
        /// Pushes the many notifications.
        /// </summary>
        /// <param name="models">The models.</param>
        public async Task PushManyNotifications(List<MessageNotificationPushModel> models)
        {
            if (!models.Any())
            {
                return;
            }

            var notificationEntities = models.Select(x => new Notification()
            {
                AccountId = x.AccountId,
                Title = x.Title,
                Body = x.Body,
                DataCustom = x.DataCustom != null ? x.DataCustom.ToJson<string, string>() : string.Empty,
                IsRead = BooleanConstants.FALSE,
            }).ToList();

            //_notificationRepository.InsertRange(notificationEntities);
            //await UnitOfWork.CommitAsync();

            var notificationMessages = models.Select(x => new NotificationRequestModel()
            {
                Title = x.Title,
                Body = x.Body,
                Data = x.DataCustom,
                DeviceId = x.DeviceId,
            }).ToList();

            await _FCMService.PushManyNotifications(notificationMessages);
            System.Console.WriteLine("Sucessfully !");
        }

        #endregion
    }
}
