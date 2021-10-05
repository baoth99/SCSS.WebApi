using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public partial class CollectDealTransactionService
    {
        #region Get Transaction Histories

        /// <summary>
        /// Gets the transaction histories.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionHistories(TransactionHistoryFilterModel model)
        {
            var dealerId = UserAuthSession.UserSession.Id;

            var dealerRoleId = _roleRepository.GetAsNoTracking(x => x.Key == AccountRole.DEALER).Id;
            var collectorRoleId = _roleRepository.GetAsNoTracking(x => x.Key == AccountRole.COLLECTOR).Id;

            var dataQuery = _collectDealTransactionRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.FromDate) || x.CreatedTime.Value.Date.CompareTo(model.FromDate.ToDateTime()) >= NumberConstant.Zero) &&
                                                                                       (ValidatorUtil.IsBlank(model.ToDate) || x.CreatedTime.Value.Date.CompareTo(model.ToDate.ToDateTime()) <= NumberConstant.Zero) &&
                                                                                       (model.FromTotal == NumberConstant.Zero || (x.Total - x.TransactionServiceFee) >= model.FromTotal) &&
                                                                                       (model.ToTotal == NumberConstant.Zero || (x.Total - x.TransactionServiceFee) <= model.ToTotal))
                                                             .Join(_accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(collectorRoleId)), x => x.CollectorAccountId, y => y.Id,
                                                                                       (x, y) => new
                                                                                       {
                                                                                           TransactionId = x.Id,
                                                                                           CollectorName = y.Name,
                                                                                           CollectorImage = y.ImageUrl,
                                                                                           TransactionDateTime = x.CreatedTime,
                                                                                           x.Total,
                                                                                           x.TransactionServiceFee
                                                                                       }).OrderByDescending(x => x.TransactionDateTime);
            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new CollectDealTransactionHistoryViewModel
            {
                Id = x.TransactionId,
                CollectorName = x.CollectorName,
                CollectorImage = x.CollectorImage,
                TransactionDateTime = x.TransactionDateTime,
                Total = x.Total - x.TransactionServiceFee
            }).ToList();


            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion


        #region Get Transaction History Detail

        /// <summary>
        /// Gets the transaction history detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionHistoryDetail(Guid id)
        {
            var transEntity = await  _collectDealTransactionRepository.GetByIdAsync(id);
            if (transEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var collectorInfo = _accountRepository.GetById(transEntity.CollectorAccountId);


            var transactionDetailItems = _collectDealTransactionDetailRepository.GetManyAsNoTracking(x => x.CollectDealTransactionId.Equals(transEntity.Id))
                                                                                .GroupJoin(_scrapCategoryDetailRepository.GetAllAsNoTracking(), x => x.DealerCategoryDetailId, y => y.Id,
                                                                                          (x, y) => new
                                                                                          {
                                                                                              x.Quantity,
                                                                                              x.PromotionId,
                                                                                              x.Total,
                                                                                              x.BonusAmount,
                                                                                              ScrapCategoryDetail = y
                                                                                          }).SelectMany(x => x.ScrapCategoryDetail.DefaultIfEmpty(), (x, y) => new
                                                                                          {
                                                                                              x.Quantity,
                                                                                              x.Total,
                                                                                              x.BonusAmount,
                                                                                              x.PromotionId,
                                                                                              y.ScrapCategoryId,
                                                                                              y.Unit
                                                                                          })
                                                                                .GroupJoin(_scrapCategoryRepository.GetAllAsNoTracking(), x => x.ScrapCategoryId, y => y.Id,
                                                                                          (x, y) => new
                                                                                          {
                                                                                              x.Quantity,
                                                                                              x.Total,
                                                                                              x.BonusAmount,
                                                                                              x.Unit,
                                                                                              x.PromotionId,
                                                                                              ScrapCategory = y
                                                                                          }).SelectMany(x => x.ScrapCategory.DefaultIfEmpty(), (x, y) => new
                                                                                          {
                                                                                              x.Quantity,
                                                                                              ScrapCategoryName = y.Name,
                                                                                              x.BonusAmount,
                                                                                              x.Unit,
                                                                                              x.Total,
                                                                                              x.PromotionId
                                                                                          })
                                                                                .Select(x => new TransactionHistoryScrapCategoryViewModel()
                                                                                {
                                                                                    ScrapCategoryName = x.ScrapCategoryName,
                                                                                    BonusAmount = x.BonusAmount.ToLongValue(),
                                                                                    Quantity = x.Quantity,
                                                                                    Total = x.Total,
                                                                                    IsBonus = !ValidatorUtil.IsBlank(x.PromotionId)
                                                                                }).ToList();


            var totalBonus = transactionDetailItems.Select(x => x.BonusAmount).Sum();

            var dataResult = new TransactionHistoryDetailViewModel()
            {
                CollectorName = collectorInfo.Name,
                TransactionCode = transEntity.TransactionCode,
                TransactionDate = transEntity.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                TransactionTime = transEntity.CreatedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM),
                Total = transEntity.Total,
                TotalBonus = totalBonus,
                TransactionFee = transEntity.TransactionServiceFee,
                ItemDetail = transactionDetailItems
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
