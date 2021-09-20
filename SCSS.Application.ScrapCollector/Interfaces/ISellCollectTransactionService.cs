using SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface ISellCollectTransactionService
    {
        Task<BaseApiResponseModel> GetTransactionInfoReview(Guid collectingRequestId);

        Task<BaseApiResponseModel> CreateCollectingTransaction(SellCollectTransactionCreateModel model);

        Task<BaseApiResponseModel> GetCollectingTransactionHistories();

        Task<BaseApiResponseModel> GetCollectingTransactionDetailHistory(Guid collectingRequestId);
    }
}
