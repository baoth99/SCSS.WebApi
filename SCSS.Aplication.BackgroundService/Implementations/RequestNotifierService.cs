using Microsoft.EntityFrameworkCore;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Aplication.BackgroundService.Models.RequestNotifierModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class RequestNotifierService : BaseService, IRequestNotifierService
    {

        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The collector coordinate repository
        /// </summary>
        private readonly IRepository<CollectorCoordinate> _collectorCoordinateRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The role repository
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        #endregion

        #region Services

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

        /// <summary>
        /// The string cache service
        /// </summary>
        private readonly IStringCacheService _cacheService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="RequestNotifierService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="SQSPublisherService">The SQS publisher service.</param>
        /// <param name="cacheService">The cache service.</param>
        public RequestNotifierService(IUnitOfWork unitOfWork, ISQSPublisherService SQSPublisherService, IStringCacheService cacheService) : base(unitOfWork)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _collectorCoordinateRepository = unitOfWork.CollectorCoordinateRepository;
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _SQSPublisherService = SQSPublisherService;
            _cacheService = cacheService;
        }

        #endregion

        #region Get Nearest Distance

        /// <summary>
        /// Gets the nearest distance.
        /// </summary>
        /// <returns></returns>
        private async Task<double> GetNearestDistance(CacheRedisKey redisKey)
        {
            if (redisKey != CacheRedisKey.NearestDistance && redisKey != CacheRedisKey.NearestDistanceOfAppointment)
            {
                throw new ArgumentException("CacheRedisKey is not correct", nameof(redisKey));
            }

            var distance = await _cacheService.GetStringCacheAsync(redisKey);

            if (ValidatorUtil.IsBlank(distance))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();

                if (entity == null)
                {
                    return NumberConstant.Five;
                }

                if (redisKey == CacheRedisKey.NearestDistance)
                {
                    var nerestDistance = entity.NearestDistance;
                    await _cacheService.SetStringCacheAsync(redisKey, nerestDistance.ToString());

                    return nerestDistance.Value;
                }

                if (redisKey == CacheRedisKey.NearestDistanceOfAppointment)
                {
                    var nerestDistanceOfAppointment = entity.NearestDistanceOfAppointment;
                    await _cacheService.SetStringCacheAsync(redisKey, nerestDistanceOfAppointment.ToString());

                    return nerestDistanceOfAppointment.Value;
                }
            }

            return distance.ToDouble();
        }

        #endregion

        #region Get Max Number of Collecting Requests that collector can receive 

        /// <summary>
        /// Gets the maximum number collecting request collector receive.
        /// </summary>
        /// <returns></returns>
        public async Task<int> MaxNumberCollectingRequestCollectorReceive()
        {
            var quantity = await _cacheService.GetStringCacheAsync(CacheRedisKey.ReceiveQuantity);
            if (ValidatorUtil.IsBlank(quantity))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.One;
                }
                var receiveQuantity = entity.ReceiveQuantity;

                await _cacheService.SetStringCacheAsync(CacheRedisKey.ReceiveQuantity, receiveQuantity.ToString());

                return receiveQuantity;
            }

            return quantity.ToInt();
        }

        #endregion

        #region Get Priority Rating

        /// <summary>
        /// Priorities the rating.
        /// </summary>
        /// <returns></returns>
        public async Task<float> PriorityRating()
        {
            var rating = await _cacheService.GetStringCacheAsync(CacheRedisKey.PriorityRating);
            if (ValidatorUtil.IsBlank(rating))
            {
                var entity = await UnitOfWork.CollectingRequestConfigRepository.GetManyAsNoTracking(x => x.IsActive).FirstOrDefaultAsync();
                if (entity == null)
                {
                    return NumberConstant.Four;
                }
                var priorityRating = entity.PriorityRating;

                await _cacheService.SetStringCacheAsync(CacheRedisKey.PriorityRating, priorityRating.ToString());

                return priorityRating.Value;
            }
            return rating.ToFloat();
        }

        #endregion

        #region Handle Request Notification

        /// <summary>
        /// Handles the request notification.
        /// </summary>
        /// <param name="models">The models.</param>
        public async Task HandleRequestNotification(List<RequestNotifierRequestModel> models)
        {
            foreach (var item in models)
            {
                var nearestCollectors = await GetNearestCollectors(item.RequestType, item.Latitude, item.Longitude);

                if (nearestCollectors.Any())
                {
                    var messages = nearestCollectors.Select(x => new NotificationMessageQueueModel()
                    {
                        AccountId = x.Id,
                        DeviceId = x.DeviceId,
                        Title = NotificationMessage.RequestGoNowTitle,
                        Body = NotificationMessage.RequestGoNowBody,
                        NotiType = NotificationType.CollectingRequest,
                        ReferenceRecordId = item.CollectingRequestId,
                        DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.CollectingRequestScreen, item.CollectingRequestId.ToString())
                    }).ToList();

                    await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(messages);
                }
            }
        }

        #endregion

        #region Get Nearest Collectors

        /// <summary>
        /// Gets the nearest collectors.
        /// </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longtitude">The longtitude.</param>
        /// <returns></returns>
        private async Task<List<NearestCollectorModel>> GetNearestCollectors(int requestType ,decimal latitude, decimal longtitude)
        {

            var collectorRoleId = _roleRepository.GetAsNoTracking(x => x.Key == AccountRole.COLLECTOR).Id;

            var dataQuery = _accountRepository.GetManyAsNoTracking(x => x.RoleId == collectorRoleId)
                                              .Join(_collectorCoordinateRepository.GetAllAsNoTracking(), x => x.Id, y => y.CollectorAccountId,
                                                    (x, y) => new
                                                    {
                                                        CollectorAccountId = x.Id,
                                                        x.Rating,
                                                        x.DeviceId,
                                                        y.Latitude,
                                                        y.Longitude
                                                    }).ToList();
            if (!dataQuery.Any())
            {
                return CollectionConstants.Empty<NearestCollectorModel>();
            }

            var redisKey = requestType == CollectingRequestType.CURRENT_REQUEST ? CacheRedisKey.NearestDistance : CacheRedisKey.NearestDistanceOfAppointment;

            var nearestDistance = await GetNearestDistance(redisKey);

            var nearestCollectors = dataQuery.Where(x => CoordinateHelper.IsInRadius(x.Latitude, x.Longitude, latitude, longtitude, nearestDistance))
                                            .Select(x => new
                                            {
                                                x.CollectorAccountId,
                                                x.Rating,
                                                x.DeviceId
                                            });

            if (!nearestCollectors.Any())
            {
                return CollectionConstants.Empty<NearestCollectorModel>();
            }

            if (requestType == CollectingRequestType.CURRENT_REQUEST)
            {
                
                // Check collector can recevice collecting request
                var collectingRequestNow = _collectingRequestRepository.GetManyAsNoTracking(x => x.RequestType == CollectingRequestType.CURRENT_REQUEST &&
                                                                                                 x.Status == CollectingRequestStatus.APPROVED &&
                                                                                                 x.CollectingRequestDate.Value.Date.CompareTo(DateTimeVN.DATE_NOW) == NumberConstant.Zero);

                var invalidCollectors = nearestCollectors.Join(collectingRequestNow, x => x.CollectorAccountId, y => y.CollectorAccountId,
                                                            (x, y) => new
                                                            {
                                                                x.CollectorAccountId,
                                                                x.Rating,
                                                                x.DeviceId
                                                            });

                var result = nearestCollectors.Except(invalidCollectors).Select(x => new NearestCollectorModel()
                {
                    Id = x.CollectorAccountId,
                    DeviceId = x.DeviceId
                }).ToList();

                return result;
            }
            else
            {
                var priorityRating = await PriorityRating();

                // Get The Collectors who have hight rating
                var collectorsHaveHighRating = nearestCollectors.Where(x => x.Rating >= priorityRating);

                if (!collectorsHaveHighRating.Any())
                {
                    return CollectionConstants.Empty<NearestCollectorModel>();
                }

                var maxNumberOfRequests = await MaxNumberCollectingRequestCollectorReceive();

                var collectingAppointment = _collectingRequestRepository.GetManyAsNoTracking(x => x.RequestType == CollectingRequestType.MAKE_AN_APPOINTMENT &&
                                                                                                  x.Status == CollectingRequestStatus.APPROVED);

                // Get The Collectors who have received the maximum number of apppointments
                var collectorsHaveMaxNumberAppointment = collectingAppointment.GroupBy(x => x.CollectorAccountId).Where(x => x.Count() >= maxNumberOfRequests).Select(x => x.Key);

                var invalidCollectors = collectorsHaveHighRating.Join(collectorsHaveMaxNumberAppointment, x => x.CollectorAccountId, y => y,
                                                            (x, y) => new
                                                            {
                                                                x.CollectorAccountId,
                                                                x.Rating,
                                                                x.DeviceId
                                                            });

                var result = collectorsHaveHighRating.Except(invalidCollectors).Select(x => new NearestCollectorModel()
                {
                    Id = x.CollectorAccountId,
                    DeviceId = x.DeviceId
                }).ToList();

                return result;
            }
        }

        #endregion

    }
}
