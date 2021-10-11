using SCSS.Application.Admin.Models.TransactionAwardAmountModels;
using SCSS.Data.Entities;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.Utilities.Extensions;
using System.Linq;
using System.Threading.Tasks;
using SCSS.AWSService.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace SCSS.Application.Admin.Implementations
{
    public partial class SystemConfigService 
    {
        #region Create Transaction Award Amount

        /// <summary>
        /// Creates the transaction award amount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ModifyTransactionAwardAmount(TransactionAwardAmountModifyModel model)
        {
            // De-Active all of old TransactionAward
            var oldAwardAmounts = _transactionAwardAmountRepository.GetManyAsNoTracking(x => x.TransactionType == model.TransactionType).ToList()
                                                                   .Select(c => 
                                                                   { 
                                                                       c.IsActive = BooleanConstants.FALSE; 
                                                                       return c; 
                                                                   }).ToList();
            if (oldAwardAmounts.Any())
            {
                _transactionAwardAmountRepository.UpdateRange(oldAwardAmounts);
            }

            // Create new TransactionAwardAmount
            var entity = new TransactionAwardAmount()
            {
                TransactionType = model.TransactionType,
                AppliedAmount = model.AppliedAmount,
                Amount = model.Amount,
                IsActive = BooleanConstants.TRUE
            };
            _transactionAwardAmountRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            // Modify Redis Cache
            var redisKey = model.TransactionType == TransactionType.SELL_COLLECT ? CacheRedisKey.SellCollectTransactionAwardAmount : CacheRedisKey.CollectDealTransactionAwardAmount;

            var cacheModel = new TransactionAwardAmountCacheViewModel()
            {
                AppliedAmount = entity.AppliedAmount.Value,
                Amount = entity.Amount.Value,
            };

            await _cacheService.SetStringCacheAsync(redisKey, cacheModel.ToJson());

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Transaction Award Amount

        /// <summary>
        /// Gets the transaction award amount.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionAwardAmount(int transactionType)
        {
            var transactionAward = await _transactionAwardAmountRepository.GetManyAsNoTracking(x => x.TransactionType == transactionType).ToListAsync();

            var histories = transactionAward.Where(x => !x.IsActive).Select(x => new TransactionAwardAmountHistoryViewModel()
            {
                AppliedAmount = x.AppliedAmount,
                Amount = x.Amount,
                DeActiveTime = x.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt)
            }).ToList();


            var transactionAwardPoint = transactionAward.Where(x => x.IsActive).Select(x => new TransactionAwardAmountViewModel()
            {
                AppliedAmount = x.AppliedAmount,
                Amount = x.Amount
            }).FirstOrDefault();

            transactionAwardPoint.Histories = histories;

            return BaseApiResponse.OK(transactionAwardPoint);
        }

        #endregion
    }
}
