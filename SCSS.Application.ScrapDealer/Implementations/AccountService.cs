using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.AccountModels;
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

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class AccountService : BaseService, IAccountService
    {
        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The role repository
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// The locatuon repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion Repositories

        #region Services

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion Services

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="storageBlobS3Service">The storage BLOB s3 service.</param>
        public AccountService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                IStorageBlobS3Service storageBlobS3Service) : base(unitOfWork, userAuthSession, logger)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _storageBlobS3Service = storageBlobS3Service;
        }

        #endregion Constructor

        #region Register Scrap Dealer Account

        /// <summary>
        /// Registers the dealer account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> RegisterDealerAccount(DealerAccountRegisterRequestModel model)
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

            var IDServer4Route = ValidatorUtil.IsNull(model.ManageBy) ? IdentityServer4Route.RegisterDealer : IdentityServer4Route.RegisterDealerMember;

            var res = await IDHttpClientHelper.IDHttpClientPost(IDServer4Route, ClientIdConstant.DealerMobileApp, dictionary);

            if (res == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.OtherException);
            }

            // Get Role, If model.ManageBy is not null, role Key is Dealer Member, In Contrast model.ManageBy is null, role Key is Dealer
            var roleKey = ValidatorUtil.IsNull(model.ManageBy) ? AccountRole.DEALER : AccountRole.DEALER_MEMBER;

            var role = _roleRepository.GetManyAsNoTracking(x => x.Key == roleKey).FirstOrDefault();

            var accId = CommonUtils.CheckGuid(res.Data as string);

            // Create New Account
            var accountEntity = new Account()
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

            // Check Dealer is branch, If model.ManageBy is not null, this dealer is branch
            if (!ValidatorUtil.IsBlank(model.ManageBy))
            {
                accountEntity.ManagedBy = model.ManageBy;
            }
            _accountRepository.Insert(accountEntity);

            // Create Dealer Location
            var locationEntity = new Location()
            {
                Address = model.DealerAddress,
                Latitude = model.Latitude,
                Longitude = model.Longitude
            };
            _locationRepository.Insert(locationEntity);

            // Create Dealer Information
            var dealerEntity = new DealerInformation()
            {
                DealerAccountId = accountEntity.Id,
                LocationId = locationEntity.Id,
                DealerPhone = model.DealerPhone,
                DealerName = model.DealerName,
            };

            var imageFile = model.Image;
            // Check Dealer Information Image to upload
            if (imageFile != null)
            {
                var fileNameEx = imageFile.FileName;
                var fileName = CommonUtils.GetFileName(PrefixFileName.DealerInformation, fileNameEx);
                var imageUrl = await _storageBlobS3Service.UploadFile(imageFile, fileName, FileS3Path.DealerInformationImages);
                dealerEntity.DealerImageUrl = imageUrl;
            }

            _dealerInformationRepository.Insert(dealerEntity);

            // Commit to Database
            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion Register Scrap Dealer Account
    }
}
