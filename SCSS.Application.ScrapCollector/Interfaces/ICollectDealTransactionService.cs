using SCSS.Application.ScrapCollector.Models;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface ICollectDealTransactionService
    {
        /// <summary>
        /// Gets the collect deal transactions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectDealTransactions(BaseFilterModel model);

        /// <summary>
        /// Gets the collect deal transaction detail.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectDealTransactionDetail(Guid transId);
    }
}
