using Microsoft.EntityFrameworkCore;
using System.Linq.Dynamic.Core;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ScrapCategoryModels;
using SCSS.AWSService.Interfaces;
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
using System.Threading.Tasks;
using SCSS.Utilities.Extensions;

namespace SCSS.Application.Admin.Implementations
{
    public class ScrapCategoryService : BaseService, IScrapCategoryService
    {
        #region Repositories

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

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

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ScrapCategoryService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public ScrapCategoryService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
        }

        #endregion

        #region Search Scrap Category

        /// <summary>
        /// Searches the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchScrapCategory(ScrapCategorySearchModel model)
        {
            var dataQuery = _scrapCategoryRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.Name) || x.Name.Contains(model.Name)) &&
                                                                              (model.Status == ScrapCategoryStatus.ALL || x.Status == model.Status))
                                                    .Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.CreatedBy) || x.Name.Contains(model.CreatedBy)) &&
                                                                                                      (ValidatorUtil.IsBlank(model.PhoneCreatedBy) || x.Phone.Contains(model.PhoneCreatedBy))),
                                                                                                 x => x.AccountId, y => y.Id, (x, y) => new
                                                                                                 {
                                                                                                     RoleId = y.RoleId,
                                                                                                     ScrapCategoryId = x.Id,
                                                                                                     ScrapCategoryName = x.Name,
                                                                                                     x.Status,
                                                                                                     x.CreatedTime,
                                                                                                     CreatedBy = y.Name,
                                                                                                 }).Join(_roleRepository.GetManyAsNoTracking(x => model.Role == CommonConstants.Zero || x.Key == model.Role), 
                                                                                                        x => x.RoleId, y => y.Id, (x, y) => new
                                                                                                        {
                                                                                                            x.ScrapCategoryId,
                                                                                                            x.ScrapCategoryName,
                                                                                                            x.CreatedBy,
                                                                                                            x.Status,
                                                                                                            x.CreatedTime,
                                                                                                            Role = y.Key
                                                                                                        }).OrderBy(DefaultSort.CreatedTimeDESC);

            var totalRecord = await dataQuery.CountAsync();

            var dataRes = dataQuery.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new ScrapCategoryViewModel()
            {
                Id = x.ScrapCategoryId,
                Name = x.ScrapCategoryName,
                CreatedBy = x.CreatedBy,
                Status = x.Status,
                CreatedTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                Role = x.Role
            }).ToList();

            return BaseApiResponse.OK(resData: dataRes, totalRecord: totalRecord);
        }

        #endregion

        #region Get Scrap Category Detail

        /// <summary>
        /// Gets the scrap category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetScrapCategory(Guid id)
        {
            if (!_scrapCategoryRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.NotFound);
            }
            
            var scrapCategory = await _scrapCategoryRepository.GetAsyncAsNoTracking(x => x.Id.Equals(id));

            var account = _accountRepository.GetById(scrapCategory.AccountId);

            // Get Scrap Category Details
            var scrapCategoryDetails = _scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(id)).OrderByDescending(x => x.CreatedTime)
                                                                    .Select(x => new ScrapCategoryDetailViewItemModel()
                                                                    {
                                                                        Unit = x.Unit,
                                                                        Price = x.Price,
                                                                        Status = x.Status,
                                                                        UpdatedTime = x.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time)
                                                                    }).ToList();
            // Check Role
            var role = _roleRepository.GetById(account.RoleId);
            // If Collector
            if (role.Name.Equals(AccountRoleConstants.COLLECTOR))
            {
                var dataResult = new CollectorScrapCategoryViewDetailModel()
                {
                    Name = scrapCategory.Name,
                    ImageUrl = scrapCategory.ImageUrl,
                    CreatedBy = account.Name,
                    CreatedTime = scrapCategory.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                    UpdatedTime = scrapCategory.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                    Role = role.Key,
                    Items = scrapCategoryDetails,
                    Status = scrapCategory.Status
                };

                return BaseApiResponse.OK(dataResult);
            }

            // If Dealer
            if (role.Name.Equals(AccountRoleConstants.DEALER))
            {
                var dealer = await _dealerInformationRepository.GetAsyncAsNoTracking(x => x.DealerAccountId.Equals(account.Id));
                var dealerManagerId = account.ManagedBy;
                string manageBy = string.Empty;

                if (!ValidatorUtil.IsNull(dealerManagerId))
                {
                    var dealerManager = _dealerInformationRepository.GetAsNoTracking(x => x.DealerAccountId.Equals(dealerManagerId));
                    manageBy = dealerManager.DealerName;
                }
                var dataResult = new DealerScrapCategoryViewDetailModel()
                {
                    Name = scrapCategory.Name,
                    DealerName = dealer.DealerName,
                    ManageBy = manageBy,
                    ImageUrl = scrapCategory.ImageUrl,
                    CreatedBy = account.Name,
                    CreatedTime = scrapCategory.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                    UpdatedTime = scrapCategory.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                    Items = scrapCategoryDetails,
                    Role = role.Key,
                    Status = scrapCategory.Status
                };

                return BaseApiResponse.OK(dataResult);
            }

            return BaseApiResponse.Error();
        }

        #endregion
    }
}
