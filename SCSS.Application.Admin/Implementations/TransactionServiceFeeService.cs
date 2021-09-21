using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.TransactionServiceFeeModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class TransactionServiceFeeService : BaseService, ITransactionServiceFeeService
    {
        #region Repositories

        /// <summary>
        /// The transaction service fee percent repository
        /// </summary>
        private readonly IRepository<TransactionServiceFeePercent> _transactionServiceFeePercentRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionServiceFeeService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public TransactionServiceFeeService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _transactionServiceFeePercentRepository = unitOfWork.TransactionServiceFeePercentRepository;
        }

        #endregion

        #region Create Transaction Service Fee

        /// <summary>
        /// Creates the transaction service fee.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateTransactionServiceFee(TransactionServiceFeeCreateModel model)
        {
            var oldTransactionServiceFees = _transactionServiceFeePercentRepository.GetManyAsNoTracking(x => x.TransactionType == model.TransactionType).ToList()
                                                                                   .Select(x =>
                                                                                   {
                                                                                       x.IsActive = BooleanConstants.FALSE;
                                                                                       return x;
                                                                                   }).ToList();
            if (oldTransactionServiceFees.Any())
            {
                _transactionServiceFeePercentRepository.UpdateRange(oldTransactionServiceFees);
            }

            var entity = new TransactionServiceFeePercent()
            {
                TransactionType = model.TransactionType,
                Percent = model.Percent,
                IsActive = BooleanConstants.TRUE
            };

            _transactionServiceFeePercentRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion
    }
}
