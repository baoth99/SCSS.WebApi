using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Models;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.AccountModels;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
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

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accountService">The account service.</param>
        /// <param name="storageBlobS3Service">The storage BLOB s3 service.</param>
        public AccountController(IAccountService accountService, IStorageBlobS3Service storageBlobS3Service)
        {
            _accountService = accountService;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion

        #region Register Scrap Collector Account

        /// <summary>
        /// Registers the scrap collector account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel),HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel),HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.AccountApiUrl.RegisterCollectorAccount)]
        public async Task<BaseApiResponseModel> RegisterScrapCollectorAccount(CollectorAccountRegisterRequestModel model)
        {
            return await _accountService.RegisterCollectorAccount(model);
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
        [Route(ScrapCollectorApiUrlDefinition.AccountApiUrl.UploadImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadDealerAccountImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.CollectorAccount, model.Image.FileName);
            var imageUrl = await _storageBlobS3Service.UploadFile(model.Image, fileName, FileS3Path.CollectorAccountImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion

        #region Update Scrap Collector Account

        /// <summary>
        /// Updates the scrap collector account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.AccountApiUrl.UpdateCollectorAccount)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateScrapCollectorAccount([FromBody] CollectorAccountUpdateRequestModel model)
        {
            return await _accountService.UpdateAccountInformation(model);
        }

        #endregion

        #region Update DeviceId

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="deviceId">The device identifier.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.AccountApiUrl.UpdateDeviceId)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateDeviceId([FromBody] string deviceId)
        {
            return await _accountService.UpdateDeviceId(deviceId);
        }   

        #endregion
    }
}
