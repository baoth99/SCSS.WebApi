using SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface ICollectDealTransactionService
    {
        /// <summary>
        /// Automatics the complete collector phone.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> AutoCompleteCollectorPhone();

        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionInfoReview(Guid id);

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

        /// <summary>
        /// Creates the collect deal transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateCollectDealTransaction(TransactionCreateModel model);

        /// <summary>
        /// Gets the transaction histories.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionHistories(TransactionHistoryFilterModel model);

        /// <summary>
        /// Gets the transaction history detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionHistoryDetail(Guid id);
    }
}
