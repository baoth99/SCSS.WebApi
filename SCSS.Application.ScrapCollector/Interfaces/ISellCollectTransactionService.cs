using SCSS.Application.ScrapCollector.Models;
using SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface ISellCollectTransactionService
    {
        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionInfoReview(Guid collectingRequestId);

        /// <summary>
        /// Creates the collecting transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateCollectingTransaction(SellCollectTransactionCreateModel model);

        /// <summary>
        /// Gets the collecting transaction histories.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingTransactionHistories(BaseFilterModel model);

        /// <summary>
        /// Gets the collecting transaction detail history.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingTransactionDetailHistory(Guid collectingRequestId);

        /// <summary>
        /// Gets the scrap category transaction.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionScrapCategories();

        /// <summary>
        /// Gets the transaction scrap category detail.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionScrapCategoryDetail(Guid id);
    }
}
