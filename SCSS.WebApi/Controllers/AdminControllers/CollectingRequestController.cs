using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.CollectingRequestModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class CollectingRequestController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The collecting request service
        /// </summary>
        private readonly ICollectingRequestService _collectingRequestService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestController"/> class.
        /// </summary>
        /// <param name="collectingRequestService">The collecting request service.</param>
        public CollectingRequestController(ICollectingRequestService collectingRequestService)
        {
            _collectingRequestService = collectingRequestService;
        }

        #endregion

        #region Search Collecting Requests

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectingRequestApiUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> Search([FromQuery] CollectingRequestSearchModel model)
        {
            return await _collectingRequestService.SearchCollectingRequest(model);
        }

        #endregion Search Collecting Requests

        #region Get Collecting Request Detail

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectingRequestApiUrl.Detail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDetail([FromQuery] Guid id)
        {
            return await _collectingRequestService.GetCollectingRequestDetail(id);
        }

        #endregion Get Collecting Request Detail
    }
}
