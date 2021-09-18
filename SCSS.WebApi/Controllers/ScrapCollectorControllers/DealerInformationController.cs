using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.DealerInformationModels;
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
    public class DealerInformationController : BaseScrapCollectorController
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

        

        #endregion

    }
}
