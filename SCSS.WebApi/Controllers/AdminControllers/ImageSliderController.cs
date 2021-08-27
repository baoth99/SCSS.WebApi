using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ImageSliderModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class ImageSliderController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The image slider service
        /// </summary>
        private readonly IImageSliderService _imageSliderService;

        #endregion

        #region Constructor

        public ImageSliderController(IImageSliderService imageSliderService)
        {
            _imageSliderService = imageSliderService;
        }

        #endregion

        #region Create New Image

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ImageSliderApiUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> Create([FromForm] ImageSliderCreateModel model)
        {
            return await _imageSliderService.CreateNewImage(model);
        }

        #endregion

        #region Get List Images Slider

        /// <summary>
        /// Gets the list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ImageSliderApiUrl.GetList)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetList()
        {
            return await _imageSliderService.GetImageSlider();
        }

        #endregion

        #region Change Selected Images

        /// <summary>
        /// Changes the selected images.
        /// </summary>
        /// <param name="list">The list.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ImageSliderApiUrl.ChangeItem)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ChangeSelectedImages([FromBody] List<ActiveImageSliderModel> list)
        {
            return await _imageSliderService.ChangeSelectedImages(list);
        }

        #endregion

        #region Get Images

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ImageSliderApiUrl.GetImages)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetImages()
        {
            return await _imageSliderService.GetImages();
        }

        #endregion

    }
}
