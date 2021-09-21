using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.AccountModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService"></param>
        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
        }

        #endregion

        #region Search Account

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> Search(SearchAccountRequestModel model)
        {
            var dataQuery = _accountRepository.GetManyAsNoTracking(x => (x.Status == AccountStatus.ACTIVE || x.Status == AccountStatus.BANNING));

            var data = dataQuery.Where(x => (ValidatorUtil.IsBlank(model.Name) || x.Name.Contains(model.Name)) &&
                                            (ValidatorUtil.IsBlank(model.Phone) || x.Phone.Contains(model.Phone)) &&
                                            (ValidatorUtil.IsBlank(model.Email) || x.Email.Contains(model.Email)) &&
                                            (ValidatorUtil.IsBlank(model.Address) || x.Address.Contains(model.Address)) &&
                                            (model.Status == CommonConstants.Zero || x.Status == model.Status) &&
                                            (model.Gender == CommonConstants.Zero || x.Gender == model.Gender) &&
                                            (ValidatorUtil.IsBlank(model.IdCard) | x.IdCard.Contains(model.IdCard)))
                                            .Join(_roleRepository.GetManyAsNoTracking(x => (model.Role == CommonConstants.Zero || x.Key == model.Role)), x => x.RoleId, y => y.Id,
                                            (x, y) => new
                                            {
                                                x.Id,
                                                x.Name,
                                                x.Phone,
                                                x.Gender,
                                                x.Status,
                                                Role = y.Key,
                                                x.TotalPoint,
                                                x.CreatedTime
                                            }).Where(x => x.Role != AccountRole.ADMIN).OrderBy(DefaultSort.CreatedTimeDESC);


            var totalRecord = await data.CountAsync();

            var dataRes = data.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new AccountViewResponseModel()
            {
                Id = x.Id,
                Gender = x.Gender,
                Name = x.Name,
                Phone = x.Phone,
                Role = x.Role,
                Status = x.Status,
                TotalPoint = x.TotalPoint
            }).ToList();


            return BaseApiResponse.OK(resData: dataRes, totalRecord: totalRecord);
        }

        #endregion

        #region Get Account Detail

        /// <summary>
        /// Gets the account detail.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public async Task<AccountDetailViewModel> GetAccountDetail(Guid Id)
        {
            if (!_accountRepository.IsExisted(x => x.Id.Equals(Id)))
            {
                return null;
            }

            var accountInfo = await _accountRepository.GetManyAsNoTracking(x => x.Id.Equals(Id))
                                            .Join(_roleRepository.GetAllAsNoTracking(), x => x.RoleId, y => y.Id,
                                                                                             (x, y) => new AccountDetailViewModel
                                                                                             {
                                                                                                 Id = x.Id,
                                                                                                 UserName = x.UserName,
                                                                                                 Address = StringUtils.GetString(x.Address),
                                                                                                 BirthDate = x.BirthDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                                                                                                 CreatedTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                                                                                                 Email = StringUtils.GetString(x.Email),
                                                                                                 Gender = x.Gender,
                                                                                                 IdCard = StringUtils.GetString(x.IdCard),
                                                                                                 Image = x.ImageUrl,
                                                                                                 Name = StringUtils.GetString(x.Name),
                                                                                                 Phone = x.Phone,
                                                                                                 RoleKey = y.Key,
                                                                                                 RoleName = y.Name,
                                                                                                 Status = x.Status,
                                                                                                 TotalPoint = x.TotalPoint,
                                                                                                 DeviceId = x.DeviceId
                                                                                             }).FirstOrDefaultAsync();
            return accountInfo;

        }

        #endregion

        #region Change Status

        /// <summary>
        /// Changes the status.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <param name="Status">The status.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ChangeStatus(AccountStatusRequestModel model)
        {
            if (!_accountRepository.IsExisted(x => x.Id.Equals(model.Id)))
            {
                return BaseApiResponse.NotFound();
            }

            var dictionary = new Dictionary<string, string>()
            {
                {"id", model.Id.ToString() },
                {"status", model.Status.ToString() }
            };

            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.ChangStatus,UserAuthSession.UserSession.ClientId, dictionary);

            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            var account = _accountRepository.GetById(model.Id);
            account.Status = model.Status;

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
