using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.DashboardModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
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

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public DashboardService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
        }

        #endregion Constructor

        #region Get The Nearest Collecting Request

        /// <summary>
        /// Gets the nearest collecting request.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetNearestCollectingRequest()
        {
            var sellerId = UserAuthSession.UserSession.Id;

            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => x.SellerAccountId.Equals(sellerId) &&
                                                                                  x.Status == CollectingRequestStatus.APPROVED &&
                                                                                  x.ApprovedTime != null)
                                                        .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                                (x, y) => new
                                                                {
                                                                    CollectingRequestId = x.Id,
                                                                    x.CollectingRequestCode,
                                                                    x.CollectingRequestDate,
                                                                    x.ApprovedTime,
                                                                    x.TimeFrom,
                                                                    x.TimeTo,
                                                                    x.IsBulky,
                                                                    x.Status,
                                                                    y.Address,
                                                                    y.AddressName,
                                                                });

            var nearestCollectingRequest = await dataQuery.OrderByDescending(x => x.ApprovedTime)
                                                          .Select(x => new CollectingRequestDashboadViewModel()
                                                          {
                                                              CollectingRequestId = x.CollectingRequestId,
                                                              CollectingRequestCode = x.CollectingRequestCode,
                                                              CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DDD_DD_MM_yyyy),
                                                              FromTime = x.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                                                              ToTime = x.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                                                              Address = x.Address,
                                                              AddressName = x.AddressName,
                                                              IsBulky = x.IsBulky,
                                                              Status = x.Status
                                                          }).FirstOrDefaultAsync();


            return BaseApiResponse.OK(nearestCollectingRequest);
        }

        #endregion Get The Nearest Collecting Request

    }
}
