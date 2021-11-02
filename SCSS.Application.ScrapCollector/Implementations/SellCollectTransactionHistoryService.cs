using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Models;
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
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingTransactionHistories(BaseFilterModel model)
        {
            var collectorId = UserAuthSession.UserSession.Id;

            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => CollectionConstants.CollectingRequestHistory.Contains(x.Status.Value) &&
                                                                                  x.CollectorAccountId.Equals(collectorId))
                                                        .GroupJoin(_sellCollectTransactionRepository.GetAllAsNoTracking(), x => x.Id, y => y.CollectingRequestId,
                                                                             (x, y) => new
                                                                             {
                                                                                 CollectingRequestId = x.Id,
                                                                                 x.SellerAccountId,
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
                                                                                 x.SellerAccountId,
                                                                                 x.Status,
                                                                                 y.Total,
                                                                                 y.TransactionServiceFee,
                                                                                 TransactionDate = y.CreatedTime,
                                                                                 x.CollectingUpdateTime
                                                                             })
                                                        .Join(_accountRepository.GetAllAsNoTracking(), x => x.SellerAccountId, y => y.Id,
                                                                            (x, y) => new
                                                                            {
                                                                                x.CollectingRequestId,
                                                                                x.CollectingRequestCode,
                                                                                SellerName = y.Name,
                                                                                x.Status,
                                                                                x.Total,
                                                                                x.TransactionServiceFee,
                                                                                x.TransactionDate,
                                                                                x.CollectingUpdateTime
                                                                            });
                                                        
                                                        

            var totalRecord = await dataQuery.CountAsync();

            var sortedList = dataQuery.ToList().Select(x => new
            {
                x.CollectingRequestId,
                x.SellerName,
                x.CollectingRequestCode,
                x.Status,
                x.TransactionDate,
                x.CollectingUpdateTime,
                x.Total,
                DoneDateTime = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value, x.TransactionDate, x.CollectingUpdateTime)
            }).OrderByDescending(x => x.DoneDateTime);


            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;

            var dataResult = sortedList.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new SellCollectTransactionHistoryViewModel()
            {
                CollectingRequestId = x.CollectingRequestId,
                SellerName = x.SellerName,
                CollectingRequestCode = x.CollectingRequestCode,
                DoneDateTime = x.DoneDateTime,
                Status = x.Status,
                Date = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value, x.TransactionDate, x.CollectingUpdateTime).ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                DayOfWeek = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value, x.TransactionDate, x.CollectingUpdateTime).GetDayOfWeek(),
                Time = DateTimeUtils.GetTransactionHistoryDate(x.Status.Value, x.TransactionDate, x.CollectingUpdateTime).Value.TimeOfDay.ToString(TimeSpanFormat.HH_MM),
                Total = x.Total
            }).ToList();

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
            var collectingRequest = await _collectingRequestRepository.GetAsyncAsNoTracking(x => x.Id.Equals(collectingRequestId));

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
