using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Models;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.AccountModels;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
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

        #region Register Scrap Seller Account

        /// <summary>
        /// Registers the scrap seller account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.RegisterSellerAccount)]
        public async Task<BaseApiResponseModel> RegisterScrapSellerAccount(SellerAccountRegistrationModel model)
        {
            return await _accountService.Register(model);
        }

        #endregion

        #region Upload Seller Account Image

        /// <summary>
        /// Uploads the dealer account image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.UploadImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadSellerAccountImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.SellerAccount, model.Image.FileName);
            var imageUrl = await _storageBlobS3Service.UploadImageFile(model.Image, fileName, FileS3Path.SellerAccountImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion

        #region Update Scrap Seller Profile

        /// <summary>
        /// Updates the scrap seller account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.UpdateSellerAccount)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateScrapSellerAccount(SellerAccountUpdateProfileModel model)
        {
            return await _accountService.UpdateAccount(model);
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
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.UpdateDeviceId)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateDeviceId([FromBody] string deviceId)
        {
            return await _accountService.UpdateDeviceId(deviceId);
        }

        #endregion

        #region Get Collector Information Detail

        /// <summary>
        /// Gets the seller account information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.AccountApiUrl.InfoDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetSellerAccountInfo()
        {
            return await _accountService.GetSellerAccountInfo();
        }

        #endregion
    }
}
