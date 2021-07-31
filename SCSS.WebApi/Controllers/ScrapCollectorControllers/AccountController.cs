using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class AccountController : BaseScrapCollectorController
    {
        #region Services

        #endregion

        #region Constructor

        public AccountController()
        {
        }

        #endregion

        #region Register Scrap Collector Account

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel),HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel),HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.AccountApiUrl.RegisterCollectorAccount)]
        public async Task<BaseApiResponseModel> RegisterScrapCollectorAccount()
        {
            return null;
        }

        #endregion

        #region Update Scrap Collector Account

        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.AccountApiUrl.UpdateCollectorAccount)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateScrapCollectorAccount()
        {
            return null;
        }

        #endregion
    }
}
