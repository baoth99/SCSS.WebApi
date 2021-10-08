using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class CollectDealTransactionController : BaseScrapCollectorController
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

        #region Get Collect-Deal Transactions

        /// <summary>
        /// Gets the collect deal transactions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectDealTransactionApiUrl.GetTransactions)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectDealTransactions([FromQuery] BaseFilterModel model)
        {
            return await _collectDealTransactionService.GetCollectDealTransactions(model);
        }

        #endregion

        #region Get Collect-Deal Transaction Detail

        /// <summary>
        /// Gets the collect deal transaction detail.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.CollectDealTransactionApiUrl.GetTransactionHistoryDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectDealTransactionDetail([FromQuery] Guid transId)
        {
            return await _collectDealTransactionService.GetCollectDealTransactionDetail(transId);
        }

        #endregion

    }
}
