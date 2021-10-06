using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models;
using SCSS.AWSService.Models.SQSModels;
using SCSS.AWSService.RedisCacheHandler;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using StackExchange.Redis;
using System;

namespace SCSS.AWSService.Implementations
{
    public class CacheListService : IDisposable, ICacheListService
    {
        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region Services

        /// <summary>
        /// The redis database
        /// </summary>
        private readonly IConnectionMultiplexer connection;

        #endregion

        #region Private variable

        private ICacheListHandler<PendingCollectingRequestCacheModel> _pendingCollectingRequestCache;

        private ICacheListHandler<ImageCacheModel> _imageSliderCache;

        private ICacheListHandler<CollectingRequestReminderCacheModel> _collectingRequestReminderCache;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CacheListService"/> class.
        /// </summary>
        /// <param name="Connection">The connection.</param>
        public CacheListService(IConnectionMultiplexer Connection)
        {
            connection = Connection;
        }

        #endregion

        #region Publish Access

        public ICacheListHandler<PendingCollectingRequestCacheModel> PendingCollectingRequestCache => _pendingCollectingRequestCache ??= (_pendingCollectingRequestCache = new CacheListHandler<PendingCollectingRequestCacheModel>(connection, AppSettingValues.RedisDB03, CacheRedisKey.PendingCollectingRequest));

        public ICacheListHandler<ImageCacheModel> ImageSliderCache => _imageSliderCache ??= (_imageSliderCache = new CacheListHandler<ImageCacheModel>(connection, AppSettingValues.RedisDB01, CacheRedisKey.ImageSlider));

        public ICacheListHandler<CollectingRequestReminderCacheModel> CollectingRequestReminderCache => _collectingRequestReminderCache ??= (_collectingRequestReminderCache = new CacheListHandler<CollectingRequestReminderCacheModel>(connection, AppSettingValues.RedisDB04, CacheRedisKey.CollectingRequestReminders));

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }
            }
            this.Disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
