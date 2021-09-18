using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SignalR.CollectorHubs.Hubs;
using SCSS.WebApi.SignalR.CollectorHubs.IHubs;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class CollectingRequestController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The collecting request service
        /// </summary>
        private readonly ICollectingRequestService _collectingRequestService;

        #endregion

        #region HubContext

        /// <summary>
        /// The collecing request hub context
        /// </summary>
        private readonly IHubContext<CollectingRequestHub, ICollecingRequestHub> _collecingRequestHubContext;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestController"/> class.
        /// </summary>
        /// <param name="collectingRequestService">The collecting request service.</param>
        /// <param name="collecingRequestHubContext">The collecing request hub context.</param>
        public CollectingRequestController(ICollectingRequestService collectingRequestService, IHubContext<CollectingRequestHub, ICollecingRequestHub> collecingRequestHubContext)
        {
            _collectingRequestService = collectingRequestService;
            _collecingRequestHubContext = collecingRequestHubContext;
        }

        #endregion

        #region Get Collecting Request List

        /// <summary>
        /// Gets the collecting request list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Get)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingRequestList([FromQuery] CollectingRequestFilterModel model)
        {
            return await _collectingRequestService.GetCollectingRequestList(model);
        }

        #endregion Get Collecting Request List

        #region Get Collecting Request Detail

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Detail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingRequestDetail([FromQuery] Guid id)
        {
            return await _collectingRequestService.GetCollectingRequestDetail(id);
        }

        #endregion Get Collecting Request Detail

        #region Receive the Collecting Request 

        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Receive)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReceiveCollectingRequest([FromBody] Guid id)
        {
            var resTuple = await _collectingRequestService.ReceiveCollectingRequest(id);

            if (resTuple == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.SystemException);
            }
            // Push Collecting Request id to another collector app
            await _collecingRequestHubContext.Clients.All.ReceiveCollectingRequest(resTuple.Item2);

            // Push notification to Seller App
            return await _collectingRequestService.SendNotificationToSeller(resTuple.Item1, resTuple.Item3);
        }

        #endregion Receive the Collecting Request 

        #region Reject the Collecting Request

        /// <summary>
        /// Rejects the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Reject)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RejectCollectingRequest([FromBody] CollectingRequestRejectModel model)
        {
            return await _collectingRequestService.RejectCollectingRequest(model);
        }

        #endregion Reject the Collecting Request

        #region Get List Of Collecting Request which was received by collector

        /// <summary>
        /// Gets the collecting request received list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.GetReceivedList)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingRequestReceivedList([FromQuery] CollectingRequestReceivingFilterModel model)
        {
            return await _collectingRequestService.GetCollectingRequestReceivedList(model);
        }

        #endregion

        #region Get Collecting Request Detail which was received by collector

        /// <summary>
        /// Gets the collecting request detail received.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.GetReceivedDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingRequestDetailReceived([FromQuery] Guid id)
        {
            return await _collectingRequestService.GetCollectingRequestDetailReceived(id);
        }

        #endregion Get Collecting Request Detail which was received by collector

        #region Cancel Collecting Request By Collector

        /// <summary>
        /// Cancels the collecting request received.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Cancel)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CancelCollectingRequestReceived([FromBody] CollectingRequestReceivedCancelModel model)
        {
            return await _collectingRequestService.CancelCollectingRequestReceived(model);
        }

        #endregion
    }
}
