using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.MapService.Interfaces;
using SCSS.MapService.Models;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using SCSS.AWSService.Models.SQSModels;
using SCSS.AWSService.Models;
using SCSS.QueueEngine.QueueEngines;
using SCSS.QueueEngine.QueueModels;

namespace SCSS.Application.ScrapCollector.Implementations
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

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The complaint repository
        /// </summary>
        private readonly IRepository<Complaint> _complaintRepository;

        /// <summary>
        /// The collector complaint repository
        /// </summary>
        private readonly IRepository<CollectorComplaint> _collectorComplaintRepository;

        /// <summary>
        /// The collector cancel reason repository
        /// </summary>
        private readonly IRepository<CollectorCancelReason> _collectorCancelReasonRepository;

        #endregion

        #region Service

        /// <summary>
        /// The map distance matrix service
        /// </summary>
        private readonly IMapDistanceMatrixService _mapDistanceMatrixService;

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

        /// <summary>
        /// The cache list service
        /// </summary>
        private readonly ICacheListService _cacheListService;

        /// <summary>
        /// The queue engine factory
        /// </summary>
        private readonly IQueueEngineFactory _queueEngineFactory;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapDistanceMatrixService">The map distance matrix service.</param>
        /// <param name="SQSPublisherService">The SQS publisher service.</param>
        /// <param name="cacheListService">The cache list service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="queueEngineFactory">The queue engine factory.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                        IMapDistanceMatrixService mapDistanceMatrixService, ISQSPublisherService SQSPublisherService, ICacheListService cacheListService,
                                        IStringCacheService cacheService, IQueueEngineFactory queueEngineFactory) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _complaintRepository = unitOfWork.ComplaintRepository;
            _collectorComplaintRepository = unitOfWork.CollectorComplaintRepository;
            _collectorCancelReasonRepository = unitOfWork.CollectorCancelReasonRepository;
            _mapDistanceMatrixService = mapDistanceMatrixService;
            _SQSPublisherService = SQSPublisherService;
            _cacheListService = cacheListService;
            _queueEngineFactory = queueEngineFactory;
        }

        #endregion

        #region Get Collecting Requests

        /// <summary>
        /// Gets the collecting request list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequests(CollectingRequestFilterModel model, List<int> requestTypes)
        {

            var collectingRequestdataQuery = _collectingRequestRepository.GetMany(x => requestTypes.Contains(x.RequestType.Value) &&
                                                                                       x.Status == CollectingRequestStatus.PENDING && x.CollectorAccountId == null)                                             
                                                        .Join(_locationRepository.GetAll(), x => x.LocationId, y => y.Id,
                                                                (x, y) => new
                                                                {
                                                                    CollectingRequestId = x.Id,
                                                                    x.CollectingRequestCode,
                                                                    x.CollectingRequestDate,
                                                                    x.SellerAccountId,
                                                                    x.TimeFrom,
                                                                    x.TimeTo,
                                                                    y.District,
                                                                    y.City,
                                                                    y.Latitude,
                                                                    y.Longitude,
                                                                    x.IsBulky,
                                                                    x.RequestType
                                                                });


            var expiredRequests = collectingRequestdataQuery.Where(x => x.CollectingRequestDate.Value.Date.CompareTo(DateTimeVN.DATE_NOW) == NumberConstant.Zero &&
                                                                                x.TimeTo.Value.CompareTo(DateTimeVN.TIMESPAN_NOW) < NumberConstant.Zero);

            if (expiredRequests.Any())
            {
                collectingRequestdataQuery.Except(expiredRequests);
            }


            if (!collectingRequestdataQuery.Any())
            {
                return BaseApiResponse.OK(CollectionConstants.Empty<CollectingRequestViewModel>());
            }

            // Get Destination List
            var radius = await AvailableRadius();

            var destinationCoordinateRequest = collectingRequestdataQuery.ToList().Where(x => CoordinateHelper.IsInRadius(model.OriginLatitude, model.OriginLongtitude, x.Latitude, x.Longitude, radius))
                                                                                .Select(x => new DestinationCoordinateModel()
                                                                                {
                                                                                    Id = x.CollectingRequestId,
                                                                                    DestinationLatitude = x.Latitude.Value,
                                                                                    DestinationLongtitude = x.Longitude.Value
                                                                                }).ToList();

            if (!destinationCoordinateRequest.Any())
            {
                return BaseApiResponse.OK(CollectionConstants.Empty<CollectingRequestViewModel>());
            }



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

            var collectingRequestData = destinationDistancesRes.Join(collectingRequestdataQuery, x => x.DestinationId, y => y.CollectingRequestId,
                                                   (x, y) => new
                                                   {
                                                       y.CollectingRequestId,
                                                       y.CollectingRequestCode,
                                                       y.CollectingRequestDate,
                                                       y.TimeFrom,
                                                       y.TimeTo,
                                                       y.District,
                                                       y.City,
                                                       y.IsBulky,
                                                       y.Latitude,
                                                       y.Longitude,
                                                       x.DistanceText,
                                                       x.DistanceVal,
                                                       x.DurationTimeText,
                                                       x.DurationTimeVal,
                                                       y.SellerAccountId,
                                                       y.RequestType
                                                   })
                                                  .Join(_accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(sellerRoleId)), x => x.SellerAccountId, y => y.Id,
                                                    (x, y) => new
                                                    {
                                                        x.CollectingRequestId,
                                                        x.CollectingRequestCode,
                                                        x.District,
                                                        x.City,
                                                        x.CollectingRequestDate,
                                                        x.DistanceVal,
                                                        x.DistanceText,
                                                        x.DurationTimeText,
                                                        x.DurationTimeVal,
                                                        FromTime = x.TimeFrom,
                                                        ToTime = x.TimeTo,
                                                        x.IsBulky,
                                                        x.Latitude,
                                                        x.Longitude,
                                                        x.RequestType,
                                                        SellerName = y.Name
                                                    }).OrderBy(x => x.DistanceVal);



            var totalRecord = collectingRequestData.Count();

            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;

            // Convert query data  to response data
            var resData = collectingRequestData.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new CollectingRequestViewModel()
            {
                Id = x.CollectingRequestId,
                CollectingRequestCode = x.CollectingRequestCode,
                Area = $"{x.District}, {x.City}",
                CollectingRequestDate = x.CollectingRequestDate,
                DayOfWeek = x.CollectingRequestDate.GetDayOfWeek(),
                Distance = x.DistanceVal,
                DistanceText = x.DistanceText,
                DurationTimeText = x.DurationTimeText,
                DurationTimeVal = x.DurationTimeVal,
                FromTime = x.FromTime.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = x.ToTime.ToStringFormat(TimeSpanFormat.HH_MM),
                IsBulky = x.IsBulky,
                Latitude = x.Latitude,
                Longtitude = x.Longitude,
                SellerName = x.SellerName,
                RequestType = x.RequestType
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: resData);
        }

        #endregion

        #region Validate Collecting Request To Recevice

        /// <summary>
        /// Checks the maximum number collecting requests collector recevice.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<string> ValidateCollectingRequest(Guid id)
        {
            var collectingRequestEntity = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(id));

            if (collectingRequestEntity == null)
            {
                return SystemMessageCode.DataNotFound;
            }

            if (collectingRequestEntity.Status != CollectingRequestStatus.PENDING || collectingRequestEntity.CollectorAccountId != null)
            {
                return InvalidCollectingRequestCode.InvalidCR;
            }

            var collectingRequests = _collectingRequestRepository.GetManyAsNoTracking(x => x.CollectorAccountId.Equals(UserAuthSession.UserSession.Id));

            if (collectingRequestEntity.RequestType == CollectingRequestType.MAKE_AN_APPOINTMENT)
            {
                var maxNumberOfRequests = await MaxNumberCollectingRequestCollectorReceive();

                var collectingAppointment = collectingRequests.Where(x => x.RequestType == CollectingRequestType.MAKE_AN_APPOINTMENT &&
                                                                                      x.Status == CollectingRequestStatus.APPROVED).ToList();
                if (collectingAppointment.Count >= maxNumberOfRequests)
                {
                    return InvalidCollectingRequestCode.OverReceive;
                }
            }

            if (collectingRequestEntity.RequestType == CollectingRequestType.CURRENT_REQUEST)
            {
                var collectingRequestsNow = collectingRequests.Where(x => x.RequestType == CollectingRequestType.CURRENT_REQUEST &&
                                                                          x.CollectingRequestDate.Value.Date.CompareTo(DateTimeVN.DATE_NOW) == NumberConstant.Zero &&
                                                                          x.Status == CollectingRequestStatus.APPROVED).ToList();
                if (collectingRequestsNow.Any())
                {
                    return InvalidCollectingRequestCode.NoReceiveNow;
                }
            }
            
            var collectingRequestDealine = collectingRequestEntity.CollectingRequestDate.Value.AddTimeSpan(collectingRequestEntity.TimeTo.Value);

            if (DateTimeVN.DATETIME_NOW.StripSecondAndMilliseconds().IsCompareDateTimeGreaterThan(collectingRequestDealine))
            {
                return InvalidCollectingRequestCode.TimeUpToReceive;
            }

            return string.Empty;
        }

        #endregion

        #region Receive the Collecting Request

        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<Tuple<Guid?, Guid, string>> ReceiveCollectingRequest(Guid id)
        {
            var collectingRequestEntity = _collectingRequestRepository.GetById(id);

            collectingRequestEntity.CollectorAccountId = UserAuthSession.UserSession.Id;
            collectingRequestEntity.Status = CollectingRequestStatus.APPROVED;
            collectingRequestEntity.ApprovedTime = DateTimeVN.DATETIME_NOW;
            _collectingRequestRepository.Update(collectingRequestEntity);

            var complaint = new Complaint()
            {
                CollectingRequestId = collectingRequestEntity.Id,
            };

            _complaintRepository.Insert(complaint);

            try
            {
                await UnitOfWork.CommitAsync();

                var cacheModel = new PendingCollectingRequestCacheModel()
                {
                    Id = collectingRequestEntity.Id,
                    Date = collectingRequestEntity.CollectingRequestDate,
                    FromTime = collectingRequestEntity.TimeFrom,
                    ToTime = collectingRequestEntity.TimeTo
                };
                await _cacheListService.PendingCollectingRequestCache.RemoveAsync(cacheModel);

                var requestDateTime = collectingRequestEntity.CollectingRequestDate.Value.Add(collectingRequestEntity.TimeFrom.Value);

                if (DateTimeVN.DATETIME_NOW.IsCompareDateTimeLessThan(requestDateTime))
                {
                    var timeRange = requestDateTime.Subtract(DateTimeVN.DATETIME_NOW).TotalMinutes;
                    if (timeRange >= NumberConstant.Twenty)
                    {
                        var location = _locationRepository.GetById(collectingRequestEntity.LocationId);
                        var queueModel = new CollectingRequestReminderQueueModel()
                        {
                            Id = collectingRequestEntity.Id,
                            CollectingRequestCode = collectingRequestEntity.CollectingRequestCode,
                            CollectorId = collectingRequestEntity.CollectorAccountId,
                            AddressName = location.AddressName,
                            FromTime = collectingRequestEntity.TimeFrom,
                            RequestDate = collectingRequestEntity.CollectingRequestDate,
                            ToTime = collectingRequestEntity.TimeTo,
                        };

                        _queueEngineFactory.CollectingRequestReminderQueueRepos.PushQueue(queueModel);
                    }
                }


                return new Tuple<Guid?, Guid, string>(collectingRequestEntity.SellerAccountId, id, collectingRequestEntity.CollectingRequestCode);
            }
            catch (DbUpdateConcurrencyException)
            {
                return null;
            }
        }

        #endregion Receive the Collecting Request

        #region Send Notification To User

        /// <summary>
        /// Sends the notification to seller.
        /// </summary>
        /// <param name="sellerId">The seller identifier.</param>
        /// <param name="collectingRequestCode"></param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SendNotification(Guid? sellerId, Guid? requestId, string collectingRequestCode)
        {
            var sellerInfo = _accountRepository.GetById(sellerId);

            var notifications = new List<NotificationMessageQueueModel>()
            {
                new NotificationMessageQueueModel()
                {
                    AccountId = sellerInfo.Id,
                    Title = NotificationMessage.SellerCollectingRequestReceviedTitle,
                    Body = NotificationMessage.SellerCollectingRequestReceviedBody(collectingRequestCode),
                    DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, requestId.ToString()),
                    DeviceId = sellerInfo.DeviceId,
                    NotiType = NotificationType.CollectingRequest,
                    ReferenceRecordId = requestId
                },
                new NotificationMessageQueueModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    Title = NotificationMessage.CollectorCollectingRequestReceviedTitle,
                    Body = NotificationMessage.CollectorCollectingRequestReceviedBody(collectingRequestCode), 
                    DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.CollectingRequestScreen, requestId.ToString()), 
                    DeviceId = UserAuthSession.UserSession.DeviceId,
                    NotiType = NotificationType.CollectingRequest,
                    ReferenceRecordId = requestId
                }
            };

            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(notifications);

            return BaseApiResponse.OK();
        }

        #endregion

        #region View Collecting Request Detail to Receive

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id)
        {
            var collectingRequestEntity = await _collectingRequestRepository.GetAsyncAsNoTracking(x => x.Id.Equals(id) &&
                                                                                                       x.Status == CollectingRequestStatus.PENDING &&
                                                                                                       x.CollectorAccountId == null);

            if (collectingRequestEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var locationEntity = _locationRepository.GetById(collectingRequestEntity.LocationId);
            var sellerAccount = _accountRepository.GetById(collectingRequestEntity.SellerAccountId);

            var isAllowedToApprove = await IsAllowedToApprove(collectingRequestEntity.RequestType);

            var responseEntity = new CollectingRequestDetailViewModel()
            {
                Id = collectingRequestEntity.Id,
                CollectingRequestCode = collectingRequestEntity.CollectingRequestCode,
                ScrapImageUrl = collectingRequestEntity.ScrapImageUrl,
                SellerName = sellerAccount.Name,
                SellerGender = sellerAccount.Gender.Value,
                SellerProfileUrl = sellerAccount.ImageUrl,
                // Date
                DayOfWeek = collectingRequestEntity.CollectingRequestDate.GetDayOfWeek(),
                CollectingRequestDate = collectingRequestEntity.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                FromTime = collectingRequestEntity.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = collectingRequestEntity.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                // Location
                Area = $"{locationEntity.District}, {locationEntity.City}",
                Latitude = locationEntity.Latitude,
                Longtitude = locationEntity.Longitude,
                IsBulky = collectingRequestEntity.IsBulky,
                Note = collectingRequestEntity.Note,
                RequestType = collectingRequestEntity.RequestType,
                IsAllowedToApprove = isAllowedToApprove
            };

            return BaseApiResponse.OK(responseEntity);
        }

        #endregion

        #region Is Allowed To Approve

        /// <summary>
        /// Determines whether [is allowed to approve] [the specified request type].
        /// </summary>
        /// <param name="requestType">Type of the request.</param>
        /// <returns>
        ///   <c>true</c> if [is allowed to approve] [the specified request type]; otherwise, <c>false</c>.
        /// </returns>
        private async Task<bool> IsAllowedToApprove(int? requestType)
        {
            var collectingRequests = _collectingRequestRepository.GetManyAsNoTracking(x => x.CollectorAccountId.Equals(UserAuthSession.UserSession.Id));

            if (requestType == CollectingRequestType.MAKE_AN_APPOINTMENT)
            {
                var maxRequest = await MaxNumberCollectingRequestCollectorReceive();

                var collectingRequestsMakeAppointment = collectingRequests.Where(x => x.RequestType == CollectingRequestType.MAKE_AN_APPOINTMENT &&
                                                                                      x.Status == CollectingRequestStatus.APPROVED).ToList();
                return !(collectingRequestsMakeAppointment.Count >= maxRequest);
            }

            var collectingRequestsNow = collectingRequests.Where(x => x.RequestType == CollectingRequestType.CURRENT_REQUEST &&
                                                                          x.CollectingRequestDate.Value.Date.CompareTo(DateTimeVN.DATE_NOW) == NumberConstant.Zero &&
                                                                          x.Status == CollectingRequestStatus.APPROVED).ToList();

            return !(collectingRequestsNow.Any());

        }

        #endregion
    }
}
