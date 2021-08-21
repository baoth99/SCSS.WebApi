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
using System;
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

        #region Register Seller Account

        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> Register(AccountRegistrationModel model)
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

            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.RegisterSeller, UserAuthSession.UserSession.ClientId, dictionary);

            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            var role = _roleRepository.GetManyAsNoTracking(x => x.Key == AccountRole.SELLER).FirstOrDefault();

            var entity = new Account()
            {
                Name = model.Name,
                UserName = model.UserName,
                Gender = model.Gender,
                DeviceId = model.DeviceId,
                Phone = model.UserName,
                RoleId = role.Id,
                Status = AccountStatus.ACTIVE
            };

            _accountRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK(entity);
        }

        #endregion

        #region Update DeviceId

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> UpdateDeviceId(AccountUpdateDeviceIdModel model)
        {
            var entity = _accountRepository.GetById(model.Id);

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
        public async Task<BaseApiResponseModel> UpdateAccount(AccountUpdateProfileModel model)
        {
            var entity = _accountRepository.GetById(model.Id);
            if (entity == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            var imageName = string.Empty;

            // Get Image Name
            if (model.Image != null)
            {
                imageName = CommonUtils.GetFileName(PrefixFileName.SellerAccount, model.Image.FileName);
            }

            if (model.IsDeleteImg)
            {
                imageName = string.Empty;
            }

            // Send Data to IdentityServer4

            var dictionary = new Dictionary<string, string>()
            {
                {"id", model.Id.ToString() },
                {"name", model.Name },
                {"email", model.Email },
                {"gender", model.Gender.ToString() },
                {"address", model.Address },
                {"bithdate", model.BirthDate },
                {"image", imageName == string.Empty ? string.Empty : $"{FileS3Path.SellerAccountImages}/{imageName}" },
                {"idcard", model.IDCard }
            };

            var res = await IDHttpClientHelper.IDHttpClientPost(IdentityServer4Route.Update, UserAuthSession.UserSession.ClientId, dictionary);

            // Check Response
            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            var imageUrl = string.Empty;

            if (model.Image != null)
            {
                imageUrl = await _storageBlobS3Service.UploadFile(model.Image, imageName, FileS3Path.SellerAccountImages);
            }

            entity.Name = model.Name;
            entity.Email = model.Email;
            entity.Gender = model.Gender;
            entity.BirthDate = model.BirthDate.ToDateTime();
            entity.Address = model.Address;
            entity.ImageUrl = imageUrl;
            entity.IdCard = model.IDCard;
            entity.DeviceId = model.DeviceID;

            _accountRepository.Update(entity);

            await UnitOfWork.CommitAsync();


            return BaseApiResponse.OK();
        }

        #endregion
    }
}
