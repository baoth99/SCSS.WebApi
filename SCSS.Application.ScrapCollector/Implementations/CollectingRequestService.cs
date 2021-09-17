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
using SCSS.FirebaseService.Models;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class CollectingRequestService : BaseService, ICollectingRequestService
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
        /// The FCM service
        /// </summary>
        private readonly IFCMService _FCMService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapDistanceMatrixService">The map distance matrix service.</param>
        /// <param name="FCMService">The FCM service.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                        IMapDistanceMatrixService mapDistanceMatrixService, IFCMService FCMService) : base(unitOfWork, userAuthSession, logger)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _collectingRequestRejectionRepository = unitOfWork.CollectingRequestRejectionRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _notificationRepository = unitOfWork.NotificationRepository;
            _mapDistanceMatrixService = mapDistanceMatrixService;
            _FCMService = FCMService;
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

            if (filterDate.IsCompareDateTimeLessThan(DateTimeInDay.DATE_NOW))
            {
                filterDate = DateTimeInDay.DATE_NOW;
            }

            var collectingRequestdataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(filterDate) || x.CollectingRequestDate.IsCompareDateTimeEqual(filterDate)) &&
                                                                                                    x.Status == CollectingRequestStatus.PENDING && ValidatorUtil.IsNull(x.CollectorAccountId))                                                       
                                                        .Join(_locationRepository.GetAll(), x => x.LocationId, y => y.Id,
                                                                (x, y) => new
                                                                {
                                                                    CollectingRequestId = x.Id,
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
            if (!collectingRequestdataQuery.Any())
            {
                return BaseApiResponse.OK(CollectionConstants.Empty<CollectingRequestViewModel>());
            }

            var collectingRequestRejection = _collectingRequestRejectionRepository.GetManyAsNoTracking(x => x.CollectorId.Equals(UserAuthSession.UserSession.Id))
                                                                                  .Join(collectingRequestdataQuery, x => x.CollectingRequestId, y => y.CollectingRequestId,
                                                                                       (x, y) => new
                                                                                       {
                                                                                           y.CollectingRequestId,
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
            if (collectingRequestRejection.Any())
            {
                collectingRequestdataQuery = collectingRequestdataQuery.Except(collectingRequestRejection);
            }

            if (!collectingRequestdataQuery.Any())
            {
                return BaseApiResponse.OK(CollectionConstants.Empty<CollectingRequestViewModel>());
            }

            // Get Destination List
            var destinationCoordinateRequest = collectingRequestdataQuery.Select(x => new DestinationCoordinateModel()
            {
                Id = x.CollectingRequestId,
                DestinationLatitude = x.Latitude.Value,
                DestinationLongtitude = x.Longitude.Value
            }).ToList();

            var mapDistanceMatrixCoordinate = new DistanceMatrixCoordinateRequestModel()
            {
                OriginLatitude = model.GeocodeLatitude.Value,
                OriginLongtitude = model.GeocodeLongtitude.Value,
                DestinationItems = destinationCoordinateRequest,
                Vehicle = Vehicle.hd
            };

            // Call to Goong Map Service to get distance between collector location and collecting request location
            var destinationDistancesRes = await _mapDistanceMatrixService.GetDistanceFromOriginToMultiDestinationsCR(mapDistanceMatrixCoordinate);

            // Sort the distance list ascending 
            var destinationDistancesSort = destinationDistancesRes.OrderBy(x => x.DistanceVal).Take(model.ScreenSize);

            // Get Seller Role 
            var sellerRoleId = UnitOfWork.RoleRepository.Get(x => x.Key.Equals(AccountRole.SELLER)).Id;

            var collectingRequestData = destinationDistancesSort.Join(collectingRequestdataQuery, x => x.DestinationId, y => y.CollectingRequestId,
                                                   (x, y) => new
                                                   {
                                                       y.CollectingRequestId,
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
                                                        CollectingAddress = x.Address,
                                                        CollectingRequestAddressName = x.AddressName,
                                                        x.CollectingRequestDate,
                                                        Distance = x.DistanceVal,
                                                        FromTime = x.TimeFrom,
                                                        ToTime = x.TimeTo,
                                                        x.IsBulky,
                                                        x.Latitude,
                                                        x.Longitude,
                                                        SellerName = y.Name
                                                    }).AsQueryable();

            var totalRecord = await collectingRequestData.CountAsync();

            var resData = collectingRequestData.Select(x => new CollectingRequestViewModel()
            {
                Id = x.CollectingRequestId,
                CollectingAddress = x.CollectingAddress,
                CollectingAddressName = x.CollectingRequestAddressName,
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                DayOfWeek = x.CollectingRequestDate.GetDayOfWeek(),
                Distance = x.Distance,
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

        #region Receive the Collecting Request

        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReceiveCollectingRequest(Guid id)
        {
            var collectingRequestEntity = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(id) &&
                                                                                            x.Status == CollectingRequestStatus.PENDING &&
                                                                                            ValidatorUtil.IsNull(x.CollectorAccountId));
            if (collectingRequestEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            collectingRequestEntity.CollectorAccountId = UserAuthSession.UserSession.Id;
            collectingRequestEntity.Status = CollectingRequestStatus.APPROVED;

            _collectingRequestRepository.Update(collectingRequestEntity);

            if (!_collectingRequestRepository.IsExisted(x => x.Id.Equals(id) && x.Status == CollectingRequestStatus.PENDING && ValidatorUtil.IsNull(x.CollectorAccountId)))
            {
                return BaseApiResponse.NotFound();
            }

            await UnitOfWork.CommitAsync();

            await SendNotificationToSeller(collectingRequestEntity.SellerAccountId);

            return BaseApiResponse.OK();
        }

        /// <summary>
        /// Sends the notification to seller.
        /// </summary>
        /// <param name="sellerId">The seller identifier.</param>
        private async Task SendNotificationToSeller(Guid? sellerId)
        {
            var sellerAccount = _accountRepository.GetById(sellerId);

            // TODO:
            var notificationMessage = new NotificationRequestModel()
            {
                Title = "",
                Body = "",
                DeviceId = sellerAccount.DeviceId
            };
            await _FCMService.PushNotification(notificationMessage);

            var notificationEntity = new Notification()
            {
                Title = notificationMessage.Title,
                Body = notificationMessage.Body,
                // TODO: DataCustom
                AccountId = sellerId,
                IsRead = BooleanConstants.FALSE
            };

            _notificationRepository.Insert(notificationEntity);

            await UnitOfWork.CommitAsync();
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

        #region View Collecting Request Detail

        public async Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id)
        {
            return BaseApiResponse.OK();
        }


        #endregion
    }
}
