using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface ICollectingRequestService
    {
        /// <summary>
        /// Gets the collecting request list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequests(CollectingRequestFilterModel model, List<int> requestTypes);

        /// <summary>
        /// Checks the maximum number collecting requests collector recevice.
        /// </summary>
        /// <returns></returns>
        Task<string> ValidateCollectingRequest(Guid id);

        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<Tuple<Guid?, Guid, string>> ReceiveCollectingRequest(Guid id);

        /// <summary>
        /// Sends the notification to seller.
        /// </summary>
        /// <param name="sellerId">The seller identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SendNotification(Guid? sellerId, Guid? requestId, string collectingRequestCode);

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id);

        /// <summary>
        /// Gets the collecting request received list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequestReceivedList(CollectingRequestReceivingFilterModel model);

        /// <summary>
        /// Gets the collecting request detail received.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectingRequestDetailReceived(Guid id);

        /// <summary>
        /// Gets the cancel reasons.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCancelReasons();


        /// <summary>
        /// Cancels the collecting request received.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CancelCollectingRequestReceived(CollectingRequestReceivedCancelModel model);

        /// <summary>
        /// Checks the collecting request is approved.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CheckCollectingRequestIsApproved(Guid id);

    }
}
