using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.PersonalSellerLocationModel;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public class PersonalSellerLocationService : BaseService, IPersonalSellerLocationService
    {
        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The personal seller location repository
        /// </summary>
        private readonly IRepository<PersonalSellerLocation> _personalSellerLocationRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalSellerLocationService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public PersonalSellerLocationService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _personalSellerLocationRepository = unitOfWork.PersonalSellerLocationRepository;
        }

        #endregion

        #region Add Personal Location

        /// <summary>
        /// Adds the personal location.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> AddPersonalLocation(PersonalSellerLocationCreateModel model)
        {
            var location = new Location()
            {
                Address = model.Address,
                AddressName = model.AddressName,
                City = model.City,
                District = model.District,
                Latitude = model.Latitude,
                Longitude = model.Longtitude
            };

            var locationEntity = _locationRepository.Insert(location);

            var personalLocation = new PersonalSellerLocation()
            {
                LocationId = locationEntity.Id,
                PlaceId = model.PlaceId,
                PlaceName = model.PlaceName,
                SellerAccountId = UserAuthSession.UserSession.Id
            };

            _personalSellerLocationRepository.Insert(personalLocation);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Personal Locations

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetLocations()
        {
            var dataQuery = _personalSellerLocationRepository.GetManyAsNoTracking(x => x.SellerAccountId == UserAuthSession.UserSession.Id)
                                                             .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                                (x, y) => new
                                                                {
                                                                    x.Id,
                                                                    x.PlaceId,
                                                                    x.PlaceName,
                                                                    y.AddressName,
                                                                    y.Address,
                                                                    y.City,
                                                                    y.District,
                                                                    y.Latitude,
                                                                    y.Longitude,
                                                                    x.CreatedTime
                                                                }).OrderByDescending(x => x.CreatedTime);

            var totalRecord = await dataQuery.CountAsync();

            var dataRes = dataQuery.Select(x => new PersonalSellerLocationViewModel()
            {
                Id = x.Id,
                Address = x.Address,
                AddressName = x.AddressName,
                PlaceId = x.PlaceId,
                PlaceName = x.PlaceName,
                City = x.City,
                District = x.District,
                Latitude = x.Latitude,
                Longtitude = x.Longitude
            }).ToList();

            return BaseApiResponse.OK(dataRes, totalRecord);
        }

        #endregion

        #region Remove Personal Location

        /// <summary>
        /// Removes the location.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RemoveLocation(Guid id)
        {
            var personalLocation = _personalSellerLocationRepository.GetAsNoTracking(x => x.Id == id && x.SellerAccountId == UserAuthSession.UserSession.Id);

            if (personalLocation == null)
            {
                return BaseApiResponse.NotFound();
            }

            _personalSellerLocationRepository.Remove(personalLocation);

            _locationRepository.RemoveById(personalLocation.LocationId);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion
    }
}
