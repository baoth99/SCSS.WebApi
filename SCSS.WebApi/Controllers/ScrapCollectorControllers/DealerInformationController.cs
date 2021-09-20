using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.DealerInformationModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class DealerInformationController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The dealer information service
        /// </summary>
        private readonly IDealerInformationService _dealerInformationService;

        /// <summary>
        /// The dealer promotion service
        /// </summary>
        private readonly IDealerPromotionService _dealerPromotionService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DealerInformationController"/> class.
        /// </summary>
        /// <param name="dealerInformationService">The dealer information service.</param>
        /// <param name="dealerPromotionService">The dealer promotion service.</param>
        public DealerInformationController(IDealerInformationService dealerInformationService, IDealerPromotionService dealerPromotionService)
        {
            _dealerInformationService = dealerInformationService;
            _dealerPromotionService = dealerPromotionService;
        }

        #endregion


        #region Search The Nearest Dealer  

        /// <summary>
        /// Searches the dealer information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.DealerInformationApiUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchDealerInfo([FromQuery] DealerInformationFilterModel model)
        {
            return await _dealerInformationService.SearchDealerInfo(model);
        }

        #endregion

        #region Get Dealer Information Detail

        /// <summary>
        /// Gets the dealer information detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.DealerInformationApiUrl.Detail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerInfoDetail([FromQuery] Guid id)
        {
            return await _dealerInformationService.GetDealerInformationDetail(id);
        }

        #endregion

        #region Get Dealer Promotions

        /// <summary>
        /// Gets the dealer promotions.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.DealerInformationApiUrl.DealerPromotion)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerPromotions([FromQuery] Guid dealerId)
        {
            return await _dealerPromotionService.GetDealerPromotions(dealerId);
        }

        #endregion

        #region Get Dealer Promotion Detail

        /// <summary>
        /// Gets the dealer information detail.
        /// </summary>
        /// <param name="promotionId">The promotion identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.DealerInformationApiUrl.DealerPromotionDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerInformationDetail([FromQuery] Guid promotionId)
        {
            return await _dealerPromotionService.GetDealerPromotionDetail(promotionId);
        }

        #endregion

    }
}
