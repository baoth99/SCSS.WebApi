using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.RequestRegisterModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class RequestRegisterController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The request register service
        /// </summary>
        private readonly IRequestRegisterService _requestRegisterService;

        #endregion

        #region Contructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestRegisterController"/> class.
        /// </summary>
        /// <param name="requestRegisterService">The request register service.</param>
        public RequestRegisterController(IRequestRegisterService requestRegisterService)
        {
            _requestRegisterService = requestRegisterService;
        }

        #endregion

        #region Search Collector Request Register

        /// <summary>
        /// Searches the collector request register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RequestRegisterApiUrl.SearchCollectors)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchCollectorRequestRegister([FromQuery] CollectorRequestRegisterSearchModel model)
        {
            return await _requestRegisterService.SearchCollectorRequestRegister(model);
        }

        #endregion

        #region Search Dealer Request Register

        /// <summary>
        /// Searches the dealer request register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RequestRegisterApiUrl.SearchDealers)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchDealerRequestRegister([FromQuery] DealerRequestRegisterSearchModel model)
        {
            return await _requestRegisterService.SearchDealerRequestRegister(model);
        }

        #endregion

        #region Get Collector Request Register Detail

        /// <summary>
        /// Gets the collector request register.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RequestRegisterApiUrl.CollectorDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectorRequestRegister([FromQuery]Guid id)
        {
            return await _requestRegisterService.GetCollectorRequestRegister(id);
        }

        #endregion

        #region Get Dealer Request Register Detail

        /// <summary>
        /// Gets the dealer request register.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RequestRegisterApiUrl.DealerDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerRequestRegister([FromQuery] Guid id)
        {
            return await _requestRegisterService.GetDealerRequestRegister(id);
        }

        #endregion

    }
}
