using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ImageSliderModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
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
    }
}
