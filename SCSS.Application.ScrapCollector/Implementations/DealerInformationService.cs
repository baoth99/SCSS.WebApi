using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.DealerInformationModels;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SCSS.FirebaseService.Interfaces;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class DealerInformationService : BaseService, IDealerInformationService
    {
        #region Repositories

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Service

        /// <summary>
        /// The map distance matrix service
        /// </summary>
        private readonly IMapDistanceMatrixService _mapDistanceMatrixService;

        #endregion

        #region Constructor        

        /// <summary>
        /// Initializes a new instance of the <see cref="DealerInformationService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="mapDistanceMatrixService">The map distance matrix service.</param>
        public DealerInformationService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                        IMapDistanceMatrixService mapDistanceMatrixService, IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _mapDistanceMatrixService = mapDistanceMatrixService;
        }

        #endregion

        #region Search Dealer Information

        /// <summary>
        /// Searches the dealer information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchDealerInfo(DealerInformationFilterModel model)
        {
            var dealerDataQuery = _dealerInformationRepository.GetAll().Join(_accountRepository.GetManyAsNoTracking(x => x.Status == AccountStatus.ACTIVE), x => x.DealerAccountId, y => y.Id,
                                                                                  (x, y) => new
                                                                                  {
                                                                                      x.Id,
                                                                                      x.DealerName,
                                                                                      x.IsActive,
                                                                                      x.LocationId,
                                                                                      x.DealerImageUrl,
                                                                                      x.OpenTime,
                                                                                      x.CloseTime
                                                                                  }) 
                                                                        .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                                                  (x, y) => new
                                                                                  {
                                                                                      DealerId = x.Id,
                                                                                      x.DealerName,
                                                                                      x.IsActive,
                                                                                      UnSignDealerName = x.DealerName.RemoveSignVietnameseString().ToLower(),
                                                                                      DealerAddress = y.Address,
                                                                                      UnSignDealerAddress = y.Address.RemoveSignVietnameseString().ToLower(),
                                                                                      y.Latitude,
                                                                                      y.Longitude,
                                                                                      x.DealerImageUrl,
                                                                                      x.OpenTime,
                                                                                      x.CloseTime,
                                                                                  })
                                                                         .Where(x => (ValidatorUtil.IsBlank(model.SearchWord) || 
                                                                                      x.UnSignDealerAddress.ToLower().Contains(model.SearchWord.ToLower()) ||
                                                                                      x.UnSignDealerName.ToLower().Contains(model.SearchWord.ToLower()))).ToList();


            if (!dealerDataQuery.Any())
            {
                return BaseApiResponse.OK(totalRecord: NumberConstant.Zero, resData: CollectionConstants.Empty<DealerInformationViewModel>());
            }

            // Get Destination List
            var destinationCoordinateRequest = dealerDataQuery.Select(x => new DestinationCoordinateModel()
            {
                Id = x.DealerId,
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

            var dealerData = destinationDistancesRes.Join(dealerDataQuery, x => x.DestinationId, y => y.DealerId,
                                                          (x, y) => new
                                                          {
                                                              y.DealerId,
                                                              y.DealerName,
                                                              y.IsActive,
                                                              y.DealerImageUrl,
                                                              y.OpenTime,
                                                              y.CloseTime,
                                                              y.DealerAddress,
                                                              y.Latitude,
                                                              y.Longitude,
                                                              x.DistanceText,
                                                              x.DistanceVal,
                                                          }).OrderBy(x => x.DistanceVal);
            var totalRecord = dealerData.Count();

            var dataResult = dealerData.Select(x => new DealerInformationViewModel()
            {
                DealerId =  x.DealerId,
                DealerName = x.DealerName,
                DealerAddress = x.DealerAddress,
                DealerImageUrl = x.DealerImageUrl,
                OpenTime = x.OpenTime.ToStringFormat(TimeSpanFormat.HH_MM),
                CloseTime = x.CloseTime.ToStringFormat(TimeSpanFormat.HH_MM),
                Latitude = x.Latitude,
                Longtitude = x.Longitude,
                Distance = x.DistanceVal,
                DistanceText = x.DistanceText,
                IsActive = x.IsActive,

            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Dealer Information Detail

        /// <summary>
        /// Gets the dealer information detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerInformationDetail(Guid id)
        {
            var dealerInformation = await _dealerInformationRepository.GetByIdAsync(id);

            if (dealerInformation == null)
            {
                return BaseApiResponse.NotFound();
            }

            var location = _locationRepository.GetById(dealerInformation.LocationId);

            var dataResult = new DealerInformationDetailViewModel()
            {
                DealerId = dealerInformation.Id,
                Rating = dealerInformation.Rating,
                DealerName = dealerInformation.DealerName,
                OpenTime = dealerInformation.OpenTime.ToStringFormat(TimeSpanFormat.HH_MM),
                CloseTime = dealerInformation.CloseTime.ToStringFormat(TimeSpanFormat.HH_MM),
                DealerPhone = dealerInformation.DealerPhone,
                DealerAddress = location.Address,
                DealerImageUrl = dealerInformation.DealerImageUrl,
                Latitude = location.Latitude,
                Longtitude = location.Longitude,
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

    }
}
