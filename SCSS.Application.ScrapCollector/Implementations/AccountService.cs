using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.AccountModels;
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

        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, IStorageBlobS3Service storageBlobS3Service, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion

        #region Register Collector Account

        /// <summary>
        /// Registers the collector account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RegisterCollectorAccount(CollectorAccountRegisterRequestModel model)
        {
            var dictionary = new Dictionary<string, string>()
            {
                {"name", model.Name },
                {"password", model.Password },
                {"email", string.Empty },
                {"gender", model.Gender.ToString() },
                {"phone", model.Phone },
                {"address", model.Address },
                {"birthdate", model.BirthDate },
                {"image", string.Empty },
                {"idcard", model.IDCard },
                {"registertoken", model.RegisterToken }
            };

            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.RegisterCollector, ClientIdConstant.CollectorMobileApp, dictionary);

            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            var role = _roleRepository.GetManyAsNoTracking(x => x.Key == AccountRole.COLLECTOR).FirstOrDefault();

            var accId = CommonUtils.CheckGuid(res.Data as string);

            var entity = new Account()
            {
                Id = accId.Value,
                Name = model.Name,
                UserName = model.Phone,
                Gender = model.Gender,
                DeviceId = model.DeviceId,
                Phone = model.Phone,
                BirthDate = model.BirthDate.ToDateTime(),
                RoleId = role.Id,
                Address = model.Address,
                IdCard = model.IDCard,
                Status = AccountStatus.NOT_APPROVED,
                TotalPoint = DefaultConstant.TotalPoint
            };

            _accountRepository.Insert(entity);

            await UnitOfWork.CommitAsync();
            Logger.LogInfo(AccountRegistrationLoggerMessage.RegistrationSuccess(model.Phone, AccountRoleConstants.COLLECTOR));

            return BaseApiResponse.OK();
        }

        #endregion

        #region Update Account Information

        /// <summary>
        /// Updates the account information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateAccountInformation(CollectorAccountUpdateRequestModel model)
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
                {"email", model.Email },
                {"name", model.Name },
                {"gender", model.Gender.ToString() },
                {"address", entity.Address },
                {"bithdate", model.BirthDate },
                {"image", model.ImageUrl},
                {"idcard", entity.IdCard },
            };
            // Update in ID4
            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.Update, ClientIdConstant.CollectorMobileApp, dictionary);

            // Check Response from ID4
            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.Gender = model.Gender;
            entity.BirthDate = model.BirthDate.ToDateTime();
            entity.ImageUrl = model.ImageUrl;

            _accountRepository.Update(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

        #region Update DeviceId

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateDeviceId(string deviceId)
        {

            var entity = _accountRepository.GetById(UserAuthSession.UserSession.Id);

            if (entity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            entity.DeviceId = deviceId;

            _accountRepository.Update(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion
    }
}
