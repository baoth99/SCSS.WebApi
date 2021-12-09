using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.CollectingRequestModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
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

        #region Get Current Collecting Requests

        /// <summary>
        /// Gets the current collecting requests.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.CurrentRequests)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCurrentCollectingRequests([FromQuery] CollectingRequestFilterModel model)
        {

            return await _collectingRequestService.GetCollectingRequests(model, CollectionConstants.CurrentRequests);
        }

        #endregion Get Current Collecting Requests

        #region Get Collecting Appointments

        /// <summary>
        /// Gets the collecting appointment.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Appointments)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingAppointment([FromQuery] CollectingRequestFilterModel model)
        {
            return await _collectingRequestService.GetCollectingRequests(model, CollectionConstants.Appointments);
        }

        #endregion

        #region Get Collecting Request Detail

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Detail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingRequestDetail([FromQuery] Guid id)
        {
            return await _collectingRequestService.GetCollectingRequestDetail(id);
        }

        #endregion Get Collecting Request Detail

        [Microsoft.AspNetCore.Authorization.AllowAnonymous]
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Receive + "/test-signalr")]
        public async Task<BaseApiResponseModel> TestSignalR([FromQuery] Guid id)
        {
            await _collecingRequestHubContext.Clients.All.ReceiveCollectingRequest(id);
            return BaseApiResponse.OK();
        }


        #region Receive the Collecting Request 

        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Receive)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReceiveCollectingRequest([FromQuery] Guid id)
        {
            var msgErrCode = await _collectingRequestService.ValidateCollectingRequest(id);
            if (!ValidatorUtil.IsBlank(msgErrCode))
            {
                if (msgErrCode == SystemMessageCode.DataNotFound)
                {
                    return BaseApiResponse.NotFound();
                }
                return BaseApiResponse.Error(msgErrCode);
            }
            var resTuple = await _collectingRequestService.ReceiveCollectingRequest(id);
            if (resTuple == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.SystemException);
            }
            // Push Collecting Request id to another collector app
            await _collecingRequestHubContext.Clients.All.ReceiveCollectingRequest(resTuple.Item2);

            // Push notification to Seller App
            return await _collectingRequestService.SendNotification(resTuple.Item1, resTuple.Item2, resTuple.Item3);
        }

        #endregion Receive the Collecting Request 

        #region Get List Of Collecting Request which was received by collector

        /// <summary>
        /// Gets the collecting request received list.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
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
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
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
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.Cancel)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CancelCollectingRequestReceived([FromBody] CollectingRequestReceivedCancelModel model)
        {
            return await _collectingRequestService.CancelCollectingRequestReceived(model);
        }

        #endregion

        #region Check Collecting Request Is Approved

        /// <summary>
        /// Determines whether [is request approved] [the specified identifier].
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.IsApproved)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> IsRequestApproved([FromQuery] Guid id)
        {
            return await _collectingRequestService.CheckCollectingRequestIsApproved(id);
        }

        #endregion

        #region Collecting Request

        /// <summary>
        /// Gets the cancel reasons.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectingRequestApiUrl.CancelReasons)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCancelReasons()
        {
            return await _collectingRequestService.GetCancelReasons();
        }

        #endregion
    }
}
