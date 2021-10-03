using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class CollectDealTransactionController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The collect deal transaction service
        /// </summary>
        private readonly ICollectDealTransactionService _collectDealTransactionService;

        #endregion

        #region Constructor

        public CollectDealTransactionController(ICollectDealTransactionService collectDealTransactionService)
        {
            _collectDealTransactionService = collectDealTransactionService;
        }

        #endregion


        #region Get Transaction Info Review

        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="collectorId">The collector identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.CollectDealTransactionApiUrl.GetTransactionInfoReview)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetTransactionInfoReview([FromQuery] Guid collectorId)
        {
            return await _collectDealTransactionService.GetTransactionInfoReview(collectorId);
        }

        #endregion

    }
}
