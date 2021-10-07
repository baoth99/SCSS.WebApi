using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models;
using SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels;
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
    public class SellCollectTransactionController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The sell collect transaction service
        /// </summary>
        private readonly ISellCollectTransactionService _sellCollectTransactionService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SellCollectTransactionController"/> class.
        /// </summary>
        /// <param name="sellCollectTransactionService">The sell collect transaction service.</param>
        public SellCollectTransactionController(ISellCollectTransactionService sellCollectTransactionService)
        {
            _sellCollectTransactionService = sellCollectTransactionService;
        }

        #endregion

        #region Get Transaction Info Review

        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.SellCollectTransactionApiUrl.GetTransactionInfoReview)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetTransactionInfoReview([FromQuery] Guid collectingRequestId)
        {
            return await _sellCollectTransactionService.GetTransactionInfoReview(collectingRequestId);
        }

        #endregion

        #region Create Collecting Transaction

        /// <summary>
        /// Creates the collecting transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.SellCollectTransactionApiUrl.CreateTransaction)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateCollectingTransaction([FromBody] SellCollectTransactionCreateModel model)
        {
            return await _sellCollectTransactionService.CreateCollectingTransaction(model);
        }

        #endregion

        #region Get Collecting Transaction History

        /// <summary>
        /// Gets the collecting transaction history.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.SellCollectTransactionApiUrl.GetTransactionHistories)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingTransactionHistory([FromQuery] BaseFilterModel model)
        {
            return await _sellCollectTransactionService.GetCollectingTransactionHistories(model);
        }

        #endregion

        #region Get Collecting Transactio nHistory Detail

        /// <summary>
        /// Gets the collecting transaction detail history.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.SellCollectTransactionApiUrl.GetTransactionHistoryDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectingTransactionDetailHistory([FromQuery] Guid collectingRequestId)
        {
            return await _sellCollectTransactionService.GetCollectingTransactionDetailHistory(collectingRequestId);
        }

        #endregion
    }
}
