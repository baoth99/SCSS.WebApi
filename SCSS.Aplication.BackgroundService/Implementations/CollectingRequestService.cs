using Dapper;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class CollectingRequestService : BaseService, ICollectingRequestService
    {

        #region Repositories

        /// <summary>
        /// The account repository/
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        #endregion

        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        /// <summary>
        /// The cache list service
        /// </summary>
        private readonly ICacheListService _cacheListService;

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dapperService">The dapper service.</param>
        /// <param name="cacheService">The cache service.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IDapperService dapperService, ICacheListService cacheListService, ISQSPublisherService SQSPublisherService) : base(unitOfWork)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _dapperService = dapperService;
            _cacheListService = cacheListService;
            _SQSPublisherService = SQSPublisherService;
        }

        #endregion

        #region Trail Collecting Request In Day (Background Task)

        /// <summary>
        /// Trails the collecting request in day.
        /// </summary>
        public async Task TrailCollectingRequestInDayBackground()
        {
            await PublishMessageToSQS();

            var sql = AppFileHelper.ReadContent(AppSettingValues.CollectingRequestSQLCommand, "TrailCollectingRequestInDay.sql");

            var parameters = new DynamicParameters();
            parameters.Add("@CancelBySystemStatus", CollectingRequestStatus.CANCEL_BY_SYSTEM);
            parameters.Add("@UpdatedBy", Guid.Empty);
            parameters.Add("@DateNow", DateTimeVN.DATE_NOW);
            parameters.Add("@DateTimeNow", DateTimeVN.DATETIME_NOW);
            parameters.Add("@ApprovedStatus", CollectingRequestStatus.APPROVED);

            await _dapperService.SqlExecuteAsync(sql, parameters);
        }

        #endregion

        #region PublishMessageToSQS ()

        /// <summary>
        /// Publishes the message to SQS.
        /// </summary>
        private async Task PublishMessageToSQS()
        {
            var dataQuery = _collectingRequestRepository.GetManyAsNoTracking(x => x.CollectingRequestDate.Value.Date.CompareTo(DateTimeVN.DATE_NOW) == NumberConstant.Zero &&
                                                                                  x.Status == CollectingRequestStatus.APPROVED);

            var sellerAccountNotifications = dataQuery.Join(_accountRepository.GetAllAsNoTracking(), x => x.SellerAccountId, y => y.Id,
                                                            (x, y) => new
                                                            {
                                                                SellerDeviceId = y.DeviceId,
                                                                CollectingRequestId = x.Id,
                                                                x.SellerAccountId,
                                                                x.CollectingRequestCode,
                                                                x.CollectingRequestDate,
                                                                x.ApprovedTime,
                                                                x.TimeFrom,
                                                                x.TimeTo,
                                                            }).Select(x => new NotificationMessageQueueModel()
                                                            {
                                                                AccountId = x.SellerAccountId,
                                                                DeviceId = x.SellerDeviceId,
                                                                Title = NotificationMessage.SystemCancelCRTitle,
                                                                Body = NotificationMessage.SystemCancelCRSellerBody(x.CollectingRequestCode, x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy)),
                                                                DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, x.CollectingRequestId.ToString()),
                                                                NotiType = CollectingRequestStatus.CANCEL_BY_SYSTEM
                                                            }).ToList();
            if (sellerAccountNotifications.Any())
            {
                await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(sellerAccountNotifications);
            }

            var collectorAccountNotifications = dataQuery.Join(_accountRepository.GetAllAsNoTracking(), x => x.CollectorAccountId, y => y.Id,
                                                            (x, y) => new
                                                            {
                                                                CollectorDeviceId = y.DeviceId,
                                                                CollectingRequestId = x.Id,
                                                                x.CollectorAccountId,
                                                                x.CollectingRequestCode,
                                                                x.CollectingRequestDate,
                                                                x.ApprovedTime,
                                                                x.TimeFrom,
                                                                x.TimeTo,
                                                            })
                                                        .Select(x => new NotificationMessageQueueModel
                                                        {
                                                            AccountId = x.CollectorAccountId,
                                                            DeviceId = x.CollectorDeviceId,
                                                            Title = NotificationMessage.SystemCancelCRTitle,
                                                            Body = NotificationMessage.SystemCancelCRCollectorBody(x.CollectingRequestCode, x.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy)),
                                                            DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.HistoryScreen, x.CollectingRequestId.ToString()),
                                                            NotiType = CollectingRequestStatus.CANCEL_BY_SYSTEM
                                                        }).ToList();

            if (collectorAccountNotifications.Any())
            {
                await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(collectorAccountNotifications);
            }
        }

        #endregion

        #region Trail To CancelCollecting Request

        /// <summary>
        /// Trails to cancel collecting request.
        /// </summary>
        public async Task ScanToCancelCollectingRequest()
        {
            var pendingCRCache = _cacheListService.PendingCollectingRequestCache;

            var cache = await pendingCRCache.GetAllAsync();

            if (cache.Any())
            {
                var cacheList = cache.ToList();

                foreach (var item in cacheList)
                {
                    if (item.Date.Value.Date.IsCompareDateTimeEqual(DateTimeVN.DATE_NOW))
                    {
                        if (item.ToTime.IsCompareTimeSpanLessOrEqual(DateTimeVN.TIMESPAN_NOW))
                        {
                            // Remove From Cache
                            await pendingCRCache.RemoveAsync(item);

                            // Update DB
                            var res = await CancelCollectingRequest(item.Id);
                            if (res > NumberConstant.Zero)
                            {
                                // Publish Message To Amazon SQS
                                await PublishMessageToSQS(item.Id);
                            }
                            continue;
                        }

                        var itemFromTime = item.FromTime.Value.Subtract(new TimeSpan(00, 15, 00));

                        if (itemFromTime.IsCompareTimeSpanLessOrEqual(DateTimeVN.TIMESPAN_NOW))
                        {
                            var request = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(item.Id));
                            if (request.RequestType == CollectingRequestType.MAKE_AN_APPOINTMENT && request.Status == CollectingRequestStatus.PENDING)
                            {
                                var executedRes = await ChangeRequetType(item.Id);
                                if (executedRes > NumberConstant.Zero)
                                {
                                    var location = _locationRepository.GetById(request.LocationId);

                                    var notifierQueue = new CollectingRequestNotiticationQueueModel()
                                    {
                                        CollectingRequestId = request.Id,
                                        Latitude = location.Latitude.Value,
                                        Longitude = location.Longitude.Value,
                                        RequestType = CollectingRequestType.CURRENT_REQUEST
                                    };
                                    // Publish Message To Amazon SQS
                                    await _SQSPublisherService.CollectingRequestNotiticationPublisher.SendMessageAsync(notifierQueue);
                                }
                            }
                        }
                    }
                }
            }
        }

        #endregion


        #region Change Collecting Request Type

        /// <summary>
        /// Changes the type of the requet.
        /// </summary>
        /// <param name="crId">The cr identifier.</param>
        /// <returns></returns>
        private async Task<int> ChangeRequetType(Guid crId)
        {
            var sql = "UPDATE [CollectingRequest] " + 
                      "SET [RequestType] = @RequestType, " +
                      "[UpdatedBy] = @UpdatedBy, [UpdatedTime] = @DateNow " +
                      "WHERE [Id] = @CollectingRequestId AND " +
                      "[Status] = @PendingStatus AND " +
                      "[CollectorAccountId] IS NULL"; 

            var parameters = new DynamicParameters();
            parameters.Add("@RequestType", CollectingRequestType.SWITCH_TO_CURRENT_REQUEST);
            parameters.Add("@CollectingRequestId", crId);
            parameters.Add("@UpdatedBy", Guid.Empty);
            parameters.Add("@DateNow", DateTimeVN.DATETIME_NOW);
            parameters.Add("@PendingStatus", CollectingRequestStatus.PENDING);

            return await _dapperService.SqlExecuteAsync(sql, parameters);
        }

        #endregion



        #region Cancel Collecting Request

        /// <summary>
        /// Cancels the collecting request.
        /// </summary>
        /// <param name="crId">The cr identifier.</param>
        private async Task<int> CancelCollectingRequest(Guid crId)
        {
            var sql = "UPDATE [CollectingRequest] " +
                      "SET [Status] = @CancelBySystemStatus, " +
                      "[UpdatedBy] = @UpdatedBy, [UpdatedTime] = @DateNow " +
                      "WHERE [Id] = @CollectingRequestId AND " +
                      "[Status] = @PendingStatus";

            var parameters = new DynamicParameters();
            parameters.Add("@CollectingRequestId", crId);
            parameters.Add("@CancelBySystemStatus", CollectingRequestStatus.CANCEL_BY_SYSTEM);
            parameters.Add("@UpdatedBy", Guid.Empty);
            parameters.Add("@DateNow", DateTimeVN.DATETIME_NOW);
            parameters.Add("@PendingStatus", CollectingRequestStatus.PENDING);

            return await _dapperService.SqlExecuteAsync(sql, parameters);
        }

        #endregion

        #region Publish Message To AWS SQS

        /// <summary>
        /// Publishes the message to SQS.
        /// </summary>
        /// <param name="id">The identifier.</param>
        private async Task PublishMessageToSQS(Guid id)
        {
            var messageInfo = _collectingRequestRepository.GetManyAsNoTracking(x => x.Id.Equals(id))
                            .Join(_accountRepository.GetAllAsNoTracking(), x => x.SellerAccountId, y => y.Id,
                                  (x, y) => new
                                  {
                                      AccountId = y.Id,
                                      CollectingRequestId = x.Id,
                                      x.CollectingRequestCode,
                                      y.DeviceId
                                  }).FirstOrDefault();

            if (messageInfo != null)
            {
                var publishModel = new NotificationMessageQueueModel()
                {
                    AccountId = messageInfo.AccountId,
                    Title = NotificationMessage.CancelCollectingRequestTitleSystem(messageInfo.CollectingRequestCode),
                    Body = NotificationMessage.CancelCollectingRequestBodySystem(messageInfo.CollectingRequestCode),
                    DeviceId = messageInfo.DeviceId,
                    DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, messageInfo.CollectingRequestId.ToString()),
                    NotiType = CollectingRequestStatus.CANCEL_BY_SYSTEM
                };

                await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessageAsync(publishModel);
            }
        }

        #endregion

    }
}
