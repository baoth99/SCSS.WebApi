using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.TransactionAwardAmountModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class TransactionAwardAmountController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The transaction award amount service
        /// </summary>
        private readonly ITransactionAwardAmountService _transactionAwardAmountService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionAwardAmountController"/> class.
        /// </summary>
        /// <param name="transactionAwardAmountService">The transaction award amount service.</param>
        public TransactionAwardAmountController(ITransactionAwardAmountService transactionAwardAmountService)
        {
            _transactionAwardAmountService = transactionAwardAmountService;
        }

        #endregion


        #region Create Transaction Award Amount

        /// <summary>
        /// Creates the transaction award amount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.TransactionAwardAmountApiUrl.Create)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateTransactionAwardAmount([FromBody] TransactionAwardAmountCreateModel model)
        {
            return await _transactionAwardAmountService.CreateTransactionAwardAmount(model);
        }

        #endregion
    }
}
