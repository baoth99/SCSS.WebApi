using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public TransactionAwardAmountController(ITransactionAwardAmountService transactionAwardAmountService)
        {
            _transactionAwardAmountService = transactionAwardAmountService;
        }

        #endregion
    }
}
