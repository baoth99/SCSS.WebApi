using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.NotificationModels;
using SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.FirebaseService.Interfaces;
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
        /// The notification repository
        /// </summary>
        private IRepository<Notification> _notificationRepository;

        /// <summary>
        /// The feedback repository
        /// </summary>
        private IRepository<Feedback> _feedbackRepository;

        /// <summary>
        /// The transaction service fee percent repository
        /// </summary>
        private IRepository<TransactionServiceFeePercent> _transactionServiceFeePercentRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The transaction award amount repository
        /// </summary>
        private IRepository<TransactionAwardAmount> _transactionAwardAmountRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SellCollectTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        public SellCollectTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,
                                             IFCMService fcmService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _sellCollectTransactionDetailRepository = unitOfWork.SellCollectTransactionDetailRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _notificationRepository = unitOfWork.NotificationRepository;
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _transactionServiceFeePercentRepository = unitOfWork.TransactionServiceFeePercentRepository;
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _transactionAwardAmountRepository = unitOfWork.TransactionAwardAmountRepository;
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

            var transactionFeePercent = _transactionServiceFeePercentRepository.GetManyAsNoTracking(x => x.TransactionType == TransactionType.SELL_COLLECT &&
                                                                                                  x.IsActive).FirstOrDefault().Percent;

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
            var awardPointForSeller = awardPoints.Where(x => x.AppliedObject == AccountRole.SELLER).FirstOrDefault();


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

            var notifications = new List<NotificationCreateModel>()
            {
                new NotificationCreateModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    DeviceId = UserAuthSession.UserSession.DeviceId,
                    Title = "", // TODO:
                    Body = "", // TODO:
                    DataCustom = null
                },
                new NotificationCreateModel()
                {
                    AccountId = sellerAccount.Id,
                    DeviceId = sellerAccount.DeviceId,
                    Title = "", // TODO:
                    Body = "", // TODO:
                    DataCustom = null
                }
            };

            await StoreAndSendManyNotifications(notifications);

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
