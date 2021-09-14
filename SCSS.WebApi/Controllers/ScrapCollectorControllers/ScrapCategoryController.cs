using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Models;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.ScrapCategoryModels;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class ScrapCategoryController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The scrap category service
        /// </summary>
        private readonly IScrapCategoryService _scrapCategoryService;

        /// <summary>
        /// The storage BLOB service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapCategoryController"/> class.
        /// </summary>
        /// <param name="scrapCategoryService">The scrap category service.</param>
        /// <param name="storageBlobService">The storage BLOB service.</param>
        public ScrapCategoryController(IScrapCategoryService scrapCategoryService, IStorageBlobS3Service storageBlobService)
        {
            _scrapCategoryService = scrapCategoryService;
            _storageBlobService = storageBlobService;
        }

        #endregion

        #region Upload Scrap Category Image

        /// <summary>
        /// Uploads the scrap category image.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.UploadImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UploadScrapCategoryImage([FromForm] ImageUploadModel model)
        {
            var fileName = CommonUtils.GetFileName(PrefixFileName.ScrapCategory, model.Image.FileName);
            var imageUrl = await _storageBlobService.UploadFile(model.Image, fileName, FileS3Path.ScrapCategoryImages);
            return BaseApiResponse.OK(imageUrl);
        }

        #endregion

        #region Get Scrap Category Image

        /// <summary>
        /// Gets the scrap category image.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.GetImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetScrapCategoryImage([FromQuery] string imageUrl)
        {
            var file = await _storageBlobService.GetFile(imageUrl);
            if (file == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }
            var image = file.Stream.ToBitmap();
            return BaseApiResponse.OK(image);
        }

        #endregion

        #region Check Duplicate Scrap Category Name

        /// <summary>
        /// Checks the name of the scrap category.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.CheckName)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CheckScrapCategoryName([FromQuery] string name)
        {
            return await _scrapCategoryService.CheckScrapCategoryName(name);
        }

        #endregion

        #region Create New Scrap Category

        /// <summary>
        /// Creates the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateScrapCategory([FromBody] ScrapCategoryCreateModel model)
        {
            return await _scrapCategoryService.CreateScrapCategory(model);
        }

        #endregion

        #region Update Scrap Category 

        /// <summary>
        /// Updates the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.Update)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UpdateScrapCategory([FromBody] ScrapCategoryUpdateModel model)
        {
            return await _scrapCategoryService.UpdateScrapCategory(model);
        }

        #endregion

        #region Get Scrap Categories

        /// <summary>
        /// Gets the scrap categories.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.Get)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetScrapCategories()
        {
            return await _scrapCategoryService.GetScrapCategories();
        }

        #endregion

        #region Get Scrap Category Detail

        /// <summary>
        /// Gets the scrap category detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.GetDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetScrapCategoryDetail([FromQuery] Guid id)
        {
            return await _scrapCategoryService.GetScrapCategoryDetail(id);
        }

        #endregion

        #region Remove Scrap Category

        /// <summary>
        /// Removes the scrap category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ScrapCategoryUrl.Remove)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RemoveScrapCategory([FromQuery] Guid id)
        {
            return await _scrapCategoryService.RemoveScrapCategory(id);
        }

        #endregion
    }
}
