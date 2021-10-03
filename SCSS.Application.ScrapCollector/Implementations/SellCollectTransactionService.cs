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

        #endregion

        #region Services

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

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
        public SellCollectTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                             IStringCacheService cacheService, ISQSPublisherService SQSPublisherService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _sellCollectTransactionDetailRepository = unitOfWork.SellCollectTransactionDetailRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _transactionAwardAmountRepository = unitOfWork.TransactionAwardAmountRepository;
            _SQSPublisherService = SQSPublisherService;
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
                TransactionFeePercent = transactionFeePercent
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

            var awardPoints = _transactionAwardAmountRepository.GetManyAsNoTracking(x => x.IsActive);
            var awardPointForSeller = awardPoints.Where(x => x.TransactionType == AccountRole.SELLER).FirstOrDefault();


            var sellCollectTransactionEntity = new SellCollectTransaction()
            {
                CollectingRequestId = model.CollectingRequestId,
                Total = model.Total,
                TransactionServiceFee = model.TransactionServiceFee,
                AwardPoint = NumberConstant.Zero
            };

            // Get AwardPoint
            //var awardPoint = model.Total
            if (awardPoints != null)
            {
                var awardPoint = (model.Total / awardPointForSeller.AppliedAmount.Value) * awardPointForSeller.Amount.Value;
                sellCollectTransactionEntity.AwardPoint = (int)awardPoint;
            }
            var insertEntity =  _sellCollectTransactionRepository.Insert(sellCollectTransactionEntity);

            // Insert SellCollectTransactionDetail
            var transactionDetails = model.ScrapCategoryItems.Select(x => new SellCollectTransactionDetail()
            {
                CollectorCategoryDetailId = x.CollectorCategoryDetailId,
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
                    DataCustom = null, // TODO:
                    NotiType = CollectingRequestStatus.COMPLETED
                },
                new NotificationMessageQueueModel()
                {
                    AccountId = sellerAccount.Id,
                    DeviceId = sellerAccount.DeviceId,
                    Title = NotificationMessage.CompletedSellerCRTitle, 
                    Body = NotificationMessage.CompletedSellerCRBody(collectingRequest.CollectingRequestCode), 
                    DataCustom = null, // TODO:
                    NotiType = CollectingRequestStatus.COMPLETED
                }
            };

            await _SQSPublisherService.NotificationMessageQueuePublisher.SendMessagesAsync(notifications);

            return BaseApiResponse.OK();
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
