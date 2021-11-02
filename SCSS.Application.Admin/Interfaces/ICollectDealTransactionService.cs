using SCSS.Application.Admin.Models.TransactionModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ICollectDealTransactionService
    {
        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> Search(CollectDealTransactionSearchModel model);

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDetail(Guid id);
    }
}
