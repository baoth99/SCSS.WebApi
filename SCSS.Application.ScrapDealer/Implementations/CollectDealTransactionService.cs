using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class CollectDealTransactionService : BaseService, ICollectDealTransactionService
    {
        #region Services

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        #endregion

        #region Constructor


        public CollectDealTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, fcmService, cacheService)
        {
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
        }

        #endregion

    }
}
