using Microsoft.EntityFrameworkCore;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer
{
    public class BaseService
    {
        /// <summary>
        /// Gets the unit of work.
        /// </summary>
        /// <value>
        /// The unit of work.
        /// </value>
        protected IUnitOfWork UnitOfWork { get; private set; }

        /// <summary>
        /// Gets the user authentication session.
        /// </summary>
        /// <value>
        /// The user authentication session.
        /// </value>
        protected IAuthSession UserAuthSession { get; private set; }

        /// <summary>
        /// Gets the logger.
        /// </summary>
        /// <value>
        /// The logger.
        /// </value>
        protected ILoggerService Logger { get; private set; }

        /// <summary>
        /// Gets the cache service.
        /// </summary>
        /// <value>
        /// The cache service.
        /// </value>
        protected IStringCacheService CacheService { get; private set; }


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        /// <param name="cacheService">The cache service.</param>
        public BaseService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService)
        {
            UnitOfWork = unitOfWork;
            UserAuthSession = userAuthSession;
            Logger = logger;
            CacheService = cacheService;
        }

        #endregion

        #region Get Transaction Service Fee

        /// <summary>
        /// Transactions the service fee percent.
        /// </summary>
        /// <returns></returns>
        public async Task<float> TransactionServiceFeePercent(CacheRedisKey redisKey)
        {
            if (!CollectionConstants.TransactionServiceFees.Contains(redisKey))
            {
                throw new ArgumentException("CacheRedisKey is not correct", nameof(redisKey));
            }

            var percentRes = await CacheService.GetStringCacheAsync(redisKey);

            if (percentRes == null)
            {
                var transType = redisKey == CacheRedisKey.SellCollectTransactionServiceFee ? TransactionType.SELL_COLLECT : TransactionType.COLLECT_DEAL;

                var entity = await UnitOfWork.TransactionServiceFeePercentRepository.GetManyAsNoTracking(x => x.TransactionType == transType &&
                                                                                                  x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.Zero;
                }
                var percent = entity.Percent.Value;
                await CacheService.SetStringCacheAsync(redisKey, percent.ToString());

                return percent;
            }
            return percentRes.ToFloat();
        }

        #endregion

        #region Get Transaction Award Point

        /// <summary>
        /// Transactions the award amount.
        /// </summary>
        /// <param name="redisKey">The redis key.</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">CacheRedisKey is not correct - redisKey</exception>
        public async Task<TransactionAwardAmountCacheViewModel> TransactionAwardAmount(CacheRedisKey redisKey)
        {
            if (!CollectionConstants.TransactionAwardAmounts.Contains(redisKey))
            {
                throw new ArgumentException("CacheRedisKey is not correct", nameof(redisKey));
            }

            var transAwardAmount = await CacheService.GetStringCacheAsync(redisKey);

            if (transAwardAmount == null)
            {
                var transType = redisKey == CacheRedisKey.SellCollectTransactionAwardAmount ? TransactionType.SELL_COLLECT : TransactionType.COLLECT_DEAL;

                var entity = await UnitOfWork.TransactionAwardAmountRepository.GetManyAsNoTracking(x => x.TransactionType == transType &&
                                                                                                  x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return new TransactionAwardAmountCacheViewModel()
                    {
                        Amount = NumberConstant.Zero,
                        AppliedAmount = NumberConstant.Zero
                    };
                }
                var cache = new TransactionAwardAmountCacheViewModel()
                {
                    Amount = entity.Amount.Value,
                    AppliedAmount = entity.AppliedAmount.Value
                };
                await CacheService.SetStringCacheAsync(redisKey, cache.ToJson());

                return cache;
            }

            return transAwardAmount.ToMapperObject<TransactionAwardAmountCacheViewModel>();
        }

        #endregion
    }
}
