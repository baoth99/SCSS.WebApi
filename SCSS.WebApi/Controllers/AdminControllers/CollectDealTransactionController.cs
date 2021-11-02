using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.TransactionModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class CollectDealTransactionController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The collect deal transaction service
        /// </summary>
        private readonly ICollectDealTransactionService _collectDealTransactionService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDealTransactionController"/> class.
        /// </summary>
        /// <param name="collectDealTransactionService">The collect deal transaction service.</param>
        public CollectDealTransactionController(ICollectDealTransactionService collectDealTransactionService)
        {
            _collectDealTransactionService = collectDealTransactionService;
        }

        #endregion

        #region Search 

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectDealTransactionApiUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> Search([FromQuery] CollectDealTransactionSearchModel model)
        {
            return await _collectDealTransactionService.Search(model);
        }

        #endregion

        #region Get Detail

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.CollectDealTransactionApiUrl.GetDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDetail([FromQuery] Guid id)
        {
            return await _collectDealTransactionService.GetDetail(id);
        }

        #endregion
    }
}
