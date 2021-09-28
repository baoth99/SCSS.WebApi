using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Models.TransactionServiceFeeModels;
using SCSS.Data.Entities;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
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
            await _cacheService.SetCacheData(redisKey, entity.Percent.ToString());

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Transaction Service Fee

        /// <summary>
        /// Gets the transaction service fee.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionServiceFee()
        {
            var transactionFee = await _transactionServiceFeePercentRepository.GetManyAsNoTracking(x => x.IsActive).ToListAsync();

            var sellCollectTransFee = transactionFee.Where(x => x.TransactionType == TransactionType.SELL_COLLECT).Select(x => new TransactionServiceFeeViewModel()
            {
                TransactionType = x.TransactionType,
                Percent = x.Percent
            }).FirstOrDefault();

            var collectDealTransFee = transactionFee.Where(x => x.TransactionType == TransactionType.COLLECT_DEAL).Select(x => new TransactionServiceFeeViewModel()
            {
                TransactionType = x.TransactionType,
                Percent = x.Percent
            }).FirstOrDefault();

            var dataResult = new Tuple<TransactionServiceFeeViewModel, TransactionServiceFeeViewModel>(sellCollectTransFee, collectDealTransFee);

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
