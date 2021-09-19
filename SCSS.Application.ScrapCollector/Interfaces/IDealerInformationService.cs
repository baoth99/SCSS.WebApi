using SCSS.Application.ScrapCollector.Models.DealerInformationModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IDealerInformationService
    {
        /// <summary>
        /// Searches the dealer information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchDealerInfo(DealerInformationFilterModel model);

        /// <summary>
        /// Gets the dealer information detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerInformationDetail(Guid id);    
    }
}
