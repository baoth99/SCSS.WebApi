using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Models.AccountModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using ICollectorAccountService = SCSS.Application.ScrapCollector.Interfaces.IAccountService;
using CollectorSendOTPRequestModel = SCSS.Application.ScrapCollector.Models.AccountModels.SendOTPRequestModel;
using IDealerAccountService = SCSS.Application.ScrapDealer.Interfaces.IAccountService;
using DealerSendOTPRequestModel = SCSS.Application.ScrapDealer.Models.AccountModels.SendOTPRequestModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.Application.ScrapDealer.Models.AccountModels;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class RegisterController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The collector account service
        /// </summary>
        private readonly ICollectorAccountService _collectorAccountService;

        /// <summary>
        /// The dealer account service
        /// </summary>
        private readonly IDealerAccountService _dealerAccountService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterController"/> class.
        /// </summary>
        /// <param name="collectorAccountService">The collector account service.</param>
        /// <param name="dealerAccountService">The dealer account service.</param>
        public RegisterController(ICollectorAccountService collectorAccountService, IDealerAccountService dealerAccountService)
        {
            _collectorAccountService = collectorAccountService;
            _dealerAccountService = dealerAccountService;
        }

        #endregion

        #region Send OTP to Register

        /// <summary>
        /// Sends the otp to register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.CollectorOtp)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SendCollectorOtpToRegister([FromBody] CollectorSendOTPRequestModel model)
        {
            return await _collectorAccountService.SendOtpToRegister(model);
        }

        #endregion

        #region Create Collector Account

        /// <summary>
        /// Creates the collector.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.CreateCollector)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateCollector([FromBody] CollectorAccountRegisterRequestModel model)
        {
            return await _collectorAccountService.RegisterCollectorAccount(model);
        }


        #endregion


        #region Send Dealer OTP to Register

        //[HttpPost]
        //[ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        //[ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        //[ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        //[Route(AdminApiUrlDefinition.RegisterApiUrl.DealerOtp)]
        //[ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        //public async Task<BaseApiResponseModel> SendDealerOtpToRegister([FromBody] DealerSendOTPRequestModel model)
        //{
        //    return await _dealerAccountService.SendOtpToRegister(model);
        //}

        #endregion

        #region Create Dealer Account

        /// <summary>
        /// Creates the collector.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.CreateDealer)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateDealer([FromBody] DealerAccountRegisterRequestModel model)
        {
            return await _dealerAccountService.RegisterDealerAccount(model);
        }

        #endregion
    }
}
