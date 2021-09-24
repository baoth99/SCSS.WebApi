using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface ICollectingRequestService
    {
        /// <summary>
        /// Requests the scrap collecting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RequestScrapCollecting(CollectingRequestCreateModel model);

        /// <summary>
        /// Cancels the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CancelCollectingRequest(CollectingRequestCancelModel model);

        /// <summary>
        /// Gets the collecting requests.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequests();

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id);
    }
}
