using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.RequestRegisterModels;
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
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class RequestRegisterService : BaseService, IRequestRegisterService
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

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        #endregion


        #region Contructor

        public RequestRegisterService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _accountRepository = UnitOfWork.AccountRepository;
            _roleRepository = UnitOfWork.RoleRepository;
            _dealerInformationRepository = UnitOfWork.DealerInformationRepository;
        }

        #endregion

        #region Search Collector Request Register

        /// <summary>
        /// Searches the collector request register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchCollectorRequestRegister(CollectorRequestRegisterSearchModel model)
        {
            var role = _roleRepository.Get(x => x.Key.Equals(AccountRole.COLLECTOR));
            var data = _accountRepository.GetManyAsNoTracking(x => (x.RoleId == role.Id) &&
                                                                    (x.Status == AccountStatus.NOT_APPROVED || x.Status == AccountStatus.BANNING));
            var dataQuery = data.Where(x => (ValidatorUtil.IsBlank(model.Phone) || x.Phone.Contains(model.Phone)) &&
                                            (ValidatorUtil.IsBlank(model.Name) || x.Name.Contains(model.Name)) &&
                                            (model.Status == CommonConstants.Zero || x.Status == model.Status)).Select(x => new
                                            {
                                                x.Id,
                                                x.Phone,
                                                x.Name,
                                                x.Gender,
                                                x.CreatedTime,
                                                x.Status
                                            }).OrderBy(DefaultSort.CreatedTimeDESC);

            var totalRecord = await dataQuery.CountAsync();

            var dataRes = dataQuery.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new CollectorRequestRegisterViewModel()
            {
                Id = x.Id,
                Gender = x.Gender,
                Name = x.Name,
                Phone = x.Phone,
                RegisterTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                Status = x.Status
            }).ToList();


            return BaseApiResponse.OK(resData: dataRes, totalRecord: totalRecord);
        }

        #endregion

        #region Search Dealer Request Register

        /// <summary>
        /// Searches the dealer request register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchDealerRequestRegister(DealerRequestRegisterSearchModel model)
        {
            var role = _roleRepository.Get(x => x.Key.Equals(AccountRole.DEALER));

            var data = _accountRepository.GetManyAsNoTracking(x => (x.RoleId == role.Id) &&
                                                                    (x.Status == AccountStatus.NOT_APPROVED || x.Status == AccountStatus.BANNING));

            var dataQuery = data.Where(x => (ValidatorUtil.IsBlank(model.Phone) || x.Phone.Contains(model.Phone)) &&
                                            (ValidatorUtil.IsBlank(model.Name) || x.Name.Contains(model.Name)) &&
                                            (model.Status == CommonConstants.Zero || x.Status == model.Status))
                                            .Join(_dealerInformationRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.DealerName) || x.DealerName.Contains(model.DealerName))),
                                                    x => x.Id, y => y.DealerAccountId, (x, y) => new
                                                    {
                                                        x.Id,
                                                        x.Phone,
                                                        y.DealerName,
                                                        x.Name,
                                                        x.Gender,
                                                        x.CreatedTime,
                                                        x.Status,
                                                        x.ManagedBy
                                                    }).GroupJoin(_dealerInformationRepository.GetAllAsNoTracking(),
                                                        x => x.ManagedBy, y => y.DealerAccountId, (x, y) => new
                                                        {
                                                            x.Id,
                                                            x.Phone,
                                                            x.DealerName,
                                                            AccountName = x.Name,
                                                            x.Gender,
                                                            x.CreatedTime,
                                                            x.Status,
                                                            DealerInfo = y
                                                        }).SelectMany(x => x.DealerInfo.DefaultIfEmpty(), (x, y) => new
                                                        {
                                                            x.Id,
                                                            x.Phone,
                                                            x.DealerName,
                                                            x.AccountName,
                                                            x.Gender,
                                                            x.CreatedTime,
                                                            x.Status,
                                                            ManagedBy = y.DealerName
                                                        }).OrderBy(DefaultSort.CreatedTimeDESC);

            var totalRecord = await dataQuery.CountAsync();

            var dataRes = dataQuery.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new DealerRequestRegisterViewModel()
            {
                Id = x.Id,
                Gender = x.Gender,
                Name = x.AccountName,
                DealerName = x.DealerName,
                Phone = x.Phone,
                RegisterTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                Status = x.Status,
                ManagedBy = x.ManagedBy
            }).ToList();

            return BaseApiResponse.OK(resData: dataRes, totalRecord: totalRecord);
        }

        #endregion

        #region Get Collector Request Register Detail

        /// <summary>
        /// Gets the collector request register.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectorRequestRegister(Guid id)
        {
            if (!_accountRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            var dataQuery = await _accountRepository.GetAsync(x => x.Id.Equals(id));

            var result = new CollectorRequestRegisterViewDetailModel()
            {
                Id = dataQuery.Id,
                Name = dataQuery.Name,
                Address = dataQuery.Address,
                BirthDate = dataQuery.BirthDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                Gender = dataQuery.Gender,
                IDCard = dataQuery.IdCard,
                Phone = dataQuery.Phone,
                RegisterTime = dataQuery.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                Status = dataQuery.Status
            };

            return BaseApiResponse.OK(result);
        }

        #endregion

        #region Get Dealer Request Register

        public async Task<BaseApiResponseModel> GetDealerRequestRegister(Guid id)
        {
            if (!_accountRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
