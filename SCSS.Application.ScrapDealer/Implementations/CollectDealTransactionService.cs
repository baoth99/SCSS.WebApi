using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels;
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
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public partial class CollectDealTransactionService : BaseService, ICollectDealTransactionService
    {
        #region Repositories

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Constructor

        public CollectDealTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _accountRepository = unitOfWork.AccountRepository;
        }

        #endregion

        #region Auto Complete Collector Phone

        /// <summary>
        /// Automatics the complete collector phone.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> AutoCompleteCollectorPhone()
        {
            var collectorRole = UnitOfWork.RoleRepository.Get(x => x.Key == AccountRole.COLLECTOR).Id;

            var dataQuery = _accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(collectorRole) &&
                                                                        x.Status == AccountStatus.ACTIVE);
            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new AutoCompleteCollectorPhoneModel()
            {
                Id = x.Id,
                Phone = x.Phone
            });

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Transaction Info Review

        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="collectorId">The collector identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionInfoReview(Guid collectorId)
        {
            var collectorRole = UnitOfWork.RoleRepository.Get(x => x.Key == AccountRole.COLLECTOR).Id;
            var collectorInfo = _accountRepository.GetAsNoTracking(x => x.Id.Equals(collectorId) &&
                                                                        x.Status == AccountStatus.ACTIVE &&
                                                                        x.RoleId.Equals(collectorRole));

            if (collectorInfo == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var serviceFee = await TransactionServiceFeePercent(CacheRedisKey.CollectDealTransactionServiceFee);

            var dataResult = new TransactionInfoReviewViewModel()
            {
                CollectorId = collectorInfo.Id,
                CollectorName = collectorInfo.Name,
                CollectorPhone = collectorInfo.Phone,
                TransactionFeePercent = serviceFee
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        #region Get Transaction Scrap Categories

        /// <summary>
        /// Gets the scrap category transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionScrapCategories()
        {
            var dataQuery = _scrapCategoryRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                              x.Status == ScrapCategoryStatus.ACTIVE);
            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new TransactionScrapCategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Transaction Scrap Category Detail

        /// <summary>
        /// Gets the transaction scrap category detail.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionScrapCategoryDetail(Guid id)
        {
            if (!_scrapCategoryRepository.IsExisted(x => x.Id.Equals(id) && x.AccountId.Equals(UserAuthSession.UserSession.Id)))
            {
                return BaseApiResponse.NotFound();
            }

            var dataQuery = _scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(id) &&
                                                                                    x.Status == ScrapCategoryStatus.ACTIVE);

            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new TransactionScrapCategoryDetailViewModel()
            {
                Id = x.Id,
                Price = x.Price.ToLongValue(),
                Unit = x.Unit
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

    }
}
