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
using System.Linq.Dynamic.Core;
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

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        /// <summary>
        /// The feedback repository
        /// </summary>
        private readonly IRepository<Feedback> _feedbackRepository;

        /// <summary>
        /// The feedback to system repository
        /// </summary>
        private readonly IRepository<FeedbackToSystem> _feedbackToSystemRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public ActivityService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _sellCollectTransactionDetailRepository = unitOfWork.SellCollectTransactionDetailRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _feedbackToSystemRepository = unitOfWork.FeedbackToSystemRepository;
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
                                                                  x.TimeFrom,
                                                                  x.TimeTo,
                                                                  x.ApprovedTime,
                                                                  x.UpdatedTime,
                                                                  x.IsBulky,
                                                                  y.AddressName,
                                                                  y.Address,
                                                                  x.Status
                                                              });

            if (CollectionConstants.RemainingCollectingRequest.Contains(model.Status))
            {
                var sortBy = model.Status == CollectingRequestStatus.PENDING ? DefaultSort.CollectingRequestDateDESC : DefaultSort.ApprovedTimeDESC;
                dataQuery = dataQuery.OrderBy(sortBy);
            }

            var totalRecord = await dataQuery.CountAsync();

            var activities = dataQuery.Select(x => new ActivityViewModel()
            {
                CollectingRequestId = x.Id,
                CollectingRequestCode = x.CollectingRequestCode,
                CollectingRequestDate = x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DDD_DD_MMM_yyy_HH_mm),
                AddressName = x.AddressName,
                Address = x.Address,
                FromTime = x.TimeFrom.ToStringFormat(TimeSpanFormat.HH_MM),
                ToTime = x.TimeTo.ToStringFormat(TimeSpanFormat.HH_MM),
                Status = x.Status,
                CompletedTime = x.UpdatedTime,
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
                                            x.Activity.CompletedTime = y != null ? y.CreatedTime : x.Activity.CompletedTime;
                                            return x.Activity;
                                        }).OrderByDescending(x => x.CompletedTime).ToList();
            }

            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;

            var dataResult = activities.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
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
                CancelReasoin = crEntity.CancelReason,
                Address = location.Address,
                AddressName = location.AddressName,
            };

            if (dataResult.Status == CollectingRequestStatus.PENDING)
            {
                dataResult.IsCancelable = BooleanConstants.TRUE;
            }

            var collectorInfo = _accountRepository.GetById(crEntity.CollectorAccountId);

            if (collectorInfo != null)
            {
                dataResult.CollectorInfo = new CollectorInformation()
                {
                    CollectorId = collectorInfo?.Id,
                    Name = collectorInfo?.Name,
                    ImageURL = collectorInfo?.ImageUrl,
                    Phone = collectorInfo?.Phone,
                    Rating = collectorInfo?.Rating
                };

                if (crEntity.ApprovedTime != null)
                {
                    dataResult.ApprovedDate = crEntity.ApprovedTime.ToStringFormat(DateTimeFormat.DD_MMM_yyyy);
                    dataResult.ApprovedTime = crEntity.ApprovedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM);
                }
            }

            var feedbackToSys = _feedbackToSystemRepository.GetAsNoTracking(x => x.CollectingRequestId.Equals(crEntity.Id) &&
                                                                                 x.CollectDealTransactionId == null);


            dataResult.FeedbackToSystemInfo = new FeedbackToSystemInfoResponse()
            {
                SellingFeedback = feedbackToSys?.SellingFeedback,
                AdminReply = feedbackToSys?.AdminReply,
                FeedbackStatus = CommonUtils.GetFeedbackToSystemStatus(crEntity.Status, crEntity.CollectorAccountId, feedbackToSys?.Id, feedbackToSys?.AdminReply)
            };


            if (dataResult.Status == CollectingRequestStatus.APPROVED)
            {
                var cancelTimeRange = await CancelTimeRange();

                var dateTimeFrom = crEntity.CollectingRequestDate.Value.Add(crEntity.TimeFrom.Value);
                var timeRange = (int)dateTimeFrom.Subtract(DateTimeVN.DATETIME_NOW.StripSecondAndMilliseconds()).TotalMinutes;
                dataResult.IsCancelable = !(timeRange <= cancelTimeRange);
            }


            if (crEntity.UpdatedTime != null)
            {
                dataResult.DoneActivityDate = crEntity.UpdatedTime.ToStringFormat(DateTimeFormat.DD_MMM_yyyy);
                dataResult.DoneActivityTime = crEntity.UpdatedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM);
            }

            if (crEntity.Status == CollectingRequestStatus.COMPLETED)
            {
                var transactionInfo = _sellCollectTransactionRepository.GetAsNoTracking(x => x.CollectingRequestId.Equals(crEntity.Id));

                var transactionDetailItems = await GetTransactionInformationDetails(transactionInfo.Id);

                var feedbackInfo = await GetFeedbackInfo(transactionInfo.Id, transactionInfo.CreatedTime);

                dataResult.Transaction = new TransactionInformation()
                {
                    TransactionId = transactionInfo?.Id,
                    TransactionDate = transactionInfo?.CreatedTime.ToStringFormat(DateTimeFormat.DDD_dd_MMM),
                    TransactionTime = transactionInfo?.CreatedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM),
                    Total = transactionInfo?.Total,
                    Fee = transactionInfo?.TransactionServiceFee,
                    AwardPoint = transactionInfo?.AwardPoint,
                    Details = transactionDetailItems,
                    FeedbackInfo = feedbackInfo
                };
                dataResult.IsCancelable = BooleanConstants.FALSE;
            }

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        #region Get Transaction Information Details

        /// <summary>
        /// Gets the transaction information details.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <returns></returns>
        private async Task<List<TransactionInformationDetail>> GetTransactionInformationDetails(Guid transId)
        {
            var transactionDetailItems = await _sellCollectTransactionDetailRepository.GetManyAsNoTracking(x => x.SellCollectTransactionId.Equals(transId))
                                                                                    .GroupJoin(_scrapCategoryDetailRepository.GetAllAsNoTracking(), x => x.CollectorCategoryDetailId, y => y.Id,
                                                                                         (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             ScrapCategoryDetail = y
                                                                                         }).SelectMany(x => x.ScrapCategoryDetail.DefaultIfEmpty(), (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             y.ScrapCategoryId,
                                                                                             y.Unit,
                                                                                         })
                                                                                    .GroupJoin(_scrapCategoryRepository.GetAllAsNoTracking(), x => x.ScrapCategoryId, y => y.Id,
                                                                                         (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             x.Unit,
                                                                                             ScrapCategory = y
                                                                                         }).SelectMany(x => x.ScrapCategory.DefaultIfEmpty(), (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             x.Unit,
                                                                                             y.Name
                                                                                         }).Select(x => new TransactionInformationDetail()
                                                                                         {
                                                                                             Quantity = x.Quantity,
                                                                                             ScrapCategoryName = x.Name,
                                                                                             Unit = x.Unit,
                                                                                             Total = x.Total,
                                                                                         }).ToListAsync();
            return transactionDetailItems;
        }

        #endregion

        #region Get FeedbackInfo

        /// <summary>
        /// Gets the feedback information.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <param name="createdTransTime">The created trans time.</param>
        /// <returns></returns>
        private async Task<FeedbackInformationResponse> GetFeedbackInfo(Guid transId, DateTime? createdTransTime)
        {
            var deadline = await FeedbackDeadline();

            var betweenDays = DateTimeUtils.IsMoreThanPastDays(createdTransTime, deadline);

            if (betweenDays)
            {
                return new FeedbackInformationResponse()
                {
                    FeedbackStatus = FeedbackStatus.TimeUpToGiveFeedback
                };
            }

            var feedback = _feedbackRepository.GetAsNoTracking(x => x.SellCollectTransactionId.Equals(transId));

            if (feedback == null)
            {
                return new FeedbackInformationResponse()
                {
                    FeedbackStatus = FeedbackStatus.HaveNotGivenFeedbackYet
                };
            }

            return new FeedbackInformationResponse()
            {
                FeedbackStatus = FeedbackStatus.HaveGivenFeedback,
                RatingFeedback = feedback.Rate
            };
        }

        #endregion
    }
}
