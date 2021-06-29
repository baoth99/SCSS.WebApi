using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class DataController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The account service
        /// </summary>
        private readonly IAccountService _accountService;

        /// <summary>
        /// The category admin service
        /// </summary>
        private readonly ICategoryAdminService _categoryAdminService;

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        public DataController(IAccountService accountService, ICategoryAdminService categoryAdminService, IStorageBlobS3Service storageBlobS3Service)
        {
            _accountService = accountService;
            _categoryAdminService = categoryAdminService;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion


        #region Unit List

        /// <summary>
        /// Units the list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminDataApiUrlDefinition.Unit)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> UnitList()
        {
            return await _categoryAdminService.GetUnitList();
        }

        #endregion

        #region Role List

        /// <summary>
        /// Roles the list.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminDataApiUrlDefinition.Role)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RoleList()
        {
            return await _accountService.GetRoleList();
        }

        #endregion

        #region Image

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="image">The image.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminDataApiUrlDefinition.Image)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetImage(string image)
        {
            var res = await _storageBlobS3Service.GetImage(image);
            return res;
        }

        #endregion
    }
}
