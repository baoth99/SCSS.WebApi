using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.AccountModels;
using SCSS.AWSService.Interfaces;
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

namespace SCSS.Application.ScrapCollector.Implementations
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

        #region Services

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, IStorageBlobS3Service storageBlobS3Service) : base(unitOfWork, userAuthSession)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion


        #region Update Account Information

        public async Task<BaseApiResponseModel> UpdateAccountInformation(AccountUpdateRequestModel model)
        {
            return null;
        }


        #endregion

    }
}
