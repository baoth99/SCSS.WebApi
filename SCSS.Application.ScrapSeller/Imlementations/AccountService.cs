using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.AccountModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
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

        #region Constructor

        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession) : base(unitOfWork, userAuthSession)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
        }

        #endregion


        #region Register Seller Account

        public async Task<BaseApiResponseModel> Register(AccountRegistrationModel model)
        {
            var entity = new Account()
            {
                Name = model.Name,
                UserName = model.UserName,
                Gender = model.Gender,
                DeviceId = model.DeviceId,
                Phone = model.UserName
            };

            var dictionary = CommonUtils.ObjToDictionary<AccountRegistrationModel>(model);

            //var res = await HttpClientHelper.IDHttpClientPost(IdentityServer4Route.RegisterSeller, UserAuthSession.UserSession.ClientId, dictionary);

            return BaseApiResponse.OK(dictionary);
        }

        #endregion


    }
}
