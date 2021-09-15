using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface IDealerInformationService
    {
        /// <summary>
        /// Gets the dealer information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerInformation();

        /// <summary>
        /// Gets the dealer branchs information.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerBranchsInfo();

        /// <summary>
        /// Gets the dealer branch information detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerBranchInfoDetail(Guid id);

        /// <summary>
        /// Changes the dealer status.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ChangeDealerStatus(bool status);
    }
}
