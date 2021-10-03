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
    }
}
