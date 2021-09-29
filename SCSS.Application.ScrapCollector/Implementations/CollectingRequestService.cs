using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
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
using SCSS.Application.ScrapCollector.Models.NotificationModels;
using SCSS.AWSService.Models.SQSModels;

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
        /// The collecting request rejection repository
        /// </summary>
        private readonly IRepository<CollectingRequestRejection> _collectingRequestRejectionRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The notification repository
        /// </summary>
        private readonly IRepository<Notification> _notificationRepository;

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
        /// <param name="fcmService">The FCM service.</param>
        /// <param name="cacheService">The cache service.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                        IMapDistanceMatrixService mapDistanceMatrixService, ISQSPublisherService SQSPublisherService, 
                                        IFCMService fcmService, ICacheService cacheService) : base(unitOfWork, userAuthSession, logger, fcmService, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _collectingRequestRejectionRepository = unitOfWork.CollectingRequestRejectionRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _notificationRepository = unitOfWork.NotificationRepository;
            _mapDistanceMatrixService = mapDistanceMatrixService;
            _SQSPublisherService = SQSPublisherService;
        }

        #endregion

        #region Get Collecting Request List

        /// <summary>
        /// Gets the collecting request list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequestList(CollectingRequestFilterModel model)
        {
            var filterDate = model.FilterDate.ToDateTime();

            if (filterDate != null)
            {
                if (filterDate.IsCompareDateTimeLessThan(DateTimeVN.DATE_NOW))
                {
                    filterDate = DateTimeVN.DATE_NOW;
                }
            }

            var collectingRequestdataQuery = _collectingRequestRepository.GetMany(x => (ValidatorUtil.IsBlank(filterDate) || x.CollectingRequestDate.Value.Date.CompareTo(filterDate.Value.Date) == NumberConstant.Zero) &&
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
                                                                    y.Address,
                                                                    y.AddressName,
                                                                    y.Latitude,
                                                                    y.Longitude,
                                                                    x.IsBulky,
                                                                });

            collectingRequestdataQuery = collectingRequestdataQuery.Where(x => !(x.CollectingRequestDate.Value.Date.CompareTo(DateTimeVN.DATE_NOW) == NumberConstant.Zero &&
                                                                                x.TimeTo.Value.CompareTo(DateTimeVN.TIMESPAN_NOW) >= NumberConstant.Zero));

            if (!collectingRequestdataQuery.Any())
            {
                return BaseApiResponse.OK(CollectionConstants.Empty<CollectingRequestViewModel>());
            }

            // Get Collecting Request List is pending that Collector rejected
            var collectingRequestRejection = _collectingRequestRejectionRepository.GetManyAsNoTracking(x => (x.CollectorId.Equals(UserAuthSession.UserSession.Id)))
                                                                                  .Join(collectingRequestdataQuery, x => x.CollectingRequestId, y => y.CollectingRequestId,
                                                                                       (x, y) => new
                                                                                       {
                                                                                           y.CollectingRequestId,
                                                                                           y.CollectingRequestCode,
                                                                                           y.CollectingRequestDate,
                                                                                           y.SellerAccountId,
                                                                                           y.TimeFrom,
                                                                                           y.TimeTo,
                                                                                           y.Address,
                                                                                           y.AddressName,
                                                                                           y.Latitude,
                                                                                           y.Longitude,
                                                                                           y.IsBulky
                                                                                       });
            // Check if [collectingRequestRejection] is any 
            if (collectingRequestRejection.Any())
            {
                // Exclude [collectingRequestRejection] from [collectingRequestdataQuery] List
                collectingRequestdataQuery = collectingRequestdataQuery.Except(collectingRequestRejection);
            }

            if (!collectingRequestdataQuery.Any())
            {
                return BaseApiResponse.OK(CollectionConstants.Empty<CollectingRequestViewModel>());
            }

            // Get Destination List
            // Caculate
            model.Radius = model.Radius <= NumberConstant.Zero ? DefaultConstant.Radius : model.Radius;

            var destinationCoordinateRequest = collectingRequestdataQuery.ToList().Where(x => CoordinateHelper.IsInRadius(model.OriginLatitude, model.OriginLongtitude, x.Latitude, x.Longitude, model.Radius.KilometerToMeter()))
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
                                                       y.Address,
                                                       y.AddressName,
                                                       y.IsBulky,
                                                       y.Latitude,
                                                       y.Longitude,
                                                       x.DistanceText,
                                                       x.DistanceVal,
                                                       y.SellerAccountId
                                                   })
                                                  .Join(_accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(sellerRoleId)), x => x.SellerAccountId, y => y.Id,
                                                    (x, y) => new
                                                    {
                                                        x.CollectingRequestId,
                                                        x.CollectingRequestCode,
                                                        CollectingAddress = x.Address,
                                                        CollectingRequestAddressName = x.AddressName,
                                                        x.CollectingRequestDate,
                                                        x.DistanceVal,
                                                        x.DistanceText,
                                                        FromTime = x.TimeFrom,
                                                        ToTime = x.TimeTo,
                                                        x.IsBulky,
                                                        x.Latitude,
                                                        x.Longitude,
                                                        SellerName = y.Name
                                                    }).OrderBy(x => x.DistanceVal);

            var totalRecord = collectingRequestData.Count();

            // Convert query data  to response data
            var resData = collectingRequestData.Select(x => new CollectingRequestViewModel()
            {
                Id = x.CollectingRequestId,
                CollectingRequestCode = x.CollectingRequestCode,
                CollectingAddress = x.CollectingAddress,
                CollectingAddressName = x.CollectingRequestAddressName,
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                DayOfWeek = x.CollectingRequestDate.GetDayOfWeek(),
                Distance = x.DistanceVal,
                DistanceText = x.DistanceText,
                FromTime = x.FromTime.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = x.ToTime.ToStringFormat(TimeSpanFormat.HH_MM),
                IsBulky = x.IsBulky,
                Latitude = x.Latitude,
                Longtitude = x.Longitude,
                SellerName = x.SellerName
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: resData);
        }

        #endregion

        #region Check Max Number Of Collecting Requests that Collector Can Receive;

        /// <summary>
        /// Checks the maximum number collecting requests collector recevices.
        /// </summary>
        /// <returns></returns>
        public async Task<string> CheckMaxNumberCollectingRequestsCollectorRecevice()
        {
            var maxRequest = await MaxNumberCollectingRequestCollectorReceive();

            var collectingRequests = _collectingRequestRepository.GetManyAsNoTracking(x => x.CollectorAccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                          x.Status == CollectingRequestStatus.APPROVED).ToList();
            if (collectingRequests.Count >= maxRequest)
            {
                return InvalidCollectingRequestCode.OverReceive;
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

            if (collectingRequestEntity == null)
            {
                return null;
            }

            if (collectingRequestEntity.Status != CollectingRequestStatus.PENDING || collectingRequestEntity.CollectorAccountId != null)
            {
                return null;
            }

            if (collectingRequestEntity.CollectingRequestDate.IsCompareDateTimeEqual(DateTimeVN.DATE_NOW) &&
                collectingRequestEntity.TimeFrom.IsCompareTimeSpanGreaterOrEqual(DateTimeVN.TIMESPAN_NOW))
            {
                return null;
            }


            collectingRequestEntity.CollectorAccountId = UserAuthSession.UserSession.Id;
            collectingRequestEntity.Status = CollectingRequestStatus.APPROVED;
            _collectingRequestRepository.Update(collectingRequestEntity);

            try
            {
                await UnitOfWork.CommitAsync();

                await RemovePendingCollectingRequestFromCache(id);

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
        public async Task<BaseApiResponseModel> SendNotification(Guid? sellerId, string collectingRequestCode)
        {
            var sellerInfo = _accountRepository.GetById(sellerId);

            var notifications = new List<NotificationMessageQueueModel>()
            {
                new NotificationMessageQueueModel()
                {
                    AccountId = sellerInfo.Id,
                    Title = NotificationMessage.CollectingRequestReceviedTitle(collectingRequestCode),
                    Body = "", // TODO:
                    DataCustom = null, // TODO:
                    DeviceId = sellerInfo.DeviceId
                },
                new NotificationMessageQueueModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    Title = "", // TODO:
                    Body = "", // TODO:
                    DataCustom = null, // TODO:
                    DeviceId = UserAuthSession.UserSession.DeviceId
                }
            };

            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(notifications);

            return BaseApiResponse.OK();
        }

        #endregion

        #region Reject the Colleting Request

        /// <summary>
        /// Rejects the collecting request.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RejectCollectingRequest(CollectingRequestRejectModel model)
        {
            var isExisted = _collectingRequestRepository.IsExisted(x => x.Id.Equals(model.Id) &&
                                                                        x.Status == CollectingRequestStatus.PENDING);

            if (!isExisted)
            {
                return BaseApiResponse.NotFound();
            }

            var rejectionEntity = new CollectingRequestRejection()
            {
                CollectingRequestId = model.Id,
                CollectorId = UserAuthSession.UserSession.Id,
                Reason = model.Reason
            };

            _collectingRequestRejectionRepository.Insert(rejectionEntity);

            await UnitOfWork.CommitAsync();

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
            var accountSellerName = _accountRepository.GetById(collectingRequestEntity.SellerAccountId).Name;

            var responseEntity = new CollectingRequestDetailViewModel()
            {
                Id = collectingRequestEntity.Id,
                CollectingRequestCode = collectingRequestEntity.CollectingRequestCode,
                ScrapImageUrl = collectingRequestEntity.ScrapImageUrl,
                SellerName = accountSellerName,
                // Date
                DayOfWeek = collectingRequestEntity.CollectingRequestDate.GetDayOfWeek(),
                CollectingRequestDate = collectingRequestEntity.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                FromTime = collectingRequestEntity.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = collectingRequestEntity.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                // Location
                CollectingAddress = locationEntity.Address,
                CollectingAddressName = locationEntity.AddressName,
                Latitude = locationEntity.Latitude,
                Longtitude = locationEntity.Longitude,
                IsBulky = collectingRequestEntity.IsBulky,
                Note = collectingRequestEntity.Note,
            };

            return BaseApiResponse.OK(responseEntity);
        }

        #endregion
    }
}
