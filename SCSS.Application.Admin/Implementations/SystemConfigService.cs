using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.SystemConfigModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public partial class SystemConfigService : BaseService, ISystemConfigService
    {
        #region Repositories

        /// <summary>
        /// The transaction award amount repository
        /// </summary>
        private readonly IRepository<TransactionAwardAmount> _transactionAwardAmountRepository;

        /// <summary>
        /// The transaction service fee percent repository
        /// </summary>
        private readonly IRepository<TransactionServiceFeePercent> _transactionServiceFeePercentRepository;

        /// <summary>
        /// The collecting request configuration repository
        /// </summary>
        private readonly IRepository<CollectingRequestConfig> _collectingRequestConfigRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        #endregion

        #region Services

        /// <summary>
        /// The cache service
        /// </summary>
        private readonly IStringCacheService _cacheService;

        #endregion

        #region Constructor

        public SystemConfigService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger)
        {
            _transactionAwardAmountRepository = unitOfWork.TransactionAwardAmountRepository;
            _transactionServiceFeePercentRepository = unitOfWork.TransactionServiceFeePercentRepository;
            _collectingRequestConfigRepository = unitOfWork.CollectingRequestConfigRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _cacheService = cacheService;
        }

        #endregion

        #region Modify System Configuration

        /// <summary>
        /// Settings the system configuration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ModifySystemConfig(SystemConfigModifyModel model)
        {
            var oldConfigs = _collectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).ToList()
                                                                                   .Select(x =>
                                                                                   {
                                                                                       x.IsActive = BooleanConstants.FALSE;
                                                                                       return x;
                                                                                   }).ToList();
            if (oldConfigs.Any())
            {
                _collectingRequestConfigRepository.UpdateRange(oldConfigs);
            }

            var newConfig = new CollectingRequestConfig()
            {
                IsActive = BooleanConstants.TRUE,
                ReceiveQuantity = model.ReceiveQuantity,
                RequestQuantity = model.RequestQuantity,
                MaxNumberOfRequestDays = model.MaxNumberOfRequestDays,
                OperatingTimeFrom = model.OperatingTimeFrom.ToTimeSpan(),
                OperatingTimeTo = model.OperatingTimeTo.ToTimeSpan(),
                CancelTimeRange = model.CancelTimeRange,
                TimeRangeRequestNow = model.TimeRangeRequestNow,
                FeedbackDealine = model.FeedbackDeadline,
            };

            var insertedEntity = _collectingRequestConfigRepository.Insert(newConfig);

            await UnitOfWork.CommitAsync();

            // Modify Redis Cache

            var cacheModel = new
            {
                model.ReceiveQuantity,
                model.RequestQuantity,
                model.MaxNumberOfRequestDays,
                model.CancelTimeRange,
                model.TimeRangeRequestNow,
                model.FeedbackDeadline
            };

            var dic = CommonUtils.ObjToDictionary(cacheModel).ToDictionary(x => x.Key.ToEnum<CacheRedisKey>(), y => y.Value);

            await _cacheService.SetStringCachesAsync(dic);

            var operatingRangeTime = new OperatingRangeTimeCache()
            {
                FromTime = model.OperatingTimeFrom.ToTimeSpan(),
                ToTime = model.OperatingTimeTo.ToTimeSpan(),
            };

            await _cacheService.SetStringCacheAsync(CacheRedisKey.OperatingTimeRange, operatingRangeTime.ToJson());

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get System Config Information

        /// <summary>
        /// Gets the system configuration information.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetSystemConfigInfo()
        {
            var configs =  _collectingRequestConfigRepository.GetAllAsNoTracking();

            var configIsUsing = await configs.Where(x => x.IsActive).FirstOrDefaultAsync();

            var configHistories = configs.Where(x => !x.IsActive).Join(_accountRepository.GetAllAsNoTracking(), x => x.UpdatedBy, y => y.Id,
                                                                        (x, y) => new
                                                                        {
                                                                            x.RequestQuantity,
                                                                            x.ReceiveQuantity,
                                                                            x.MaxNumberOfRequestDays,
                                                                            x.UpdatedTime,
                                                                            x.CreatedTime,
                                                                            x.OperatingTimeFrom,
                                                                            x.OperatingTimeTo,
                                                                            y.Name,
                                                                            x.CancelTimeRange,
                                                                            x.TimeRangeRequestNow,
                                                                            x.FeedbackDealine
                                                                        })
                                                                  .OrderByDescending(x => x.CreatedTime)
                                                                  .Select(x => new SystemConfigHistoryViewModel()
                                                                  {
                                                                      RequestQuantity = x.RequestQuantity,
                                                                      ReceiveQuantity = x.ReceiveQuantity,
                                                                      MaxNumberOfRequestDays = x.MaxNumberOfRequestDays,
                                                                      OperatingTimeFrom = x.OperatingTimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                                                                      OperatingTimeTo = x.OperatingTimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                                                                      DeActiveTime = x.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                                                                      DeActiveBy = x.Name,
                                                                      CancelTimeRange = x.CancelTimeRange,
                                                                      FeedbackDeadline = x.FeedbackDealine,
                                                                      TimeRangeRequestNow = x.TimeRangeRequestNow
                                                                  }).ToList();
            var dataResult = new SystemConfigViewModel()
            {
                Id = configIsUsing.Id,
                RequestQuantity = configIsUsing.RequestQuantity,
                ReceiveQuantity = configIsUsing.ReceiveQuantity,
                MaxNumberOfRequestDays = configIsUsing.MaxNumberOfRequestDays,
                CancelTimeRange = configIsUsing.CancelTimeRange,
                TimeRangeRequestNow = configIsUsing.TimeRangeRequestNow,
                FeedbackDeadline = configIsUsing.FeedbackDealine,
                ActiveTime = configIsUsing.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                OperatingTimeFrom = configIsUsing.OperatingTimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                OperatingTimeTo = configIsUsing.OperatingTimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                Histories = configHistories
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
