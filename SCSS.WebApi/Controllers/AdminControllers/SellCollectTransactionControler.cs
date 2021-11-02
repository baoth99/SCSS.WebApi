using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.TransactionModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class SellCollectTransactionControler : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The sell collect transaction service
        /// </summary>
        private readonly ISellCollectTransactionService _sellCollectTransactionService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SellCollectTransactionControler"/> class.
        /// </summary>
        /// <param name="sellCollectTransactionService">The sell collect transaction service.</param>
        public SellCollectTransactionControler(ISellCollectTransactionService sellCollectTransactionService)
        {
            _sellCollectTransactionService = sellCollectTransactionService;
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
        [Route(AdminApiUrlDefinition.SellCollectTransactionApiUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> Search([FromQuery] SellCollectTransactionSearchModel model)
        {
            return await _sellCollectTransactionService.SearchSellCollectTransactions(model);
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
        [Route(AdminApiUrlDefinition.SellCollectTransactionApiUrl.GetDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDetail([FromQuery] Guid id)
        {
            return await _sellCollectTransactionService.GetSellCollectTransactionDetail(id);
        }

        #endregion
    }
}
