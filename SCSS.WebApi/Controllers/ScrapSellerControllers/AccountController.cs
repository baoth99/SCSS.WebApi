using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.AccountModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class AccountController : BaseScrapSellerControllers
    {
        #region Services

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;

        #endregion

        #region Constructor

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        #endregion

        #region Register Scrap Seller Account

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.RegisterSellerAccount)]
        public async Task<BaseApiResponseModel> RegisterScrapSellerAccount(AccountRegistrationModel model)
        {
            return await _accountService.Register(model);
        }

        #endregion

        #region Update Scrap Seller Profile

        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.UpdateSellerAccount)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateScrapSellerAccount()
        {
            return null;
        }

        #endregion
    }
}
