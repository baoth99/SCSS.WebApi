using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.AccountModels;
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

namespace SCSS.Application.Admin.Implementations
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

        #region Search Account



        #endregion

        #region Get Account Detail

        /// <summary>
        /// Gets the account detail.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetAccountDetail(Guid Id)
        {
            if (!_accountRepository.IsExisted(x => x.Id.Equals(Id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            var account = await _accountRepository.GetManyAsNoTracking(x => x.Id.Equals(Id))
                                            .Join(_roleRepository.GetAllAsNoTracking(), x => x.RoleId, y => y.Id,
                                                                                             (x, y) => new AccountDetailViewModel
                                                                                             {
                                                                                                 Id = x.Id,
                                                                                                 UserName = x.UserName,
                                                                                                 Address = x.Address,
                                                                                                 BirthDate = x.BirthDate,
                                                                                                 CreatedTime = x.CreatedTime,
                                                                                                 Email = x.Email,
                                                                                                 Gender = x.Gender,
                                                                                                 IdCard = x.IdCard,
                                                                                                 Image = x.ImageUrl,
                                                                                                 Name = x.Name,
                                                                                                 Phone = x.Phone,
                                                                                                 RoleKey = y.Key,
                                                                                                 RoleName = y.Name,
                                                                                                 Status = x.Status,
                                                                                                 TotalPoint = x.TotalPoint
                                                                                             }).FirstOrDefaultAsync();
            return BaseApiResponse.OK(account);

        }

        #endregion

        #region Change Status

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ChangeStatus(Guid Id, int? Status)
        {
            if (!_accountRepository.IsExisted(x => x.Id.Equals(Id)))
            {
                return BaseApiResponse.NotFound();
            }
            var dictionary = new Dictionary<string, string>()
            {
                {"id", Id.ToString() },
                {"status", Status.ToString() }
            };

            var res = await HttpClientHelper.IDHttpClientPost(IdentityServer4Route.ChangStatus,UserAuthSession.UserSession.ClientId, dictionary);

            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            var account = _accountRepository.GetById(Id);
            account.Status = Status;

            _accountRepository.Update(account);

            await UnitOfWork.CommitAsync();


            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Role List

        public async Task<BaseApiResponseModel> GetRoleList()
        {
            var data = await _roleRepository.GetAllAsNoTracking().Select(x => new RoleViewModel()
            {
                Key = x.Key,
                Val = x.Name
            }).ToListAsync();

            return BaseApiResponse.OK(data);
        }

        #endregion

    }
}
