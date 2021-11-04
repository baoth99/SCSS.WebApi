﻿using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models;
using SCSS.Application.ScrapDealer.Models.NotificationModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class NotificationService : BaseService, INotificationService
    {
        #region Repositories

        /// <summary>
        /// The notification repository
        /// </summary>
        private readonly IRepository<Notification> _notificationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="NotificationService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public NotificationService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _notificationRepository = unitOfWork.NotificationRepository;
        }

        #endregion

        #region Get Notifications

        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetNotifications(BaseFilterModel model)
        {
            var dataQuery = _notificationRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id)).OrderByDescending(x => x.CreatedTime);

            var totalRecord = await dataQuery.CountAsync();

            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;

            var dataResult = dataQuery.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new NotificationViewModel()
            {
                Id = x.Id,
                Title = x.Title,
                Body = x.Body,
                IsRead = x.IsRead,
                PreviousTime = DateTimeUtils.GetPreviousTime(x.CreatedTime),
                DataCustom = x.DataCustom,
                Date = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                NotiType = NumberConstant.Zero,
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Number Of UnRead Notifications

        /// <summary>
        /// Gets the number of un read notifications.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetNumberOfUnReadNotifications()
        {
            var dataQuery = _notificationRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id) && !x.IsRead);
            var total = await dataQuery.CountAsync();
            return BaseApiResponse.OK(totalRecord: total, resData: total);
        }

        #endregion

        #region Read All Notifications

        /// <summary>
        /// Reads all notifications.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReadAllNotifications()
        {
            var unreadNotifications = _notificationRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                       x.IsRead == BooleanConstants.FALSE).ToList()
                                                             .Select(x =>
                                                             {
                                                                 x.IsRead = BooleanConstants.TRUE;
                                                                 return x;
                                                             }).ToList();

            if (unreadNotifications.Any())
            {
                _notificationRepository.UpdateRange(unreadNotifications);

                await UnitOfWork.CommitAsync();
            }

            return BaseApiResponse.OK();
        }

        #endregion

        #region Read Notification

        /// <summary>
        /// Reads the notification.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReadNotification(Guid id)
        {
            var entity = _notificationRepository.GetById(id);

            if (entity == null)
            {
                return BaseApiResponse.NotFound();
            }

            entity.IsRead = BooleanConstants.TRUE;
            _notificationRepository.Update(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion
    }
}