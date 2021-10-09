using Microsoft.EntityFrameworkCore;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller
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
        public BaseService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService)
        {
            UnitOfWork = unitOfWork;
            UserAuthSession = userAuthSession;
            Logger = logger;
            CacheService = cacheService;
        }

        #endregion

        #region Get Max Number of Collecting Request that seller can request in day

        /// <summary>
        /// Gets the maximum number collecting request seller request.
        /// </summary>
        /// <returns></returns>
        public async Task<int> MaxNumberCollectingRequestSellerRequest()
        {
            var quantity = await CacheService.GetStringCacheAsync(CacheRedisKey.RequestQuantity);

            if (ValidatorUtil.IsBlank(quantity))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.One;
                }
                var requestQuantity = entity.RequestQuantity;

                await CacheService.SetStringCacheAsync(CacheRedisKey.RequestQuantity, requestQuantity.ToString());

                return requestQuantity;
            }
            return quantity.ToInt();
        }

        #endregion

        #region Get Max Number of Days that seller request in advance

        /// <summary>
        /// Gets the maximum number days seller request advance.
        /// </summary>
        /// <returns></returns>
        public async Task<int> MaxNumberDaysSellerRequestAdvance()
        {
            var quantity = await CacheService.GetStringCacheAsync(CacheRedisKey.MaxNumberOfRequestDays);
            if (ValidatorUtil.IsBlank(quantity))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.One;
                }
                var maxNumberOfRequestDays = entity.MaxNumberOfRequestDays;

                await CacheService.SetStringCacheAsync(CacheRedisKey.MaxNumberOfRequestDays, maxNumberOfRequestDays.ToString());

                return maxNumberOfRequestDays;
            }
            return quantity.ToInt();
        }

        #endregion

        #region Get Operating Range Time 

        /// <summary>
        /// Operatings the range time.
        /// </summary>
        /// <returns></returns>
        public async Task<Tuple<TimeSpan?, TimeSpan?>> OperatingTimeRange()
        {
            var operatingRangeTimeVal = await CacheService.GetStringCacheAsync(CacheRedisKey.OperatingTimeRange);
            
            if (ValidatorUtil.IsBlank(operatingRangeTimeVal))
            {
                var systemConfig = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();

                if (systemConfig == null)
                {
                    return null;
                }

                var operatingRangeTimeCache = new OperatingRangeTimeCache()
                {
                    FromTime = systemConfig.OperatingTimeFrom,
                    ToTime = systemConfig.OperatingTimeTo
                };

                await CacheService.SetStringCacheAsync(CacheRedisKey.OperatingTimeRange, operatingRangeTimeCache.ToJson());

                return new Tuple<TimeSpan?, TimeSpan?>(operatingRangeTimeCache.FromTime, operatingRangeTimeCache.ToTime);
            }

            var operatingRangeTime = operatingRangeTimeVal.ToMapperObject<OperatingRangeTimeCache>();

            var rangeTime = new Tuple<TimeSpan?, TimeSpan?>(operatingRangeTime.FromTime, operatingRangeTime.ToTime);
            return rangeTime;
        }

        #endregion

        #region Get Cancel Time Range

        /// <summary>
        /// Cancels the time range.
        /// </summary>
        /// <returns></returns>
        public async Task<int> CancelTimeRange()
        {
            var count = await CacheService.GetStringCacheAsync(CacheRedisKey.CancelRangeTime);
            if (ValidatorUtil.IsBlank(count))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.Ten;
                }
                var cancelTimeRange = entity.CancelTimeRange;

                await CacheService.SetStringCacheAsync(CacheRedisKey.CancelRangeTime, cancelTimeRange.ToString());

                return cancelTimeRange;
            }
            return count.ToInt();
        }

        #endregion

        #region Get Time Range Request Now

        /// <summary>
        /// Times the name of the range request.
        /// </summary>
        /// <returns></returns>
        public async Task<int> TimeRangeRequestNow()
        {
            var timeRange = await CacheService.GetStringCacheAsync(CacheRedisKey.TimeRangeRequestNow);
            if (ValidatorUtil.IsBlank(timeRange))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.Ten;
                }
                var timeRangeRequest = entity.TimeRangeRequestNow;

                await CacheService.SetStringCacheAsync(CacheRedisKey.TimeRangeRequestNow, timeRangeRequest.ToString());

                return timeRangeRequest;
            }
            return timeRange.ToInt();
        }

        #endregion

        #region Get Feedback Deadline

        /// <summary>
        /// Feedbacks the deadline.
        /// </summary>
        /// <returns></returns>
        public async Task<int> FeedbackDeadline()
        {
            var feedbackDeadline = await CacheService.GetStringCacheAsync(CacheRedisKey.FeedbackDeadline);
            if (ValidatorUtil.IsBlank(feedbackDeadline))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.Five;
                }
                var deadline = entity.FeedbackDealine;

                await CacheService.SetStringCacheAsync(CacheRedisKey.FeedbackDeadline, deadline.ToString());

                return deadline;
            }
            return feedbackDeadline.ToInt();
        }

        #endregion
    }
}
