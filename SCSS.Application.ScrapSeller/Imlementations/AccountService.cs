using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.AccountModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public class AccountService : BaseService, IAccountService
    {

        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The role repository
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        #endregion

        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession) : base(unitOfWork, userAuthSession)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
        }


        #region Register Seller Account

        public async Task<BaseApiResponseModel> Register(AccountRegistrationModel model)
        {
            return null;
        }

        #endregion


    }
}
