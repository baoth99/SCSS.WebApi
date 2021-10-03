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
            parameters.Add("@DateNow", DateTimeVN.DATETIME_NOW);
            parameters.Add("@ApprovedStatus", CollectingRequestStatus.APPROVED);
            parameters.Add("@PendingStatus", CollectingRequestStatus.PENDING);

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
                                                                DataCustom = null, // TODO:
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
                                                            DataCustom = null, // TODO:
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
                    if (item.Date.IsCompareDateTimeEqual(DateTimeVN.DATE_NOW) && item.ToTime.IsCompareTimeSpanGreaterOrEqual(DateTimeVN.TIMESPAN_NOW))
                    {
                        // Remove From Cache
                        await pendingCRCache.RemoveAsync(item);

                        // Update DB
                        await CancelCollectingRequest(item.Id);

                        // Publish Message To Amazon SQS
                        await PublishMessageToSQS(item.Id);
                    }
                }
            }

            Console.WriteLine("Processing....");

            //var publishModel = new NotificationMessageQueueModel()
            //{
            //    AccountId = Guid.Parse("78e1f230-effd-4b62-9ea6-b98491dd4ed9"),
            //    Title = NotificationMessage.CancelCollectingRequestTitleSystem("SQR-21312312312312"),
            //    Body = NotificationMessage.CancelCollectingRequestBodySystem("SQR-21312312312312"),
            //    DeviceId = "d3goJZwvQaWZaxLJG5OBuZ:APA91bHurVY2ATlZ2Cgg66RpN9ZX0K3FJBpg_P4Yv8uZIJgmNC-RoWV9zaNPCV9VZ_qjhlJW52vKQGK8fyR494sJpibsy3UKjYTbENqNcSrXdbcy0wOTP9ZBFvoFck6LQ-P5KiMBFfbE"
            //};

            //await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessageAsync(publishModel);
        }

        #endregion

        #region Cancel Collecting Request

        /// <summary>
        /// Cancels the collecting request.
        /// </summary>
        /// <param name="crId">The cr identifier.</param>
        private async Task CancelCollectingRequest(Guid crId)
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

            await _dapperService.SqlExecuteAsync(sql, parameters);
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
                    DataCustom = null, // TODO:
                    NotiType = CollectingRequestStatus.CANCEL_BY_SYSTEM
                };

                await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessageAsync(publishModel);
            }
        }

        #endregion

    }
}
