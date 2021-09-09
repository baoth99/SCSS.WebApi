using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class ImageSliderController : BaseScrapSellerControllers
    {
        #region Services

        /// <summary>
        /// The image slider service
        /// </summary>
        private readonly IImageSliderService _imageSliderService;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageSliderController"/> class.
        /// </summary>
        /// <param name="imageSliderService">The image slider service.</param>
        public ImageSliderController(IImageSliderService imageSliderService)
        {
            _imageSliderService = imageSliderService;
        }

        #endregion

        #region Seller App Gets Image Slider 

        /// <summary>
        /// Gets the images.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.ImageSliderApiUrl.GetImageSlider)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetImageSlider()
        {
            return await _imageSliderService.GetImages();
        }

        #endregion
    }
}
