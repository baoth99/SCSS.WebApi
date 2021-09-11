using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.DealerInformationModels;
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

namespace SCSS.Application.Admin.Implementations
{
    public class DealerInformationService : BaseService, IDealerInformationService
    {
        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private IRepository<Account> _accountRepository;

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private IRepository<Location> _locationRepository;

        #endregion


        #region Constructor

        public DealerInformationService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _locationRepository = unitOfWork.LocationRepository;
        }

        #endregion

        #region Search Dealer Information

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> Search(DealerInformationSearchModel model)
        {
            // Get Dealer Information by requested model
            var dealerDataQuery = _dealerInformationRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.DealerName) || x.DealerName.Contains(model.DealerName)) &&
                                                                                  (ValidatorUtil.IsBlank(model.DealerPhone) || x.DealerPhone.Contains(model.DealerPhone)) &&
                                                                                  (model.Status == null || x.IsActive == model.Status))
                                                        .Join(_locationRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.DealerAddress) || x.Address.Contains(model.DealerAddress))),
                                                                                  x => x.LocationId, y => y.Id, (x, y) => new
                                                                                  {
                                                                                      x.Id,
                                                                                      x.DealerName,
                                                                                      x.DealerPhone,
                                                                                      x.IsActive,
                                                                                      x.IsSubcribed,
                                                                                      x.CreatedTime,
                                                                                      x.DealerAccountId
                                                                                  });
            // Get Dealer Information and Dealer Manager Name by request manager name
            var dataQuery = dealerDataQuery.Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.ManagedBy) || x.Name.Contains(model.ManagedBy))),
                                                x => x.DealerAccountId, y => y.Id, (x, y) => new
                                                {
                                                    DealerId = x.Id,
                                                    x.DealerName,
                                                    x.DealerPhone,
                                                    DealerStatus = x.IsActive,
                                                    IsDealerSubcribe = x.IsSubcribed,
                                                    DealerCreatedTime = x.CreatedTime,
                                                    y.Name,
                                                    y.ManagedBy
                                                });

            // Check dataQuery is Empty !!
            if (!dataQuery.Any())
            {
                return BaseApiResponse.OK(totalRecord: CommonConstants.Zero, resData: CollectionConstants.Empty<DealerInformationViewModel>());
            }

            // Check Dealer Type is Leader
            if (model.DealerType == DealerType.LEADER)
            {
                // Dealer Type is Leader when ManagedBy Field is blank
                dataQuery = dataQuery.Where(x => !ValidatorUtil.IsBlank(x.ManagedBy));
            }

            // Check Dealer Type is Member
            if (model.DealerType == DealerType.MEMBER)
            {
                // Dealer Type is Member when ManagedBy Field is not blank
                dataQuery = dataQuery.Where(x => ValidatorUtil.IsBlank(x.ManagedBy));
            }

            var totalRecord = await dataQuery.CountAsync();

            var resultData = dataQuery.OrderByDescending(x => x.DealerCreatedTime)
                                      .Skip((model.Page - 1) * model.PageSize).Take(model.PageSize)
                                        .Select(x => new DealerInformationViewModel()
                                        {
                                            Id = x.DealerId,
                                            DealerName = x.DealerName,
                                            DealerPhone = x.DealerPhone,
                                            DealerType = ValidatorUtil.IsBlank(x.ManagedBy), // Leader is true, Member is false
                                            CreatedTime = x.DealerCreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time),
                                            ManagedBy = x.Name,
                                            IsSubcribed = x.IsDealerSubcribe,
                                            Status = x.DealerStatus
                                        }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: resultData);
        }

        #endregion


        #region Get Dealer Information Detail

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDetail(Guid id)
        {
            if (!_dealerInformationRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }



            return BaseApiResponse.OK();
        }

        #endregion

    }
}
