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
    public class TransactionServiceFeeController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The transaction service fee service
        /// </summary>
        private readonly ITransactionServiceFeeService _transactionServiceFeeService;

        #endregion

        #region Constructor

        public TransactionServiceFeeController(ITransactionServiceFeeService transactionServiceFeeService)
        {
            _transactionServiceFeeService = transactionServiceFeeService;
        }

        #endregion
    }
}
