using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Models;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.AccountModels;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class AccountController : BaseScrapDealerController
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

        #endregion Services

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

        #endregion Constructor

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
        [Route(ScrapDealerApiUrlDefinition.AccountApiUrl.UploadImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadDealerAccountImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.DealerAccount, model.Image.FileName);
            var imageUrl = await _storageBlobS3Service.UploadImageFile(model.Image, fileName, FileS3Path.DealerAccountImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion Upload Dealer Account Image

        #region Register Scrap Dealer Account

        /// <summary>
        /// Registers the scrap dealer account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.AccountApiUrl.RegisterDealerAccount)]
        public async Task<BaseApiResponseModel> RegisterScrapDealerAccount([FromForm] DealerAccountRegisterRequestModel model)
        {
            return await _accountService.RegisterDealerAccount(model);
        }

        #endregion Register Scrap Dealer Account

        #region Update Scrap Dealer Account

        /// <summary>
        /// Updates the dealer account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.AccountApiUrl.UpdateDealerAccount)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateDealerAccount([FromBody] DealerAccountUpdateRequestModel model)
        {
            return await _accountService.UpdateDealerAccount(model);
        }

        #endregion Update Scrap Dealer Account

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
        [Route(ScrapDealerApiUrlDefinition.AccountApiUrl.UpdateDeviceId)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateDeviceId([FromBody] DeviceIdUpdateModel model)
        {
            return await _accountService.UpdateDeviceId(model);
        }

        #endregion

        #region Get Dealer Information Detail

        /// <summary>
        /// Gets the dealer account detail.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.AccountApiUrl.InfoDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerAccountDetail()
        {
            return await _accountService.GetDealerAccountInfo();
        }

        #endregion
    }
}
