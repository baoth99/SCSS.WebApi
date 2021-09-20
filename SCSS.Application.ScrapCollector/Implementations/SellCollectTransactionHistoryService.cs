using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public partial class SellCollectTransactionService
    {
        #region Get Collecting Transaction Histories

        /// <summary>
        /// Gets the collecting transaction histories.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingTransactionHistories()
        {
            var collectorId = UserAuthSession.UserSession.Id;

            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => CollectionConstants.CollectingRequestHistory.Contains(x.Status.Value) &&
                                                                                  x.CollectorAccountId.Equals(collectorId))
                                                        .GroupJoin(_sellCollectTransactionRepository.GetAllAsNoTracking(), x => x.Id, y => y.CollectingRequestId,
                                                                             (x, y) => new
                                                                             {
                                                                                 CollectingRequestId = x.Id,
                                                                                 x.CollectingRequestCode,
                                                                                 CollectingUpdateTime = x.UpdatedTime,
                                                                                 x.Status,
                                                                                 Transaction = y
                                                                             })
                                                        .SelectMany(x => x.Transaction.DefaultIfEmpty(), 
                                                                             (x, y) => new
                                                                             {
                                                                                 x.CollectingRequestId,
                                                                                 x.CollectingRequestCode,
                                                                                 x.Status,
                                                                                 y.Total,
                                                                                 y.TransactionServiceFee,
                                                                                 TransactionDate = y.CreatedTime,
                                                                                 x.CollectingUpdateTime
                                                                             });

            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new SellCollectTransactionHistoryViewModel()
            {
                CollectingRequestId = x.CollectingRequestId,
                CollectingRequestCode = x.CollectingRequestCode,
                Status = x.Status,
                Date = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value, x.TransactionDate, x.CollectingUpdateTime).ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                DayOfWeek = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value,x.TransactionDate, x.CollectingUpdateTime).GetDayOfWeek(),
                Time = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value, x.TransactionDate, x.CollectingUpdateTime).Value.TimeOfDay.ToString(TimeSpanFormat.HH_MM),
                Total = x.Total
            });

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Collecting Transaction History Detail

        /// <summary>
        /// Gets the collecting transaction history detail.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingTransactionDetailHistory(Guid collectingRequestId)
        {
            var collectingRequest = await _collectingRequestRepository.GetByIdAsync(collectingRequestId);

            if (collectingRequest == null)
            {
                return BaseApiResponse.NotFound();
            }

            var sellerName = _accountRepository.GetById(collectingRequest.SellerAccountId).Name;

            var transaction = await _sellCollectTransactionRepository.GetAsync(x => x.CollectingRequestId.Equals(collectingRequest.Id));

            if (collectingRequest.Status == CollectingRequestStatus.COMPLETED && transaction != null)
            {
                var transactionDetailItems = _sellCollectTransactionDetailRepository.GetManyAsNoTracking(x => x.SellCollectTransactionId.Equals(transaction.Id))
                                                                               .GroupJoin(_scrapCategoryDetailRepository.GetAllAsNoTracking(), x => x.CollectorCategoryDetailId, y => y.Id,
                                                                                         (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             ScrapCategoryDetail = y
                                                                                         }).SelectMany(x => x.ScrapCategoryDetail.DefaultIfEmpty(), (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             y.ScrapCategoryId,
                                                                                             y.Unit,
                                                                                         })
                                                                               .GroupJoin(_scrapCategoryRepository.GetAllAsNoTracking(), x => x.ScrapCategoryId, y => y.Id,
                                                                                         (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             x.Unit,
                                                                                             ScrapCategory = y
                                                                                         }).SelectMany(x => x.ScrapCategory.DefaultIfEmpty(), (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             x.Unit,
                                                                                             y.Name
                                                                                         }).Select(x => new TransactionDetailHistoryViewModel()
                                                                                         {
                                                                                             Quantity = x.Quantity,
                                                                                             ScrapCategoryName = x.Name,
                                                                                             Unit = x.Unit,
                                                                                             Total = x.Total,
                                                                                         }).ToList();
                var dataResult = new SellCollectTransactionHistoryDetailViewModel()
                {
                    CollectingRequestCode = collectingRequest.CollectingRequestCode,
                    SellerName = sellerName,
                    DayOfWeek = transaction.CreatedTime.GetDayOfWeek(),
                    Date = transaction.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                    Time = transaction.CreatedTime.Value.TimeOfDay.ToString(TimeSpanFormat.HH_MM),
                    Total = transaction.Total,
                    Status = collectingRequest.Status,
                    TransactionFee = transaction.TransactionServiceFee,
                    Items = transactionDetailItems
                };

                return BaseApiResponse.OK(dataResult);

            }

            var dataRes = new SellCollectTransactionHistoryDetailViewModel()
            {
                CollectingRequestCode = collectingRequest.CollectingRequestCode,
                Status = collectingRequest.Status,
                DayOfWeek = collectingRequest.UpdatedTime.GetDayOfWeek(),
                Date = collectingRequest.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                Time = collectingRequest.UpdatedTime.Value.TimeOfDay.ToString(TimeSpanFormat.HH_MM),
                SellerName = sellerName
            };

            return BaseApiResponse.OK(dataRes);
        }

        #endregion
    }
}
