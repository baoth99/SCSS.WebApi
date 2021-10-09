using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.CollectorCancelReasonModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class CollectorCancelReasonController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The collector cancel reason service
        /// </summary>
        private readonly ICollectorCancelReasonService _collectorCancelReasonService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectorCancelReasonController"/> class.
        /// </summary>
        /// <param name="collectorCancelReasonService">The collector cancel reason service.</param>
        public CollectorCancelReasonController(ICollectorCancelReasonService collectorCancelReasonService)
        {
            _collectorCancelReasonService = collectorCancelReasonService;
        }

        #endregion

        #region Create New Cancel Reason

        /// <summary>
        /// Creates the new cancel reason.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectorCancelReasonApiUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateNewCancelReason([FromBody] CollectorCancelReasonCreateModel model)
        {
            return await _collectorCancelReasonService.CreateNewCancelReason(model);
        }

        #endregion

        #region Update Cancel Reason

        /// <summary>
        /// Updates the new cancel reason.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectorCancelReasonApiUrl.Update)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateNewCancelReason([FromBody] CollectorCancelReasonUpdateModel model)
        {
            return await _collectorCancelReasonService.UpdateCancelReason(model);
        }

        #endregion

        #region Delete Cancel Reason

        /// <summary>
        /// Deletes the cancel reason.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectorCancelReasonApiUrl.Delete)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> DeleteCancelReason([FromQuery] Guid id)
        {
            return await _collectorCancelReasonService.DeleteCancelReason(id);
        }

        #endregion

        #region Get Cancel Reasons

        /// <summary>
        /// Gets the cancel reasons.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectorCancelReasonApiUrl.Get)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCancelReasons()
        {
            return await _collectorCancelReasonService.GetCancelReasons();
        }

        #endregion
    }
}
