using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.AWSService.Models.SQSModels;
using SCSS.MapService.Models;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.Validations.InvalidResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public partial class CollectingRequestService
    {
        #region Get List Of Collecting Request which was received by collector

        /// <summary>
        /// Gets the collecting request received list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequestReceivedList(CollectingRequestReceivingFilterModel model)
        {
            var collectorId = UserAuthSession.UserSession.Id;
            var receivingDataQuery = _collectingRequestRepository.GetMany(x => x.CollectorAccountId.Equals(collectorId) &&
                                                                               x.Status == CollectingRequestStatus.APPROVED)
                                                                  .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                                               (x, y) => new
                                                                               {
                                                                                   CollectingRequestId = x.Id,
                                                                                   x.CollectingRequestCode,
                                                                                   x.CollectingRequestDate,
                                                                                   x.SellerAccountId,
                                                                                   x.TimeFrom,
                                                                                   x.TimeTo,
                                                                                   x.IsBulky,
                                                                                   y.Address,
                                                                                   y.AddressName,
                                                                                   y.Latitude,
                                                                                   y.Longitude,
                                                                               })
                                                                  .Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.SellerPhone) || x.Phone.Contains(model.SellerPhone))), x => x.SellerAccountId, y => y.Id,
                                                                              (x, y) => new
                                                                              {
                                                                                  x.CollectingRequestId,
                                                                                  x.CollectingRequestCode,
                                                                                  x.CollectingRequestDate,
                                                                                  x.TimeFrom,
                                                                                  x.TimeTo,
                                                                                  x.IsBulky,
                                                                                  x.Address,
                                                                                  x.AddressName,
                                                                                  x.Latitude,
                                                                                  x.Longitude,
                                                                                  SellerName = y.Name,
                                                                              });
            if (!receivingDataQuery.Any())
            {
                return BaseApiResponse.OK(totalRecord: NumberConstant.Zero, resData: CollectionConstants.Empty<CollectingRequestReceivingViewModel>());
            }

            // Get Destination List
            var destinationCoordinateRequest = receivingDataQuery.Select(x => new DestinationCoordinateModel()
            {
                Id = x.CollectingRequestId,
                DestinationLatitude = x.Latitude.Value,
                DestinationLongtitude = x.Longitude.Value
            }).ToList();

            var mapDistanceMatrixCoordinate = new DistanceMatrixCoordinateRequestModel()
            {
                OriginLatitude = model.OriginLatitude.Value,
                OriginLongtitude = model.OriginLongtitude.Value,
                DestinationItems = destinationCoordinateRequest,
                Vehicle = Vehicle.hd
            };

            // Call to Goong Map Service to get distance between collector location and collecting request location
            var destinationDistancesRes = await _mapDistanceMatrixService.GetDistanceFromOriginToMultiDestinations(mapDistanceMatrixCoordinate);

            // Get Seller Role 
            var sellerRoleId = UnitOfWork.RoleRepository.Get(x => x.Key.Equals(AccountRole.SELLER)).Id;

            var receivingData = destinationDistancesRes.Join(receivingDataQuery, x => x.DestinationId, y => y.CollectingRequestId,
                                                             (x, y) => new
                                                             {
                                                                 y.CollectingRequestId,
                                                                 y.CollectingRequestCode,
                                                                 y.CollectingRequestDate,
                                                                 y.TimeFrom,
                                                                 y.TimeTo,
                                                                 y.Address,
                                                                 y.AddressName,
                                                                 y.IsBulky,
                                                                 y.SellerName,
                                                                 x.DistanceVal,
                                                                 x.DistanceText,
                                                             }).OrderBy(x => x.DistanceVal);

            var totalRecord = receivingData.Count();

            var dataResult = receivingData.Select(x => new CollectingRequestReceivingViewModel()
            {
                Id = x.CollectingRequestId,
                CollectingRequestCode = x.CollectingRequestCode,
                SellerName = x.SellerName,
                // Date
                DayOfWeek = x.CollectingRequestDate.GetDayOfWeek(),
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                FromTime = x.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = x.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                // Location
                CollectingAddressName = x.AddressName,
                CollectingAddress = x.Address,
                IsBulky = x.IsBulky,
                Distance = x.DistanceVal,
                DistanceText = x.DistanceText,
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion Get List Of Collecting Request which have received

        #region Get Collecting Request Detail which was received by collector

        /// <summary>
        /// Gets the collecting request detail received.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequestDetailReceived(Guid id)
        {
            var collectingRequestEntity = await _collectingRequestRepository.GetAsyncAsNoTracking(x => x.Id.Equals(id) &&
                                                                                                       x.CollectorAccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                                       x.Status == CollectingRequestStatus.APPROVED);
            if (collectingRequestEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var locationEntity = _locationRepository.GetAsNoTracking(x => x.Id.Equals(collectingRequestEntity.LocationId));
            var sellerInfo = _accountRepository.GetAsNoTracking(x => x.Id.Equals(collectingRequestEntity.SellerAccountId));

            var dataResult = new CollectingRequestDetailReceivingViewModel()
            {
                Id = collectingRequestEntity.Id,
                CollectingRequestCode = collectingRequestEntity.CollectingRequestCode,
                CollectingRequestDate = collectingRequestEntity.CollectingRequestDate.ToStringFormat(DateTimeFormat.DDD_dd_MMM),
                DayOfWeek = collectingRequestEntity.CollectingRequestDate.GetDayOfWeek(),
                FromTime = collectingRequestEntity.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = collectingRequestEntity.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                IsBulky = collectingRequestEntity.IsBulky,
                Note = collectingRequestEntity.Note,
                ScrapImgUrl = collectingRequestEntity.ScrapImageUrl,
                // Location
                CollectingAddressName = locationEntity.AddressName,
                CollectingAddress = locationEntity.AddressName,
                Latitude = locationEntity.Latitude,
                Longtitude = locationEntity.Longitude,
                // Seller Information
                SellerName = sellerInfo.Name,
                SellerImgUrl = sellerInfo.ImageUrl,
                SellerPhone = sellerInfo.Phone
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion Get Collecting Request Detail which was received by collector

        #region Cancel Collecting Request By Collector

        /// <summary>
        /// Cancels the collecting request received.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CancelCollectingRequestReceived(CollectingRequestReceivedCancelModel model)
        {
            var entity = _collectingRequestRepository.GetById(model.Id);

            if (entity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var errorList = ValidateCancelCollectingRequest(entity.CollectorAccountId, entity.Status, model.CancelReason);

            if (errorList.Any())
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid, errorList);
            }

            entity.Status = CollectingRequestStatus.CANCEL_BY_COLLECTOR;
            entity.CancelReason = model.CancelReason;

            try
            {
                _collectingRequestRepository.Update(entity);
                await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                return BaseApiResponse.Error(SystemMessageCode.SystemException);
            }

            // Push Notification to notice seller that their collecting request was canceled by collector
            var sellerInfo = _accountRepository.GetById(entity.SellerAccountId);

            var notifications = new List<NotificationMessageQueueModel>()
            {
                new NotificationMessageQueueModel()
                {
                    AccountId = sellerInfo.Id,
                    Body = NotificationMessage.CollectingRequestCancelBody(entity.CollectingRequestCode),
                    Title = NotificationMessage.CollectingRequestCancelTitle(),
                    DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, entity.Id.ToString()), 
                    DeviceId = sellerInfo.DeviceId,
                    NotiType = CollectingRequestStatus.CANCEL_BY_COLLECTOR
                },
                new NotificationMessageQueueModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    Body = NotificationMessage.CancelCRBySellerTitle, 
                    Title = NotificationMessage.CancelCRByCollector(entity.CollectingRequestCode), 
                    DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.HistoryScreen, entity.Id.ToString()),
                    DeviceId = UserAuthSession.UserSession.DeviceId,
                    NotiType = CollectingRequestStatus.CANCEL_BY_COLLECTOR
                }
            };

            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(notifications);

            // Remove reminder in RemiderCacche
            var reminderCaches = _cacheListService.CollectingRequestReminderCache.GetMany(x => x.Id.Equals(entity.Id));
            await _cacheListService.CollectingRequestReminderCache.RemoveRangeAsync(reminderCaches);

            return BaseApiResponse.OK();
        }

        #endregion Cancel Collecting Request By Collector

        #region Validate Cancel Collecting Request Received

        /// <summary>
        /// Validates the cancel collecting request.
        /// </summary>
        /// <param name="collectorAccountId">The collector account identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="collectingRequestDate">The collecting request date.</param>
        /// <param name="timeTo">The time to.</param>
        /// <returns></returns>
        private List<ValidationError> ValidateCancelCollectingRequest(Guid? collectorAccountId, int? status, string cancelReason)
        {
            var errorList = new List<ValidationError>();

            if (!collectorAccountId.Equals(UserAuthSession.UserSession.Id))
            {
                errorList.Add(new ValidationError(nameof(collectorAccountId), InvalidCollectingRequestCode.InvalidCollector));
            }

            if (status != CollectingRequestStatus.APPROVED)
            {
                errorList.Add(new ValidationError(nameof(status), InvalidCollectingRequestCode.InvalidStatus));
            }

            if (ValidatorUtil.IsBlank(cancelReason))
            {
                errorList.Add(new ValidationError(nameof(cancelReason), InvalidCollectingRequestCode.InvalidDate));
            }


            //if (!collectingRequestDate.IsCompareDateTimeEqual(DateTimeVN.DATETIME_NOW))
            //{
            //    errorList.Add(new ValidationError(nameof(collectingRequestDate), InvalidCollectingRequestCode.InvalidDate));
            //}

            //if (timeTo.IsCompareTimeSpanGreaterOrEqual(DateTimeVN.TIMESPAN_NOW))
            //{
            //    errorList.Add(new ValidationError(nameof(timeTo), InvalidCollectingRequestCode.InvalidTimeTo));
            //}

            return errorList;

        }

        #endregion Validate Cancel Collecting Request Received
    }
}
