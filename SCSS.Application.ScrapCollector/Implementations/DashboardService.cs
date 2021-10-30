using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.DashboardModels;
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
using SCSS.Utilities.ResponseModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class DashboardService : BaseService, IDashboardService
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

        #endregion

        #region Services

        /// <summary>
        /// The map distance matrix service
        /// </summary>
        private readonly IMapDistanceMatrixService _mapDistanceMatrixService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="mapDistanceMatrixService">The map distance matrix service.</param>
        public DashboardService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, 
                                IStringCacheService cacheService, IMapDistanceMatrixService mapDistanceMatrixService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _complaintRepository = unitOfWork.ComplaintRepository;
            _mapDistanceMatrixService = mapDistanceMatrixService;
        }

        #endregion

        #region Get The First Approved Collecting Request

        /// <summary>
        /// Gets the first approved collecting request.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetFirstApprovedCollectingRequest(DashboardCollectingRequestFilterModel model)
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
                                                                                   x.RequestType,
                                                                                   y.Address,
                                                                                   y.AddressName,
                                                                                   y.Latitude,
                                                                                   y.Longitude,
                                                                               })
                                                                  .Join(_accountRepository.GetAllAsNoTracking(), x => x.SellerAccountId, y => y.Id,
                                                                              (x, y) => new
                                                                              {
                                                                                  x.CollectingRequestId,
                                                                                  x.CollectingRequestCode,
                                                                                  x.CollectingRequestDate,
                                                                                  x.TimeFrom,
                                                                                  x.TimeTo,
                                                                                  x.IsBulky,
                                                                                  x.RequestType,
                                                                                  x.Address,
                                                                                  x.AddressName,
                                                                                  x.Latitude,
                                                                                  x.Longitude,
                                                                                  SellerName = y.Name,
                                                                              });
            if (!receivingDataQuery.Any())
            {
                return BaseApiResponse.OK();
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

            var receivedData = destinationDistancesRes.Join(receivingDataQuery, x => x.DestinationId, y => y.CollectingRequestId,
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
                                                                 y.RequestType,
                                                                 x.DistanceVal,
                                                                 x.DistanceText,
                                                                 x.DurationTimeVal,
                                                                 x.DurationTimeText,
                                                             });

            var currentRequest = receivedData.Where(x => x.RequestType == CollectingRequestType.CURRENT_REQUEST).OrderBy(x => x.DistanceVal);
            var appointment = receivedData.Where(x => x.RequestType == CollectingRequestType.MAKE_AN_APPOINTMENT).OrderBy(x => x.CollectingRequestDate);

            var result = currentRequest.Concat(appointment).Select(x => new DashboardCollectingRequestRecevingViewModel()
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
                DurationTimeText = x.DurationTimeText,
                DurationTimeVal = x.DurationTimeVal,
                RequestType = x.RequestType
            }).FirstOrDefault();

            return BaseApiResponse.OK(result);
        }

        #endregion

    }
}
