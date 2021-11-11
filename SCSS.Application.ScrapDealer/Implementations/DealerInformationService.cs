using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.DealerInformationModels;
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

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class DealerInformationService : BaseService, IDealerInformationService
    {
        #region Repositories

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DealerInformationService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public DealerInformationService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,  IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _locationRepository = unitOfWork.LocationRepository;
        }

        #endregion

        #region Get Dealer Information Detail

        /// <summary>
        /// Gets the dealer information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerInformation()
        {
            // Get Dealer Account Id from UserAuthSession
            var dealerAccountId = UserAuthSession.UserSession.Id;

            // Get Dealer Information from dealerAccountId
            var dealerInformation = await _accountRepository.GetManyAsNoTracking(x => x.Id.Equals(dealerAccountId))
                                                  .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.Id, y => y.DealerAccountId,
                                                        (x, y) => y).FirstOrDefaultAsync();
          
            // Check Dealer is not existed
            if (dealerInformation == null)
            {
                return BaseApiResponse.NotFound();
            }
            // Get Dealer Location
            var dealerLocation = _locationRepository.GetById(dealerInformation.LocationId);

            var resData = new DealerInformationDetailViewModel()
            {
                Id = dealerInformation.Id,
                DealerName = dealerInformation.DealerName,
                DealerPhone = dealerInformation.DealerPhone,
                DealerAddress = dealerLocation.Address,
                DealerLatitude = dealerLocation.Latitude,
                DealerLongtitude = dealerLocation.Longitude,
                OpenTime = dealerInformation.OpenTime.ToStringFormat(TimeSpanFormat.HH_MM),
                CloseTime = dealerInformation.OpenTime.ToStringFormat(TimeSpanFormat.HH_MM),
                DealerImageUrl = dealerInformation.DealerImageUrl,
                IsActive = dealerInformation.IsActive,
            };

            return BaseApiResponse.OK(resData);
        }

        #endregion

        #region Get Dealer Branchs Information

        /// <summary>
        /// Gets the dealer branchs information.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerBranchsInfo()
        {
            // Check Account is Leader Dealer
            if (UserAuthSession.UserSession.Role == AccountRole.DEALER_MEMBER)
            {
                return BaseApiResponse.Forbidden();
            }

            // Get Dealer Leader Account Id from UserAuthSession
            var dealerLeaderAccId = UserAuthSession.UserSession.Id;

            var dataQuery = _accountRepository.GetManyAsNoTracking(x => x.ManagedBy.Equals(dealerLeaderAccId) && x.Status == AccountStatus.ACTIVE)
                                              .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.Id, y => y.DealerAccountId,
                                                    (x, y) => new
                                                    {
                                                        y.Id,
                                                        y.DealerName,
                                                        y.DealerImageUrl,
                                                        y.LocationId
                                                    })
                                              .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                    (x, y) => new
                                                    {
                                                        DealerBranchId = x.Id,
                                                        DealerBranchName = x.DealerName,
                                                        DealerBranchImageUrl = x.DealerImageUrl,
                                                        DealerBranchAddress = y.Address
                                                    }).OrderBy(x => x.DealerBranchName);

            var totalRecord = await dataQuery.CountAsync();

            var resData = dataQuery.Select(x => new DealerBranchInformationViewModel()
            {
                Id = x.DealerBranchId,
                DealerBranchName = x.DealerBranchName,
                DealerBranchAddress = x.DealerBranchAddress,
                DealerBranchImageUrl = x.DealerBranchImageUrl
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: resData);
        }

        #endregion Get Dealer Branchs Information

        #region Get Dealer Branch Information Detail

        /// <summary>
        /// Gets the dealer branch information detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerBranchInfoDetail(Guid id)
        {
            // Check Account is Leader Dealer
            if (UserAuthSession.UserSession.Role == AccountRole.DEALER_MEMBER)
            {
                return BaseApiResponse.Forbidden();
            }

            // Get Dealer Account Id from UserAuthSession
            var dealerAccountId = UserAuthSession.UserSession.Id;

            var dealerBranchinfo = await _dealerInformationRepository.GetManyAsNoTracking(x => x.Id.Equals(id))
                                                                    .Join(_accountRepository.GetManyAsNoTracking(x => x.ManagedBy.Equals(dealerAccountId)),
                                                                        x => x.DealerAccountId, y => y.Id, (x, y) => new
                                                                        {
                                                                            DealerInfo = x,
                                                                            y.Name,
                                                                            y.Phone
                                                                        }).FirstOrDefaultAsync();
            // Check Dealer Branch not existed
            if (dealerBranchinfo == null)
            {
                return BaseApiResponse.NotFound();
            }

            var dealerLocation = _locationRepository.GetById(dealerBranchinfo.DealerInfo.LocationId);

            var resData = new DealerBranchInformationDetailViewModel()
            {
                Id = dealerBranchinfo.DealerInfo.Id,
                DealerBranchName = dealerBranchinfo.DealerInfo.DealerName,
                DealerBranchPhone = dealerBranchinfo.DealerInfo.DealerPhone,
                DealerBranchAddress = dealerLocation.Address,
                DealerBranchOpenTime = dealerBranchinfo.DealerInfo.OpenTime.ToStringFormat(TimeSpanFormat.HH_MM),
                DealerBranchCloseTime = dealerBranchinfo.DealerInfo.CloseTime.ToStringFormat(TimeSpanFormat.HH_MM),
                DealerBranchImageUrl = dealerBranchinfo.DealerInfo.DealerImageUrl,
                DealerAccountBranch = new DealerAccountBranchInformationViewModel()
                {
                    Name = dealerBranchinfo.Name,
                    Phone = dealerBranchinfo.Phone
                }
            };
            return BaseApiResponse.OK(resData);
        }

        #endregion Get Dealer Branch Information Detail

        #region Change Dealer Status

        /// <summary>
        /// Changes the dealer status.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ChangeDealerStatus(bool status)
        {
            // Get Dealer Account Id from UserAuthSession
            var dealerAccountId = UserAuthSession.UserSession.Id;

            // Get Dealer Information from dealerAccountId
            var dealerInformation = await _accountRepository.GetManyAsNoTracking(x => x.Id.Equals(dealerAccountId))
                                                  .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.Id, y => y.DealerAccountId,
                                                        (x, y) => y).FirstOrDefaultAsync();

            // Check Dealer is not existed
            if (dealerInformation == null)
            {
                return BaseApiResponse.NotFound();
            }

            // Change Status
            dealerInformation.IsActive = status;

            _dealerInformationRepository.Update(dealerInformation);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Dealer Branchs

        /// <summary>
        /// Gets the dealer branch infos.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerBranchs()
        {
            // Check Account is Leader Dealer
            if (UserAuthSession.UserSession.Role == AccountRole.DEALER_MEMBER)
            {
                return BaseApiResponse.Forbidden();
            }

            var dealerAccountId = UserAuthSession.UserSession.Id;

            var dataQuery = _accountRepository.GetManyAsNoTracking(x => x.ManagedBy.Equals(dealerAccountId))
                                                 .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.Id, y => y.DealerAccountId,
                                                        (x, y) => new
                                                        {
                                                            x.Id,
                                                            x.RoleId,
                                                            y.DealerName
                                                        }).OrderByDescending(x => x.DealerName);


            var dealer = _dealerInformationRepository.GetAsNoTracking(x => x.DealerAccountId.Equals(dealerAccountId));

            var dataResult = await dataQuery.Select(x => new DealerBranchViewModel()
            {
                DealerAccountId = x.Id,
                DealerName = x.DealerName
            }).ToListAsync();

            dataResult.Insert(NumberConstant.Zero, new DealerBranchViewModel()
            {
                DealerAccountId = dealerAccountId,
                DealerName = dealer.DealerName + " *"
            });

            var totalRecord = dataResult.Count();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

    }
}
