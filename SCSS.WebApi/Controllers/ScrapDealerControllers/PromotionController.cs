using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.PromotionModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class PromotionController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The promotion service
        /// </summary>
        private readonly IPromotionService _promotionService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionController"/> class.
        /// </summary>
        /// <param name="promotionService">The promotion service.</param>
        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        #endregion

        #region Create New Promotion

        /// <summary>
        /// Creates the new promotion.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.PromotionApiUrl.CreateNewPromotion)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateNewPromotion([FromBody] PromotionCreateModel model)
        {
            return await _promotionService.CreateNewPromotion(model);
        }

        #endregion

        #region Get Promotions

        /// <summary>
        /// Gets the promotions.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.PromotionApiUrl.GetPromotions)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetPromotions()
        {
            return await _promotionService.GetPromotions();
        }

        #endregion

        #region Get Promotion Detail

        /// <summary>
        /// Gets the promotion detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.PromotionApiUrl.GetPromotionDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetPromotionDetail([FromQuery] Guid id)
        {
            return await _promotionService.GetPromotionDetail(id);
        }

        #endregion
    }
}
