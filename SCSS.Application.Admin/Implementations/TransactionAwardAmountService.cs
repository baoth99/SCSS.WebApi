using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.TransactionAwardAmountModels;
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
    public class TransactionAwardAmountService : BaseService, ITransactionAwardAmountService
    {
        #region Repositories

        /// <summary>
        /// The transaction award amount repository
        /// </summary>
        private readonly IRepository<TransactionAwardAmount> _transactionAwardAmountRepository;

        #endregion

        #region Constructor

        public TransactionAwardAmountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _transactionAwardAmountRepository = unitOfWork.TransactionAwardAmountRepository;
        }

        #endregion

        #region Create Transaction Award Amount

        /// <summary>
        /// Creates the transaction award amount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateTransactionAwardAmount(TransactionAwardAmountCreateModel model)
        {
            // De-Active all of old TransactionAward
            var oldAwardAmounts = _transactionAwardAmountRepository.GetManyAsNoTracking(x => x.AppliedObject == model.AppliedObject).ToList()
                                                                   .Select(c => 
                                                                   { 
                                                                       c.IsActive = BooleanConstants.FALSE; 
                                                                       return c; 
                                                                   }).ToList();

            if (oldAwardAmounts.Any())
            {
                _transactionAwardAmountRepository.UpdateRange(oldAwardAmounts);
            }

            // Create new TransactionAwardAmount
            var entity = new TransactionAwardAmount()
            {
                AppliedObject = model.AppliedObject,
                AppliedAmount = model.AppliedAmount,
                Amount = model.Amount,
                IsActive = BooleanConstants.TRUE
            };
            _transactionAwardAmountRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
