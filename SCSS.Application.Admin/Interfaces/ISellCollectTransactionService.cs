using SCSS.Application.Admin.Models.TransactionModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ISellCollectTransactionService
    {
        /// <summary>
        /// Searches the sell collect transactions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchSellCollectTransactions(SellCollectTransactionSearchModel model);

        /// <summary>
        /// Gets the sell collect transaction detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetSellCollectTransactionDetail(Guid id);
    }
}
