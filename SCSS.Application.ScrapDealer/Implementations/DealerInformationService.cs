using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class DealerInformationService : BaseService, IDealerInformationService
    {
        #region Repositories

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Constructor

        public DealerInformationService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _locationRepository = unitOfWork.LocationRepository;
        }

        #endregion

        #region Get Dealer Information Detail

        public async Task<BaseApiResponseModel> GetDealerInformation(Guid id)
        {
            var dealerInformation = _dealerInformationRepository.GetAsyncAsNoTracking(x => x.Id.Equals(id) && x.DealerAccountId.Equals(UserAuthSession.UserSession.Id));
            if (dealerInformation == null)
            {
                return BaseApiResponse.NotFound();
            }



            return BaseApiResponse.OK();
        }

        #endregion


    }
}
