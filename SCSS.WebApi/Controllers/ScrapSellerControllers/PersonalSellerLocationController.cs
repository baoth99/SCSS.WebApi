using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.PersonalSellerLocationModel;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class PersonalSellerLocationController : BaseScrapSellerControllers
    {
        #region Services

        /// <summary>
        /// The personal seller location service
        /// </summary>
        private readonly IPersonalSellerLocationService _personalSellerLocationService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PersonalSellerLocationController"/> class.
        /// </summary>
        /// <param name="personalSellerLocationService">The personal seller location service.</param>
        public PersonalSellerLocationController(IPersonalSellerLocationService personalSellerLocationService)
        {
            _personalSellerLocationService = personalSellerLocationService;
        }

        #endregion

        #region Get Personal Location

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.PersonalSellerLocationApiUrl.GetLocation)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetLocations()
        {
            return await _personalSellerLocationService.GetLocations();
        }

        #endregion

        #region Add Personal Location

        /// <summary>
        /// Removes the location.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.PersonalSellerLocationApiUrl.AddLocation)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> AddLocation([FromBody] PersonalSellerLocationCreateModel model)
        {
            return await _personalSellerLocationService.AddPersonalLocation(model);
        }

        #endregion

        #region Remove Personal Location

        /// <summary>
        /// Removes the location.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.PersonalSellerLocationApiUrl.RemoveLocation)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RemoveLocation([FromQuery] Guid id)
        {
            return await _personalSellerLocationService.RemoveLocation(id);
        }

        #endregion
    }
}
