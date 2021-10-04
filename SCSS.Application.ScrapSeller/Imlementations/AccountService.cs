using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.AccountModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
        }

        #endregion

        #region Register Seller Account

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> Register(SellerAccountRegistrationModel model)
        {
            var dictionary = new Dictionary<string, string>()
            {
                {"name", model.Name },
                {"password", model.Password },
                {"email", string.Empty },
                {"gender", model.Gender.ToString() },
                {"phone", model.UserName },
                {"address", string.Empty },
                {"birthdate", string.Empty },
                {"image", string.Empty },
                {"idcard", string.Empty },
                {"registertoken", model.RegisterToken }
            };

            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.RegisterSeller, ClientIdConstant.SellerMobileApp, dictionary);

            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            var role = _roleRepository.GetManyAsNoTracking(x => x.Key == AccountRole.SELLER).FirstOrDefault();

            var accId = CommonUtils.CheckGuid(res.Data as string);

            var entity = new Account()
            {
                Id = accId.Value,
                Name = model.Name,
                UserName = model.UserName,
                Gender = model.Gender,
                DeviceId = model.DeviceId,
                Phone = model.UserName,
                RoleId = role.Id,
                Status = AccountStatus.ACTIVE,
                TotalPoint = DefaultConstant.TotalPoint
            };

            _accountRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Update DeviceId

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateDeviceId(DeviceIdUpdateModel model)
        {
            var entity = _accountRepository.GetById(UserAuthSession.UserSession.Id);

            if (entity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            entity.DeviceId = model.DeviceId;

            _accountRepository.Update(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Update Account

        /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateAccount(SellerAccountUpdateProfileModel model)
        {
            var id = UserAuthSession.UserSession.Id;
            var entity = _accountRepository.GetById(id);
            if (entity == null)
            {
                return BaseApiResponse.NotFound();
            }

            // Send Data to IdentityServer4
            var dictionary = new Dictionary<string, string>()
            {
                {"id", id.ToString() },
                {"name", model.Name },
                {"email", model.Email },
                {"gender", model.Gender.ToString() },
                {"address", model.Address },
                {"bithdate", model.BirthDate },
                {"image", model.ImageUrl},
                {"idcard", model.IDCard }
            };

            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.Update, UserAuthSession.UserSession.ClientId, dictionary);

            // Check Response
            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }
            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.Gender = model.Gender;
            entity.BirthDate = model.BirthDate.ToDateTime();
            entity.Address = model.Address;
            entity.ImageUrl = model.ImageUrl;
            entity.IdCard = model.IDCard;
            entity.DeviceId = model.DeviceID;

            _accountRepository.Update(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Get Seller Account Info

        /// <summary>
        /// Gets the seller account information.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetSellerAccountInfo()
        {
            var dealerAccountId = UserAuthSession.UserSession.Id;
            if (!_accountRepository.IsExisted(x => x.Id.Equals(dealerAccountId)))
            {
                return null;
            }

            var accountInfo = await _accountRepository.GetManyAsNoTracking(x => x.Id.Equals(dealerAccountId))
                                            .Join(_roleRepository.GetAllAsNoTracking(), x => x.RoleId, y => y.Id,
                                                                                             (x, y) => new SellerAccountInfoDetailViewModel
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
                                                                                             }).FirstOrDefaultAsync();
            return BaseApiResponse.OK(accountInfo);
        }

        #endregion
    }
}
