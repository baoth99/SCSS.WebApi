using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SCSS.Application.Admin.Models;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SignalR.CollectorHubs.Hubs;
using SCSS.WebApi.SignalR.CollectorHubs.IHubs;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class CollectingRequestController : BaseScrapSellerControllers
    {
        #region Services

        /// <summary>
        /// The collecting request service
        /// </summary>
        private readonly ICollectingRequestService _collectingRequestService;

        /// <summary>
        /// The storage BLOB service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobService;

        #endregion Services

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
        /// <param name="storageBlobService">The storage BLOB service.</param>
        /// <param name="collecingRequestHubContext">The collecing request hub context.</param>
        public CollectingRequestController(ICollectingRequestService collectingRequestService, IStorageBlobS3Service storageBlobService, IHubContext<CollectingRequestHub, ICollecingRequestHub> collecingRequestHubContext)
        {
            _collectingRequestService = collectingRequestService;
            _storageBlobService = storageBlobService;
            _collecingRequestHubContext = collecingRequestHubContext;
        }

        #endregion

        #region Upload Request Scrap Collecting Image

        /// <summary>
        /// Uploads the scrap collecting image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.CollectingRequestApiUrl.UploadCollectingRequestImg)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadScrapCollectingImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.ScrapCollectingRequest, model.Image.FileName);
            var imageUrl = await _storageBlobService.UploadImageFile(model.Image, fileName, FileS3Path.ScrapCollectingRequestImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion

        #region Get Operating Time Range

        /// <summary>
        /// Gets the operating time range.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.CollectingRequestApiUrl.GetOperatingTimeRange)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetOperatingTimeRange()
        {
            return await _collectingRequestService.GetOperatingTimeRange();
        }

        #endregion

        #region Request Scrap Collecting

        /// <summary>
        /// Requests the scrap collecting.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.CollectingRequestApiUrl.RequestCollecting)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RequestScrapCollecting([FromBody] CollectingRequestCreateModel model)
        {
            return await _collectingRequestService.RequestScrapCollecting(model);
        }

        #endregion

        #region Cancel Scrap Collecting Request

        /// <summary>
        /// Cancels the scrap collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.CollectingRequestApiUrl.CancelCollectingRequest)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CancelScrapCollectingRequest([FromBody] CollectingRequestCancelModel model)
        {
            var response = await _collectingRequestService.CancelCollectingRequest(model);

            if (response == null)
            {
                return BaseApiResponse.Error();
            }

            var requestNotice = new CollectingRequestNoticeModel()
            {
                Id = response.Id,
                RequestType = response.RequestType,
                Status = response.Status
            };

            await _collecingRequestHubContext.Clients.All.ReceiveCollectingRequest(requestNotice);

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Number Of Remaining Days that seller can request

        /// <summary>
        /// Gets the number of remaining days.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.CollectingRequestApiUrl.GetRemainingDays)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetRemainingDays()
        {
            return await _collectingRequestService.GetRemainingDays();
        }

        #endregion

        #region Check Seller Request Ability

        /// <summary>
        /// Checks the seller request ability.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.CollectingRequestApiUrl.RequestAbility)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CheckSellerRequestAbility()
        {
            return await _collectingRequestService.CheckSellerRequestAbility();
        }

        #endregion
    }
}
