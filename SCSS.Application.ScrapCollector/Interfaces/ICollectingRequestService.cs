using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.Utilities.ResponseModel;
using System;
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
        Task<BaseApiResponseModel> GetCollectingRequestList(CollectingRequestFilterModel model);

        /// <summary>
        /// Checks the maximum number collecting requests collector recevice.
        /// </summary>
        /// <returns></returns>
        Task<string> CheckMaxNumberCollectingRequestsCollectorRecevice();

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
        Task<BaseApiResponseModel> SendNotification(Guid? sellerId, string collectingRequestCode);

        /// <summary>
        /// Rejects the collecting request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RejectCollectingRequest(CollectingRequestRejectModel model);

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
        /// Cancels the collecting request received.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CancelCollectingRequestReceived(CollectingRequestReceivedCancelModel model);

    }
}
