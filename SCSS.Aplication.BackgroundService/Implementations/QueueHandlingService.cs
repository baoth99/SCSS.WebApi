using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.QueueEngine.QueueEngines;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class QueueHandlingService : IQueueHandlingService
    {
        #region Services

        /// <summary>
        /// The queue engine factory
        /// </summary>
        private readonly IQueueEngineFactory _queueEngineFactory;

        /// <summary>
        /// The cache list service
        /// </summary>
        private readonly ICacheListService _cacheListService;

        #endregion


        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="QueueHandlingService"/> class.
        /// </summary>
        /// <param name="queueEngineFactory">The queue engine factory.</param>
        /// <param name="cacheListService">The cache list service.</param>
        public QueueHandlingService(IQueueEngineFactory queueEngineFactory, ICacheListService cacheListService)
        {
            _queueEngineFactory = queueEngineFactory;
            _cacheListService = cacheListService;
        }

        #endregion

        #region HandleCollectingRequestReminderQueue

        /// <summary>
        /// Handles the collecting request reminder queue.
        /// </summary>
        public async Task HandleCollectingRequestReminderQueue()
        {
            var queueList = _queueEngineFactory.CollectingRequestReminderQueueRepos.ConsumeDequeueQueue(NumberConstant.Ten);

            if (queueList.Any())
            {
                foreach (var item in queueList)
                {
                    var notiTime = TimeSpan.Parse(AppSettingValues.NoticeToCollector);

                    var remindTimeFrom = item.FromTime.Value.Subtract(notiTime);
                    var remindTimeTo = item.ToTime.Value.Add(notiTime);

                    var cacheModels = new List<CollectingRequestReminderCacheModel>()
                    {
                        new CollectingRequestReminderCacheModel()
                        {
                            Id = item.Id,
                            CollectingRequestCode = item.CollectingRequestCode,
                            CollectorId = item.CollectorId,
                            RequestDate = item.RequestDate,
                            RemindTime = remindTimeFrom,
                        },
                        new CollectingRequestReminderCacheModel()
                        {
                            Id = item.Id,
                            CollectingRequestCode = item.CollectingRequestCode,
                            CollectorId = item.CollectorId,
                            RequestDate = item.RequestDate,
                            RemindTime = remindTimeTo,
                        }
                    };

                    await _cacheListService.CollectingRequestReminderCache.PushManyAsync(cacheModels);
                }
            }
        }

        #endregion
    }
}
