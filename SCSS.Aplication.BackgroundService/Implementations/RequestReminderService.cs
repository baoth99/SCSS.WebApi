using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class RequestReminderService : BaseService, IRequestReminderService
    {
        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private IRepository<Account> _accountRepository;

        #endregion

        #region Services

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

        /// <summary>
        /// The cache list service
        /// </summary>
        private readonly ICacheListService _cacheListService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestReminderService"/> class.
        /// </summary>
        /// <param name="SQSPublisherService">The SQS publisher service.</param>
        /// <param name="cacheListService">The cache list service.</param>
        public RequestReminderService(IUnitOfWork unitOfWork, ISQSPublisherService SQSPublisherService, ICacheListService cacheListService) : base(unitOfWork)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _SQSPublisherService = SQSPublisherService;
            _cacheListService = cacheListService;
        }

        #endregion

        #region Remind Collecting Request

        /// <summary>
        /// Reminds the collecting request.
        /// </summary>
        public async Task RemindCollectingRequest()
        {
            var cacheList = await _cacheListService.CollectingRequestReminderCache.GetAllAsync();

            if (cacheList.Any())
            {
                foreach (var item in cacheList)
                {
                    if (item.RequestDate.Value.Date.IsCompareDateTimeEqual(DateTimeVN.DATE_NOW) &&
                        item.RemindTime.IsCompareTimeSpanLessOrEqual(DateTimeVN.TIMESPAN_NOW))
                    {
                        var collectorInfo = _accountRepository.GetAsNoTracking(x => x.Id.Equals(item.CollectorId));

                        if (collectorInfo != null)
                        {
                            await _cacheListService.CollectingRequestReminderCache.RemoveAsync(item);

                            var notification = new NotificationMessageQueueModel()
                            {
                                AccountId = collectorInfo.Id,
                                Title = NotificationMessage.CollectorReminderTitle(item.CollectingRequestCode), 
                                Body = NotificationMessage.CollectorReminderBody(item.CollectingRequestCode, item.AddressName),
                                DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.CollectingRequestScreen, item.Id.ToString()),
                                DeviceId = collectorInfo.DeviceId
                            };

                            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessageAsync(notification);
                            System.Console.WriteLine("DOne !!");
                        }
                    }
                }
            }
        }

        #endregion
    }
}
