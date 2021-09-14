using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.DealerInformationModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class DealerInformationController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The dealer information service
        /// </summary>
        private readonly IDealerInformationService _dealerInformationService;

        #endregion

        #region Constructor

        public DealerInformationController(IDealerInformationService dealerInformationService)
        {
            _dealerInformationService = dealerInformationService;
        }

        #endregion

        #region Search Dealer Information

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.DealerInformationUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> Search([FromQuery] DealerInformationSearchModel model)
        {
            return await _dealerInformationService.Search(model);
        }

        #endregion

        #region Get Dealer Information Detail

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.DealerInformationUrl.GetDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDetail([FromQuery] Guid id)
        {
            return await _dealerInformationService.GetDetail(id);
        }

        #endregion
    }
}
