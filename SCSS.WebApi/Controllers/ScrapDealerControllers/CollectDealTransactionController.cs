using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDealTransactionController"/> class.
        /// </summary>
        /// <param name="collectDealTransactionService">The collect deal transaction service.</param>
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

        #region Create Collect-Deal Transaction

        /// <summary>
        /// Creates the collect deal transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.CollectDealTransactionApiUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateCollectDealTransaction([FromBody] TransactionCreateModel model)
        {
            return await _collectDealTransactionService.CreateCollectDealTransaction(model);
        }

        #endregion


        #region Get Collect-Deal Transaction Histories



        #endregion


        #region Get Collect-Deal Transaction History Detail



        #endregion
    }
}
