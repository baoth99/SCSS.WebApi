using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.AdminCategoryModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class CategoryAdminController : BaseAdminController
    {

        #region Services

        /// <summary>
        /// The category admin service
        /// </summary>
        private readonly ICategoryAdminService _categoryAdminService;

        #endregion

        #region Constructor

        public CategoryAdminController(ICategoryAdminService categoryAdminService)
        {
            _categoryAdminService = categoryAdminService;
        }

        #endregion

        #region Seach Category Admin

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminCategoryApiUrlDefinition.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchCategoryAdmins([FromQuery] SearchCategoryAdminModel model)
        {
            return await _categoryAdminService.SearchCategoryAdmin(model);
        }

        #endregion

        #region Get Category Admin Detail

        /// <summary>
        /// Gets the category admin detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminCategoryApiUrlDefinition.Detail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCategoryAdminDetail([FromQuery] Guid id)
        {
            return await _categoryAdminService.GetCategoryAdminDetail(id);
        }

        #endregion

        #region Create Category Admin 

        /// <summary>
        /// Creates the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminCategoryApiUrlDefinition.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateCategoryAdmin([FromForm] CreateCategoryAdminModel model)
        {
            return await _categoryAdminService.CreateCategoryAdmin(model);
        }

        #endregion

        #region Edit Category Admin

        /// <summary>
        /// Edits the specified model.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminCategoryApiUrlDefinition.Edit)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> EditCategoryAdmin([FromForm] CategoryAdminEditModel model)
        {
            return await _categoryAdminService.EditCategoryAdmin(model);
        }

        #endregion

        #region Remove Category Admin

        /// <summary>
        /// Removes the specified model.
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminCategoryApiUrlDefinition.Remove)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RemoveCategoryAdmin([FromQuery] Guid Id)
        {
            return await _categoryAdminService.RemoveCategoryAdmin(Id);
        }

        #endregion
    }
}
