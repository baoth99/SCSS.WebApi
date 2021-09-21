using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
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
    public class TransactionServiceFeeController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The transaction service fee service
        /// </summary>
        private readonly ITransactionServiceFeeService _transactionServiceFeeService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionServiceFeeController"/> class.
        /// </summary>
        /// <param name="transactionServiceFeeService">The transaction service fee service.</param>
        public TransactionServiceFeeController(ITransactionServiceFeeService transactionServiceFeeService)
        {
            _transactionServiceFeeService = transactionServiceFeeService;
        }

        #endregion

        #region Create Transaction Service Fee

        /// <summary>
        /// Creates the transaction service fee.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.TransactionServiceFeeApiUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateTransactionServiceFee([FromBody] TransactionServiceFeeCreateModel model)
        {
            return await _transactionServiceFeeService.CreateTransactionServiceFee(model);
        }

        #endregion
    }
}
