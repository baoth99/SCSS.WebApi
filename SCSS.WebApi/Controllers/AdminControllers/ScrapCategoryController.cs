using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ScrapCategoryModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class ScrapCategoryController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The scrap category service
        /// </summary>
        private readonly IScrapCategoryService _scrapCategoryService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapCategoryController"/> class.
        /// </summary>
        /// <param name="scrapCategoryService">The scrap category service.</param>
        public ScrapCategoryController(IScrapCategoryService scrapCategoryService)
        {
            _scrapCategoryService = scrapCategoryService;            
        }

        #endregion

        #region Search Scrap Category

        /// <summary>
        /// Searches the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ScrapCategoryApiUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchScrapCategory([FromQuery] ScrapCategorySearchModel model)
        {
            return await _scrapCategoryService.SearchScrapCategory(model);
        }

        #endregion

        #region Get Scrap Category Detail

        /// <summary>
        /// Gets the scrap category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ScrapCategoryApiUrl.Detail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetScrapCategory([FromQuery] Guid id)
        {
            return await _scrapCategoryService.GetScrapCategory(id);
        }

        #endregion

    }
}
