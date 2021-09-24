using SCSS.Application.Admin.Models.CollectingRequestModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ICollectingRequestService
    {
        /// <summary>
        /// Searches the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchCollectingRequest(CollectingRequestSearchModel model);

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id);

    }
}
