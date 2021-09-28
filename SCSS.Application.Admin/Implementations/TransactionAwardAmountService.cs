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
                Amount = entity.AppliedAmount.Value,
            };

            await _cacheService.SetCacheData(redisKey, cacheModel.ToJson());

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Transaction Award Amount

        /// <summary>
        /// Gets the transaction award amount.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionAwardAmount()
        {
            var transactionAward = await _transactionAwardAmountRepository.GetManyAsNoTracking(x => x.IsActive).ToListAsync();

            var sellCollectTrans = transactionAward.Where(x => x.TransactionType == TransactionType.SELL_COLLECT).Select(x => new TransactionAwardAmountViewModel()
            {
                TransactionType = x.TransactionType,
                AppliedAmount = x.AppliedAmount,
                Amount = x.Amount
            }).FirstOrDefault();
            var collectDealTrans = transactionAward.Where(x => x.TransactionType == TransactionType.COLLECT_DEAL).Select(x => new TransactionAwardAmountViewModel()
            {
                TransactionType = x.TransactionType,
                AppliedAmount = x.AppliedAmount,
                Amount = x.Amount
            }).FirstOrDefault();

            var dataResult = new Tuple<TransactionAwardAmountViewModel, TransactionAwardAmountViewModel>(sellCollectTrans, collectDealTrans);

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
