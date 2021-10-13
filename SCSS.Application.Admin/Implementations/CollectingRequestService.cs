using Dapper;
using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.CollectingRequestModels;
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
    public class CollectingRequestService : BaseService, ICollectingRequestService
    {
        #region Repositories

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The collecting request rejection repository
        /// </summary>
        private readonly IRepository<CollectingRequestRejection> _collectingRequestRejectionRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The role repository
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _collectingRequestRejectionRepository = unitOfWork.CollectingRequestRejectionRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _locationRepository = unitOfWork.LocationRepository;
        }

        #endregion

        #region Search Collecting Request

        /// <summary>
        /// Searches the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchCollectingRequest(CollectingRequestSearchModel model)
        {
            var sellerRoleId = _roleRepository.GetAsNoTracking(x => x.Key == AccountRole.SELLER).Id;
            var collectorRoleId = _roleRepository.GetAsNoTracking(x => x.Key == AccountRole.COLLECTOR).Id;

            //var testData = _collectingRequestRepository.GetManyAsNoTracking(x => DbFunctions.TruncateTime(x.CreatedTime))

            var dataDB = await _collectingRequestRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.CollectingRequestCode) || x.CollectingRequestCode.Contains(model.CollectingRequestCode)) &&
                                                                                        (model.Status == NumberConstant.Zero || x.Status == model.Status)).ToListAsync();

            var dataQuery = dataDB.Where(x => (ValidatorUtil.IsBlank(model.FromDate) || x.CollectingRequestDate.Value.Date.CompareTo(model.FromDate.ToDateTime()) >= NumberConstant.Zero) &&
                                                (ValidatorUtil.IsBlank(model.ToDate) || x.CollectingRequestDate.Value.Date.CompareTo(model.ToDate.ToDateTime()) <= NumberConstant.Zero))
                                                .Join(_accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(sellerRoleId) &&
                                                                                                    (ValidatorUtil.IsBlank(model.RequestedBy) || x.Name.Contains(model.RequestedBy))), x => x.SellerAccountId, y => y.Id,
                                                                                                    (x, y) => new
                                                                                                    {
                                                                                                        CollectingRequestId = x.Id,
                                                                                                        x.CollectingRequestCode,
                                                                                                        x.CollectingRequestDate,
                                                                                                        x.TimeFrom,
                                                                                                        x.TimeTo,
                                                                                                        CollectingRequestStatus = x.Status,
                                                                                                        RequesterName = y.Name,
                                                                                                        x.CollectorAccountId,
                                                                                                        x.CreatedTime,
                                                                                                    })
                                                .GroupJoin(_accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(collectorRoleId) &&
                                                                                                    (ValidatorUtil.IsBlank(model.ReceivedBy) || x.Name.Contains(model.ReceivedBy))), x => x.CollectorAccountId, y => y.Id,
                                                                                                    (x, y) => new
                                                                                                    {
                                                                                                        x.CollectingRequestId,
                                                                                                        x.CollectingRequestCode,
                                                                                                        x.CollectingRequestDate,
                                                                                                        x.TimeFrom,
                                                                                                        x.TimeTo,
                                                                                                        x.CollectingRequestStatus,
                                                                                                        x.RequesterName,
                                                                                                        x.CreatedTime,
                                                                                                        CollectorInfo = y
                                                                                                    })
                                                                                                    .SelectMany(x => x.CollectorInfo.DefaultIfEmpty(), (x, y) => new
                                                                                                    {
                                                                                                        x.CollectingRequestId,
                                                                                                        x.CollectingRequestCode,
                                                                                                        x.CollectingRequestDate,
                                                                                                        x.TimeFrom,
                                                                                                        x.TimeTo,
                                                                                                        x.CollectingRequestStatus,
                                                                                                        x.RequesterName,
                                                                                                        x.CreatedTime,
                                                                                                        ReceiverName = y?.Name
                                                                                                    });

            if (!ValidatorUtil.IsBlank(model.ReceivedBy))
            {
                dataQuery = dataQuery.Where(x => !string.IsNullOrEmpty(x.ReceiverName));
            }

            var totalRecord = dataQuery.Count();

            var dataResult = dataQuery.OrderByDescending(x => x.CreatedTime).Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).Select(x => new CollectingRequestViewModel()
            {
                Id = x.CollectingRequestId,
                CollectingRequestCode = x.CollectingRequestCode,
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                CollectingRequestRangeTime = string.Format("{0}-{1}", x.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                                                                      x.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM)),
                RecevicedBy = x.ReceiverName,
                RequestedBy = x.RequesterName,
                Status = x.CollectingRequestStatus
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion


        #region Get Collecting Request Detail

        /// <summary>
        /// Gets the collecting request detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectingRequestDetail(Guid id)
        {
            if (!_collectingRequestRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound();
            }

            var dataEntity = await _collectingRequestRepository.GetAsync(x => x.Id.Equals(id));

            var sellerInfo = _accountRepository.GetById(dataEntity.SellerAccountId);

            var collectorInfo = _accountRepository.GetById(dataEntity.CollectorAccountId);

            var location = await _locationRepository.GetByIdAsync(dataEntity.LocationId);

            var collectorRoleId = _roleRepository.GetAsNoTracking(x => x.Key == AccountRole.COLLECTOR).Id;


            var crRejections = _collectingRequestRejectionRepository.GetManyAsNoTracking(x => x.CollectingRequestId.Equals(dataEntity.Id))
                                                                    .Join(_accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(collectorRoleId)),
                                                                           x => x.CollectorId, y => y.Id, (x, y) => new CollectingRequestRejectionViewModel()
                                                                           {
                                                                               RejecterName = y.Name,
                                                                               Reason = x.Reason
                                                                           }).ToList();
            var dataResult = new CollectingRequestDetailViewModel()
            {
                Id = dataEntity.Id,
                Address = location.Address,
                CollectingRequestCode = dataEntity.CollectingRequestCode,
                CollectingRequestDate = dataEntity.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                CollectingRequestRangeTime = string.Format("{0}-{1}", dataEntity.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                                                                      dataEntity.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM)),
                IsBulky = dataEntity.IsBulky,
                Note = dataEntity.Note,
                RequestedBy = sellerInfo.Name,
                Status = dataEntity.Status,
                RejectionItems = crRejections,
                ReceivedBy = collectorInfo == null ? CommonConstants.Null : collectorInfo.Name
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion


    }
}
