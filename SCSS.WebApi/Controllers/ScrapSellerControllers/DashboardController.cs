using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class DashboardController : BaseScrapSellerControllers
    {
        #region Services

        /// <summary>
        /// The dashboard service
        /// </summary>
        private readonly IDashboardService _dashboardService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardController"/> class.
        /// </summary>
        /// <param name="dashboardService">The dashboard service.</param>
        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        #region Get The Nearest Approved Collecting Request

        /// <summary>
        /// Gets the nearest approved cr.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.DashboardApiUrl.NearestApprovedCR)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetNearestApprovedCR()
        {
            return await _dashboardService.GetNearestCollectingRequest();
        }

        #endregion

    }
}
