using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Models;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class DealerInformationController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The dealer information service
        /// </summary>
        private readonly IDealerInformationService _dealerInformationService;

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DealerInformationController"/> class.
        /// </summary>
        /// <param name="dealerInformationService">The dealer information service.</param>
        /// <param name="storageBlobS3Service">The storage BLOB s3 service.</param>
        public DealerInformationController(IDealerInformationService dealerInformationService, IStorageBlobS3Service storageBlobS3Service)
        {
            _dealerInformationService = dealerInformationService;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion

        #region Upload Dealer Information Image        

        /// <summary>
        /// Uploads the dealer information image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DealerInformationApiUrl.UploadImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadDealerInformationImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.DealerInformation, model.Image.FileName);
            var imageUrl = await _storageBlobS3Service.UploadFile(model.Image, fileName, FileS3Path.DealerInformationImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion

        #region Get Dealer Information

        /// <summary>
        /// Gets the dealer information.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DealerInformationApiUrl.GetDealerInformation)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerInformation()
        {
            return await _dealerInformationService.GetDealerInformation();
        }

        #endregion Get Dealer Information

        #region Get Dealer Branchs Information

        /// <summary>
        /// Gets the dealer branchs infomation.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DealerInformationApiUrl.GetDealerBranchInformation)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerBranchsInfomation()
        {
            return await _dealerInformationService.GetDealerBranchsInfo();
        }

        #endregion Get Dealer Branchs Information

        #region Get Dealer Branch Information Detail

        /// <summary>
        /// Gets the dealer branch infomation detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DealerInformationApiUrl.GetDealerBranchInformationDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetDealerBranchInfomationDetail([FromQuery] Guid id)
        {
            return await _dealerInformationService.GetDealerBranchInfoDetail(id);
        }

        #endregion Get Dealer Branch Information Detail

        #region Change Dealer Status

        /// <summary>
        /// Changes the dealer status.
        /// </summary>
        /// <param name="status">if set to <c>true</c> [status].</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DealerInformationApiUrl.ChangeDealerStatus)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ChangeDealerStatus([FromQuery] bool status)
        {
            return await _dealerInformationService.ChangeDealerStatus(status);
        }

        #endregion
    }
}
