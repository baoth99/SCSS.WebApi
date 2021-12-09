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
using DealerSendOTPRequestModel = SCSS.Application.ScrapDealer.Models.AccountModels.SendOTPRequestModel;
using IDealerAccountService = SCSS.Application.ScrapDealer.Interfaces.IAccountService;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.Application.ScrapDealer.Models.AccountModels;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Helper;
using SCSS.Application.Admin.Models;
using SCSS.Utilities.BaseResponse;

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

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterController"/> class.
        /// </summary>
        /// <param name="collectorAccountService">The collector account service.</param>
        /// <param name="dealerAccountService">The dealer account service.</param>
        /// <param name="storageBlobS3Service">The storage BLOB s3 service.</param>
        public RegisterController(ICollectorAccountService collectorAccountService, IDealerAccountService dealerAccountService, IStorageBlobS3Service storageBlobS3Service)
        {
            _collectorAccountService = collectorAccountService;
            _dealerAccountService = dealerAccountService;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion

        #region Upload Collector Account Image

        /// <summary>
        /// Uploads the dealer account image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.UploadCollectorImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadCollectorAccountImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.CollectorAccount, model.Image.FileName);
            var imageUrl = await _storageBlobS3Service.UploadImageFile(model.Image, fileName, FileS3Path.CollectorAccountImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion

        #region Upload Dealer Account Image

        /// <summary>
        /// Uploads the dealer account image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.UploadDealerImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadDealerAccountImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.DealerAccount, model.Image.FileName);
            var imageUrl = await _storageBlobS3Service.UploadImageFile(model.Image, fileName, FileS3Path.DealerAccountImages);
            return BaseApiResponse.OK(imageUrl);
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

        /// <summary>
        /// Sends the dealer otp to register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.DealerOtp)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SendDealerOtpToRegister([FromBody] DealerSendOTPRequestModel model)
        {
            return await _dealerAccountService.SendOtpToRegister(model);
        }

        #endregion


        #region MyRegion

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
        public async Task<BaseApiResponseModel> CreateDealer([FromForm] DealerAccountRegisterRequestModel model)
        {
            return await _dealerAccountService.RegisterDealerAccount(model);
        }

        #endregion

        #region Get Dealer Leader List

        /// <summary>
        /// Gets the dealer leader list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.RegisterApiUrl.GetDealerLeader)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerLeaderList()
        {
            return await _dealerAccountService.GetDealerLeaderList();
        }

        #endregion
    }
}
