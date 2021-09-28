using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.ActivityModels;
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
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public class ActivityService : BaseService, IActivityService
    {
        #region Repositories

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        /// <summary>
        /// The sell collect transaction repository
        /// </summary>
        private readonly IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        /// <summary>
        /// The sell collect transaction detail repository
        /// </summary>
        private readonly IRepository<SellCollectTransactionDetail> _sellCollectTransactionDetailRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;



        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public ActivityService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, ICacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _sellCollectTransactionDetailRepository = unitOfWork.SellCollectTransactionDetailRepository;
            _accountRepository = unitOfWork.AccountRepository;
        }

        #endregion

        #region Get Activities

        /// <summary>
        /// Gets the activities.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetActivities(ActivityFilterModel model)
        {
            var crStatus = CommonUtils.GetActivityStatus(model.Status);

            if (!crStatus.Any())
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => x.SellerAccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                  crStatus.Contains(x.Status.Value))
                                                        .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                              (x, y) => new
                                                              {
                                                                  x.Id,
                                                                  x.CollectingRequestCode,
                                                                  x.CollectingRequestDate,
                                                                  x.CreatedTime,
                                                                  x.IsBulky,
                                                                  y.AddressName,
                                                                  x.Status
                                                              });

            var totalRecord = await dataQuery.CountAsync();

            var activities = dataQuery.Select(x => new ActivityViewModel()
            {
                CollectingRequestId = x.Id,
                CollectingRequestCode = x.CollectingRequestCode,
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                AddressName = x.AddressName,
                CreatedDate = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MMM_yyyy),
                CreatedTime = x.CreatedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM),
                Status = x.Status,
                IsBulky = x.IsBulky
            }).ToList();

            if (model.Status == CollectingRequestStatus.COMPLETED)
            {
                activities = activities.GroupJoin(_sellCollectTransactionRepository.GetAllAsNoTracking(), x => x.CollectingRequestId, y => y.CollectingRequestId,
                                                (x, y) => new
                                                {
                                                    Activity = x,
                                                    Transaction = y
                                                })
                                        .SelectMany(x => x.Transaction.DefaultIfEmpty(), (x, y) =>
                                        {
                                            x.Activity.Total = y?.Total;
                                            return x.Activity;
                                        }).ToList();
            }

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: activities);
        }

        #endregion


        #region Get Activity Detail

        /// <summary>
        /// Gets the activity detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetActivityDetail(Guid id)
        {
            var crEntity = await _collectingRequestRepository.GetAsync(x => x.Id.Equals(id) &&
                                                                            x.SellerAccountId.Equals(UserAuthSession.UserSession.Id));
            if (crEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var location = _locationRepository.GetById(crEntity.LocationId);

            var dataResult = new ActivityDetailViewModel()
            {
                Id = crEntity.Id,
                CreatedDate = crEntity.CreatedTime.ToStringFormat(DateTimeFormat.DD_MMM_yyyy),
                CreatedTime = crEntity.CreatedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM),
                CollectingRequestCode = crEntity.CollectingRequestCode,
                CollectingRequestDate = crEntity.CollectingRequestDate.ToStringFormat(DateTimeFormat.DDD_dd_MMM),
                FromTime = crEntity.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = crEntity.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                IsBulky = crEntity.IsBulky,
                Note = crEntity.Note,
                ScrapCategoryImageUrl = crEntity.ScrapImageUrl,
                Status = crEntity.Status,
                Address = location.Address,
                AddressName = location.AddressName,
            };

            var collectorInfo = _accountRepository.GetById(crEntity.CollectorAccountId);

            dataResult.CollectorInfo = new CollectorInformation()
            {
                Name = collectorInfo?.Name,
                Phone = collectorInfo?.Phone
            };

            // TODO: 
            if (crEntity.Status == CollectingRequestStatus.COMPLETED)
            {

            }

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
