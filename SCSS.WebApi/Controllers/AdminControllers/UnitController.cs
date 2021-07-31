using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.UnitModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class UnitController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The unit service
        /// </summary>
        private readonly IUnitService _unitService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitController"/> class.
        /// </summary>
        /// <param name="unitService">The unit service.</param>
        public UnitController(IUnitService unitService)
        {
            _unitService = unitService;
        }

        #endregion

        #region Search Unit

        /// <summary>
        /// Searches the unit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.UnitApiUrl.Search)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchUnit([FromQuery] SearchUnitModel model)
        {
            return await _unitService.SearchUnit(model);
        }

        #endregion

        #region Create Unit

        /// <summary>
        /// Creates the unit.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.UnitApiUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateUnit([FromBody] CreateUnitModel model)
        {
            return await _unitService.CreateUnit(model);
        }

        #endregion

        #region Edit Unit

        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.UnitApiUrl.Edit)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> EditUnit([FromBody] EditUnitModel model)
        {
            return await _unitService.UpdateUnit(model);
        }
        #endregion

        #region Remove Unit

        [HttpDelete]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.UnitApiUrl.Remove)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> RemoveUnit([FromQuery] Guid id)
        {
            return await _unitService.RemoveUnit(id);
        }

        #endregion
    }
}
