using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class DataController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        /// <summary>
        /// The scrap category service
        /// </summary>
        private readonly ICollectDealTransactionService _collectDealTransactionService;

        #endregion

        #region Constructor

        public DataController(IStorageBlobS3Service storageBlobS3Service, ICollectDealTransactionService collectDealTransactionService)
        {
            _storageBlobS3Service = storageBlobS3Service;
            _collectDealTransactionService = collectDealTransactionService;
        }

        #endregion

        #region Get Image

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DataApiUrl.GetImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<IActionResult> GetImage([FromQuery] string imageUrl)
        {
            var file = await _storageBlobS3Service.GetFile(imageUrl);

            if (file == null)
            {
                return new NotFoundResult();
            }

            var image = file.Stream.ToByteArray();
            return File(image, CommonUtils.GetContentImageTypeString(file.Extension));
        }

        #endregion

        #region Get Transaction Scrap Categories

        /// <summary>
        /// Gets the transaction scrap categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DataApiUrl.TransScrapCategories)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetTransactionScrapCategories()
        {
            return await _collectDealTransactionService.GetTransactionScrapCategories();
        }

        #endregion

        #region Get Transaction Scrap Category Detail

        /// <summary>
        /// Gets the transaction scrap category detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DataApiUrl.TransSCDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetTransactionScrapCategoryDetail([FromQuery] Guid id)
        {
            return await _collectDealTransactionService.GetTransactionScrapCategoryDetail(id);
        }

        #endregion

        #region Auto Complete Collector Phone

        /// <summary>
        /// Automatics the complete collector phone.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DataApiUrl.AutoCompleteCollectorPhone)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> AutoCompleteCollectorPhone() 
        {
            return await _collectDealTransactionService.AutoCompleteCollectorPhone();
        }

        #endregion
    }
}
