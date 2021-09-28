using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.SystemConfigModels;
using SCSS.Application.Admin.Models.TransactionAwardAmountModels;
using SCSS.Application.Admin.Models.TransactionServiceFeeModels;
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
    public class SystemConfigController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The system configuration service
        /// </summary>
        private readonly ISystemConfigService _systemConfigService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemConfigController"/> class.
        /// </summary>
        /// <param name="systemConfigService">The system configuration service.</param>
        public SystemConfigController(ISystemConfigService systemConfigService)
        {
            _systemConfigService = systemConfigService;
        }

        #endregion

        #region Get System Config Information

        /// <summary>
        /// Gets the system configuration information.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.SystemConfigApiUrl.SystemConfigInfo)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetSystemConfigInfo()
        {
            return await _systemConfigService.GetSystemConfigInfo();
        }

        #endregion

        #region Modify System Configuration

        /// <summary>
        /// Modifies the system configuration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.SystemConfigApiUrl.ModifySystemConfig)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ModifySystemConfig([FromBody] SystemConfigModifyModel model)
        {
            return await _systemConfigService.ModifySystemConfig(model);
        }

        #endregion

        #region Modify Transaction Award Amount

        /// <summary>
        /// Modifies the transaction award amount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.SystemConfigApiUrl.ModifyTransactionAward)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ModifyTransactionAwardAmount([FromBody] TransactionAwardAmountModifyModel model)
        {
            return await _systemConfigService.ModifyTransactionAwardAmount(model);
        }

        #endregion

        #region Get Transaction Award Amount is using

        /// <summary>
        /// Gets the transaction award amount.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.SystemConfigApiUrl.GetTransactionAward)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetTransactionAwardAmount()
        {
            return await _systemConfigService.GetTransactionAwardAmount();
        }

        #endregion

        #region Modify Transaction Service Fee 

        /// <summary>
        /// Modifies the transaction service fee.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.SystemConfigApiUrl.ModifyTransactionFee)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ModifyTransactionServiceFee([FromBody] TransactionServiceFeeModifyModel model)
        {
            return await _systemConfigService.ModifyTransactionServiceFee(model);
        }

        #endregion

        #region Get Transaction Service Fee is using

        /// <summary>
        /// Gets the transaction service fee.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.SystemConfigApiUrl. GetTransactionFee)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetTransactionServiceFee()
        {
            return await _systemConfigService.GetTransactionServiceFee();
        }

        #endregion

    }
}
