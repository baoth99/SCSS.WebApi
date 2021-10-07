using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.AWSService.Models.SQSModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.Validations.InvalidResponseModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public partial class CollectingRequestService : BaseService, ICollectingRequestService
    {
        #region Repositories

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Services

        /// <summary>
        /// The cache list service
        /// </summary>
        private readonly ICacheListService _cacheListService;

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="cacheListService">The cache list service.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService,
                                        ICacheListService cacheListService, ISQSPublisherService SQSPublisherService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _cacheListService = cacheListService;
            _SQSPublisherService = SQSPublisherService;
        }

        #endregion

        #region Get Operating Time Range

        /// <summary>
        /// Gets the operating time range.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetOperatingTimeRange()
        {
            var operatingTimeRange = await OperatingTimeRange();

            if (operatingTimeRange == null)
            {
                return BaseApiResponse.OK();
            }

            var result = new OperatingTimeRangeViewModel()
            {
                OperatingFromTime = operatingTimeRange.Item1.ToStringFormat(TimeSpanFormat.HH_MM),
                OperatingToTime = operatingTimeRange.Item2.ToStringFormat(TimeSpanFormat.HH_MM)
            };

            return BaseApiResponse.OK(result);
        }

        #endregion

        #region Request Scrap Collecting 

        /// <summary>
        /// Requests the scrap collecting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RequestScrapCollecting(CollectingRequestCreateModel model)
        {
            var collectingRequestFromTime = model.FromTime.ToTimeSpan().Value;
            var collectingRequestToTime = model.ToTime.ToTimeSpan().Value;

            // Validate Collecting Request Time
            var errorList = await ValidateCollectingRequestTime(model.CollectingRequestDate.ToDateTime(), collectingRequestFromTime, collectingRequestToTime);

            if (errorList.Any())
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid, errorList);
            }

            var sellerAccountId = UserAuthSession.UserSession.Id;

            // Create new Collecting Request Location Entity
            var locationEntity = new Location()
            {
                Address = model.Address,
                AddressName = model.AddressName,
                Latitude = model.Latitude,
                Longitude = model.Longtitude
            };

            // Insert Collecting Request Location Entity
            var locationInsertEntity = _locationRepository.Insert(locationEntity);

            // Auto Generate CollectingRequestEntityCode from CollectingRequestDate, collectingRequestFromTime and collectingRequestToTime
            var collectingRequestEntityCode = await GenerateCollectingRequestCode(model.CollectingRequestDate.ToDateTime().Value, collectingRequestFromTime, collectingRequestToTime);

            // Create new Collecting Request Entity
            var collectingRequestEntity = new CollectingRequest()
            {
                CollectingRequestCode = collectingRequestEntityCode,
                CollectingRequestDate = model.CollectingRequestDate.ToDateTime().Value.Date,
                TimeFrom = collectingRequestFromTime,
                TimeTo = collectingRequestToTime,
                SellerAccountId = sellerAccountId,
                IsBulky = model.IsBulky,
                ScrapImageUrl = model.CollectingRequestImageUrl,
                Note = model.Note,
                LocationId = locationInsertEntity.Id,
                Status = CollectingRequestStatus.PENDING,
            };

            // Insert Collecting Request Entity 
            var insertEntity =  _collectingRequestRepository.Insert(collectingRequestEntity);

            // Commit Data into Database
            await UnitOfWork.CommitAsync();

            var cacheModel = new PendingCollectingRequestCacheModel()
            {
                Id = insertEntity.Id,
                Date = insertEntity.CollectingRequestDate.Value.Date,
                FromTime = insertEntity.TimeFrom,
                ToTime = insertEntity.TimeTo
            };

            await _cacheListService.PendingCollectingRequestCache.PushAsync(cacheModel);


            var message = new NotificationMessageQueueModel()
            {
                AccountId = UserAuthSession.UserSession.Id,
                DeviceId = UserAuthSession.UserSession.DeviceId,
                DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, insertEntity.Id.ToString()), 
                Title = NotificationMessage.SellerRequestCRTitle,
                Body = NotificationMessage.SellerRequestCRBody(insertEntity.CollectingRequestCode),
                NotiType = CollectingRequestStatus.PENDING
            };

            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessageAsync(message);

            return BaseApiResponse.OK();
        }


        /// <summary>
        /// Generates the collecting request code.
        /// </summary>
        /// <param name="collectingRequestDate">The collecting request date.</param>
        /// <param name="fromTime">From time.</param>
        /// <param name="toTime">To time.</param>
        /// <returns></returns>
        private async Task<string> GenerateCollectingRequestCode(DateTime collectingRequestDate, TimeSpan fromTime, TimeSpan toTime)
        {
            var collectingRequestCount = await _collectingRequestRepository.GetAllAsNoTracking().CountAsync();

            string collectingRequestDateCode = collectingRequestDate.ToDateCode(DateCodeFormat.DDMMYYYY);
            string fromTimeCode = fromTime.ToTimeSpanCode(TimeSpanCodeFormat.HHMM);
            string toTimeCode = toTime.ToTimeSpanCode(TimeSpanCodeFormat.HHMM);

            var collectingRequestCode = string.Format(GenerationCodeFormat.COLLECTING_REQUEST_CODE, collectingRequestDateCode,
                                                                fromTimeCode,
                                                                toTimeCode,
                                                                collectingRequestCount);
            return collectingRequestCode;
        }

        #endregion

        #region Cancel Scrap Collecting Request

        /// <summary>
        /// Cancels the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CancelCollectingRequest(CollectingRequestCancelModel model)
        {
            // Get Collecting Request from ID, sellerAccountId and Status = CollectingRequestStatus.PENDING (1)
            var entity = await _collectingRequestRepository.GetByIdAsync(model.Id);

            // Check Collecting Request existed
            if (entity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var errorList = await ValidateCollectingRequest(entity.SellerAccountId, entity.Status, 
                                                            entity.CollectingRequestDate, 
                                                            entity.TimeFrom);

            if (errorList.Any())
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var notifcations = new List<NotificationMessageQueueModel>()
            {
                new NotificationMessageQueueModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    DeviceId = UserAuthSession.UserSession.DeviceId,
                    DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, entity.Id.ToString()),
                    Title = NotificationMessage.CancelCRBySellerTitle,
                    Body = NotificationMessage.CancelCRBySellerBody(entity.CollectingRequestCode),
                    NotiType = CollectingRequestStatus.CANCEL_BY_SELLER
                }
            };

            if (entity.Status == CollectingRequestStatus.APPROVED)
            {
                var collectorDeviceId = UnitOfWork.AccountRepository.GetById(entity.CollectorAccountId)?.DeviceId;
                notifcations.Add(new NotificationMessageQueueModel()
                {
                    AccountId = entity.CollectorAccountId,
                    DeviceId = collectorDeviceId,
                    NotiType = CollectingRequestStatus.CANCEL_BY_SELLER,
                    Title = NotificationMessage.CancelCRBySellerTitle,
                    Body = NotificationMessage.CancelCRBySellerToCollectorBody(entity.CollectingRequestCode),
                    DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.HistoryScreen, entity.CollectorAccountId.ToString())
                });
            }

            // Update Status and Cancel Reason
            entity.Status = CollectingRequestStatus.CANCEL_BY_SELLER;
            entity.CancelReason = model.CancelReason;

            try
            {
                _collectingRequestRepository.Update(entity);
                // Commit Data to Database
                await UnitOfWork.CommitAsync();
            }
            catch (Exception)
            {
                return BaseApiResponse.Error();
            }

            // Remove CRCache in PendingCollectingRequestCache
            var cacheModel = new PendingCollectingRequestCacheModel()
            {
                Id = entity.Id,
                Date = entity.CollectingRequestDate,
                FromTime = entity.TimeFrom,
                ToTime = entity.TimeTo
            };
            await _cacheListService.PendingCollectingRequestCache.RemoveAsync(cacheModel);

            // Remove reminder in RemiderCacche
            var remiderCaches = _cacheListService.CollectingRequestReminderCache.GetMany(x => x.Id.Equals(entity.Id));
            await _cacheListService.CollectingRequestReminderCache.RemoveRangeAsync(remiderCaches);

            // Push to AWS SQS
            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(notifcations);

            return BaseApiResponse.OK();
        }



        /// <summary>
        /// Validates the collecting request.
        /// </summary>
        /// <param name="sellerAccountId">The seller account identifier.</param>
        /// <param name="status">The status.</param>
        /// <param name="collectorAccountId">The collector account identifier.</param>
        /// <returns></returns>
        private async Task<List<string>> ValidateCollectingRequest(Guid? sellerAccountId, int? status, DateTime? collectingRequestDate, TimeSpan? timeFrom)
        {
            var errorList = new List<string>();
            if (!sellerAccountId.Equals(UserAuthSession.UserSession.Id))
            {
                errorList.Add(InvalidCollectingRequestCode.InvalidSeller);
            }

            if (status == CollectingRequestStatus.COMPLETED || status == CollectingRequestStatus.CANCEL_BY_COLLECTOR)
            {
                errorList.Add(InvalidCollectingRequestCode.InvalidStatus);
            }


            if (status == CollectingRequestStatus.APPROVED)
            {
                var cancelTimeRange = await CancelTimeRange();

                var dateTimeFrom = collectingRequestDate.Value.Add(timeFrom.Value);
                var timeRange = (int)dateTimeFrom.Subtract(DateTimeVN.DATETIME_NOW.StripSecondAndMilliseconds()).TotalMinutes;
                if (timeRange <= cancelTimeRange)
                {
                    errorList.Add(InvalidCollectingRequestCode.TimeRangeNotValid);
                }
            }

            return errorList;
        }

        #endregion      

        #region Validate Collecting Request Time

        /// <summary>
        /// Validates the collecting request time.
        /// </summary>
        /// <param name="fromTime">From time.</param>
        /// <param name="toTime">To time.</param>
        /// <returns></returns>
        private async Task<List<ValidationError>> ValidateCollectingRequestTime(DateTime? collectingRequestDate, TimeSpan? fromTime, TimeSpan? toTime)
        {
            var errorList = new List<ValidationError>();

            var days = await MaxNumberDaysSellerRequestAdvance();

            // Check Collecting Request day is more than days
            if (DateTimeUtils.IsMoreThanFutureDays(collectingRequestDate, days))
            {
                errorList.Add(new ValidationError(nameof(collectingRequestDate), InvalidCollectingRequestCode.MoreThanDays));
            }


            // Check Colleting Request in request Day
            // Only One Collecting Request is pending in request day.
            var collectingRequestInDay = _collectingRequestRepository.GetManyAsNoTracking(x => x.SellerAccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                         x.CollectingRequestDate.Value.Date.CompareTo(collectingRequestDate.Value.Date) == NumberConstant.Zero &&
                                                                                         (x.Status == CollectingRequestStatus.PENDING || x.Status == CollectingRequestStatus.APPROVED));

            var requests = await MaxNumberCollectingRequestSellerRequest();

            if (collectingRequestInDay.Count() >= requests)
            {
                errorList.Add(new ValidationError("MaxNumberCR", InvalidCollectingRequestCode.LimitCR));
            }

            //  Check CollectingRequestFromTime with CollectingRequestToTime
            //  If CollectingRequestFromTime is greater than CollectingRequestFromTime 
            if (fromTime.IsCompareTimeSpanGreaterOrEqual(toTime))
            {
                errorList.Add(new ValidationError("FromTimeToTime", InvalidCollectingRequestCode.FromTimeGreaterThanToTime));
            }

            // Check Operating Time Range
            var operatingTimeRange = await OperatingTimeRange();

            if (operatingTimeRange != null)
            {
                var IsValidFromTime = (fromTime.IsCompareTimeSpanGreaterOrEqual(operatingTimeRange.Item1) && fromTime.IsCompareTimeSpanLessOrEqual(operatingTimeRange.Item2));
                var IsValidToTime = (toTime.IsCompareTimeSpanGreaterOrEqual(operatingTimeRange.Item1) && toTime.IsCompareTimeSpanLessOrEqual(operatingTimeRange.Item2));
                if (!(IsValidFromTime && IsValidToTime))
                {
                    errorList.Add(new ValidationError("OperatingTime", InvalidCollectingRequestCode.TimeRangeNotValid));
                }
            }

            // Check CollectingRequestFromTime and CollectingRequestToTime in day
            if (collectingRequestDate.IsCompareDateTimeEqual(DateTimeVN.DATE_NOW))
            {
                if (fromTime.IsCompareTimeSpanLessThan(DateTimeVN.TIMESPAN_NOW.StripMilliseconds()))
                {
                    errorList.Add(new ValidationError(nameof(fromTime), InvalidCollectingRequestCode.FromTimeGreaterThanToTime));
                }
                if (toTime.IsCompareTimeSpanLessThan(DateTimeVN.TIMESPAN_NOW.StripMilliseconds()))
                {
                    errorList.Add(new ValidationError(nameof(toTime), InvalidCollectingRequestCode.FromTimeGreaterThanToTime));
                }

                // Check FromTime in Day
                var timeRange = await TimeRangeRequestNow();

                var isValid = DateTimeUtils.IsMoreThanOrEqualMinutes(DateTimeVN.TIMESPAN_NOW, fromTime, timeRange);
                if (!isValid)
                {
                    errorList.Add(new ValidationError(nameof(fromTime), InvalidCollectingRequestCode.InvalidTimeFrom));
                }
            }

            // Check minutes between fromTime and toTime. if it is less than 15 minutes => error
            if (!DateTimeUtils.IsMoreThanOrEqualMinutes(fromTime, toTime))
            {
                errorList.Add(new ValidationError("Between", InvalidCollectingRequestCode.LessThan15Minutes));
            }

            return errorList;
        }

        #endregion
    }
}
