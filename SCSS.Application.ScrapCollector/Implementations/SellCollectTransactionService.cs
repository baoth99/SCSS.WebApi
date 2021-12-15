using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public partial class SellCollectTransactionService : BaseService, ISellCollectTransactionService
    {
        #region Repositories

        /// <summary>
        /// The sell collect transaction repository
        /// </summary>
        private IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        /// <summary>
        /// The sell collect transaction detail repository
        /// </summary>
        private IRepository<SellCollectTransactionDetail> _sellCollectTransactionDetailRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private IRepository<Account> _accountRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The transaction award amount repository
        /// </summary>
        private IRepository<TransactionAwardAmount> _transactionAwardAmountRepository;

        /// <summary>
        /// The complaint repository
        /// </summary>
        private IRepository<Complaint> _complaintRepository;

        /// <summary>
        /// The collector complaint repository
        /// </summary>
        private IRepository<CollectorComplaint> _collectorComplaintRepository;

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
        /// Initializes a new instance of the <see cref="SellCollectTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="SQSPublisherService">The SQS publisher service.</param>
        /// <param name="cacheListService">The cache list service.</param>
        public SellCollectTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                             IStringCacheService cacheService, ISQSPublisherService SQSPublisherService,
                                             ICacheListService cacheListService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _sellCollectTransactionDetailRepository = unitOfWork.SellCollectTransactionDetailRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _transactionAwardAmountRepository = unitOfWork.TransactionAwardAmountRepository;
            _complaintRepository = unitOfWork.ComplaintRepository;
            _collectorComplaintRepository = unitOfWork.CollectorComplaintRepository;
            _SQSPublisherService = SQSPublisherService;
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _cacheListService = cacheListService;
        }

        #endregion

        #region Get Transaction Information to Review
        
        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionInfoReview(Guid collectingRequestId)
        {
            var collectorId = UserAuthSession.UserSession.Id;

            var collectingRequest = await _collectingRequestRepository.GetAsyncAsNoTracking(x => x.Id.Equals(collectingRequestId) &&
                                                                    x.CollectorAccountId.Equals(collectorId) &&
                                                                    x.Status == CollectingRequestStatus.APPROVED);
            if (collectingRequest == null)
            {
                return BaseApiResponse.NotFound();
            }

            var sellerInfo = _accountRepository.GetById(collectingRequest.SellerAccountId);

            var transactionFeePercent = await TransactionServiceFeePercent(CacheRedisKey.SellCollectTransactionServiceFee);

            var dataResult = new TransactionInfoReviewViewModel()
            {
                CollectingRequestId = collectingRequest.Id,
                CollectingRequestCode = collectingRequest.CollectingRequestCode,
                SellerName = sellerInfo.Name,
                SellerPhone = sellerInfo.Phone,
                TransactionFeePercent = transactionFeePercent / 100
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        #region Create Collecting Transaction

        /// <summary>
        /// Creates the collecting transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateCollectingTransaction(SellCollectTransactionCreateModel model)
        {
            var collectingRequest = _collectingRequestRepository.GetById(model.CollectingRequestId);

            if (collectingRequest == null)
            {
                return BaseApiResponse.NotFound();
            }

            if (!collectingRequest.CollectorAccountId.Equals(UserAuthSession.UserSession.Id) || collectingRequest.Status != CollectingRequestStatus.APPROVED )
            {
                return BaseApiResponse.Error();
            }
            // Remove Remider in ReminderCache
            var remiderCaches = _cacheListService.CollectingRequestReminderCache.GetMany(x => x.Id.Equals(collectingRequest.Id));
            await _cacheListService.CollectingRequestReminderCache.RemoveRangeAsync(remiderCaches);


            // Create Entity
            var sellCollectTransactionEntity = new SellCollectTransaction()
            {
                CollectingRequestId = model.CollectingRequestId,
                Total = model.Total,
                TransactionServiceFee = model.TransactionServiceFee,
                AwardPoint = NumberConstant.Zero
            };

            var awardPointForSeller = await TransactionAwardAmount(CacheRedisKey.SellCollectTransactionAwardAmount);

            // Get AwardPoint
            //var awardPoint = model.Total
            if (awardPointForSeller != null)
            {
                var awardPoint = (model.Total / awardPointForSeller.AppliedAmount) * awardPointForSeller.Amount;
                sellCollectTransactionEntity.AwardPoint = (int)awardPoint;
            }
            var insertEntity =  _sellCollectTransactionRepository.Insert(sellCollectTransactionEntity);


            var sellCollecTransDetails = model.ScrapCategoryItems.GroupJoin(_scrapCategoryDetailRepository.GetAllAsNoTracking(),
                                                        x => x.CollectorCategoryDetailId, y => y.Id, (x, y) => new
                                                        {
                                                            x.CollectorCategoryDetailId,
                                                            x.Total,
                                                            x.Quantity,
                                                            x.Price,
                                                            ScrapCategoryDetail = y
                                                        })
                                                        .SelectMany(x => x.ScrapCategoryDetail.DefaultIfEmpty(), (x, y) => new
                                                        {
                                                            x.CollectorCategoryDetailId,
                                                            x.Total,
                                                            x.Quantity,
                                                            x.Price,
                                                            ScrapCategoryId = y?.ScrapCategoryId
                                                        })
                                                     .GroupJoin(_scrapCategoryRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id)),
                                                        x => x.ScrapCategoryId, y => y.Id, (x, y) => new
                                                        {
                                                            x.CollectorCategoryDetailId,
                                                            x.Total,
                                                            x.Quantity,
                                                            x.Price,
                                                            ScrapCategory = y
                                                        })
                                                        .SelectMany(x => x.ScrapCategory.DefaultIfEmpty(), (x, y) => new
                                                        {
                                                            x.CollectorCategoryDetailId,
                                                            x.Total,
                                                            x.Quantity,
                                                            x.Price,
                                                            ScrapCategoryName = y?.Name
                                                        }).ToList();


            // Insert SellCollectTransactionDetail
            var transactionDetails = sellCollecTransDetails.Select(x => new SellCollectTransactionDetail()
            {
                CollectorCategoryDetailId = x.CollectorCategoryDetailId,
                ScrapCategoryName = x.ScrapCategoryName,
                SellCollectTransactionId = insertEntity.Id,
                Quantity = x.Quantity,
                Price = x.Price,
                Total = x.Total
            }).ToList();

            _sellCollectTransactionDetailRepository.InsertRange(transactionDetails);

            // Update Collecting Request Status
            collectingRequest.Status = CollectingRequestStatus.COMPLETED;
            _collectingRequestRepository.Update(collectingRequest);

            // Update seller's award point
            var sellerAccount = _collectingRequestRepository.GetManyAsNoTracking(x => x.Id.Equals(model.CollectingRequestId))
                                                                .Join(_accountRepository.GetAll(), x => x.SellerAccountId, y => y.Id,
                                                                     (x, y) => y).FirstOrDefault();
            sellerAccount.TotalPoint += sellCollectTransactionEntity.AwardPoint;

            _accountRepository.Update(sellerAccount);

            var serviceTransactionNow = UnitOfWork.ServiceTransactionRepository.GetAsNoTracking(x => x.CollectorId.Equals(UserAuthSession.UserSession.Id) &&
                                                                                                     !x.IsFinished);
            if (serviceTransactionNow != null)
            {
                serviceTransactionNow.Amount += insertEntity.TransactionServiceFee;
                UnitOfWork.ServiceTransactionRepository.Update(serviceTransactionNow);
            }

            await UnitOfWork.CommitAsync();
            // Send Notification to Seller

            var notifications = new List<NotificationMessageQueueModel>()
            {
                new NotificationMessageQueueModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    DeviceId = UserAuthSession.UserSession.DeviceId,
                    Title = NotificationMessage.CompletedSellerCRTitle, 
                    Body = NotificationMessage.CompletedCollectorCRBody(collectingRequest.CollectingRequestCode), 
                    DataCustom = DictionaryConstants.FirebaseCustomData(CollectorAppScreen.CollectingRequestScreen, collectingRequest.Id.ToString(), CollectingRequestStatus.COMPLETED.ToString()),
                    NotiType = NotificationType.CollectingRequest,
                    ReferenceRecordId = collectingRequest.Id
                },
                new NotificationMessageQueueModel()
                {
                    AccountId = sellerAccount.Id,
                    DeviceId = sellerAccount.DeviceId,
                    Title = NotificationMessage.CompletedSellerCRTitle, 
                    Body = NotificationMessage.CompletedSellerCRBody(collectingRequest.CollectingRequestCode), 
                    DataCustom = DictionaryConstants.FirebaseCustomData(SellerAppScreen.ActivityScreen, collectingRequest.Id.ToString(), CollectingRequestStatus.COMPLETED.ToString()),
                    NotiType = NotificationType.CollectingRequest,
                    ReferenceRecordId = collectingRequest.Id,
                }
            };

            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(notifications);

            var response = new
            {
                Id = insertEntity.Id
            };

            return BaseApiResponse.OK(response);
        }

        #endregion

        #region Get Scrap Category To Create transaction

        /// <summary>
        /// Gets the scrap category transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionScrapCategories()
        {
            var dataQuery = _scrapCategoryRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                              x.Status == ScrapCategoryStatus.ACTIVE);
            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new TransactionScrapCategoryViewModel()
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Transaction Scrap Category Unit

        /// <summary>
        /// Gets the transaction scrap category detail.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionScrapCategoryDetail(Guid id)
        {
            if (!_scrapCategoryRepository.IsExisted(x => x.Id.Equals(id) && x.AccountId.Equals(UserAuthSession.UserSession.Id)))
            {
                return BaseApiResponse.NotFound();
            }

            var dataQuery = _scrapCategoryDetailRepository.GetManyAsNoTracking(x => x.ScrapCategoryId.Equals(id) &&
                                                                                    x.Status == ScrapCategoryStatus.ACTIVE);

            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new TransactionScrapCategoryDetailViewModel()
            {
                Id = x.Id,
                Price = x.Price.ToLongValue(),
                Unit = x.Unit
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion
    }
}
