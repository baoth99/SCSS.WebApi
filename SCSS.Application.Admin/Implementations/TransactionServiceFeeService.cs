using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Models.TransactionServiceFeeModels;
using SCSS.Data.Entities;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.ResponseModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public partial class SystemConfigService
    {
        #region Create Transaction Service Fee

        /// <summary>
        /// Modify the transaction service fee.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ModifyTransactionServiceFee(TransactionServiceFeeModifyModel model)
        {
            var oldTransactionServiceFees = _transactionServiceFeePercentRepository.GetManyAsNoTracking(x => x.TransactionType == model.TransactionType).ToList()
                                                                                   .Select(x =>
                                                                                   {
                                                                                       x.IsActive = BooleanConstants.FALSE;
                                                                                       return x;
                                                                                   }).ToList();
            if (oldTransactionServiceFees.Any())
            {
                _transactionServiceFeePercentRepository.UpdateRange(oldTransactionServiceFees);
            }

            var entity = new TransactionServiceFeePercent()
            {
                TransactionType = model.TransactionType,
                Percent = model.Percent,
                IsActive = BooleanConstants.TRUE
            };

            _transactionServiceFeePercentRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            // Modify Redis Cache
            var redisKey = model.TransactionType == TransactionType.SELL_COLLECT ? CacheRedisKey.SellCollectTransactionServiceFee : CacheRedisKey.CollectDealTransactionServiceFee;
            await _cacheService.SetStringCacheAsync(redisKey, entity.Percent.ToString());

            return BaseApiResponse.OK();
        }

        #endregion


        #region Get Transaction Service Fee

        /// <summary>
        /// Gets the transaction service fee.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionServiceFee(int transactionType)
        {
            var transactionFee = await _transactionServiceFeePercentRepository.GetManyAsNoTracking(x => x.TransactionType == transactionType).ToListAsync();


            var histories = transactionFee.Where(x => !x.IsActive).Select(x => new TransactionServiceFeeHistoryViewModel()
            {
                Percent = x.Percent,
                DeActiveTime = x.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt)
            }).ToList();

            var sellCollectTransFee = transactionFee.Where(x => x.IsActive).Select(x => new TransactionServiceFeeViewModel()
            {
                Percent = x.Percent
            }).FirstOrDefault();

            sellCollectTransFee.Histories = histories;

            return BaseApiResponse.OK(sellCollectTransFee);
        }

        #endregion
    }
}
