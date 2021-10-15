using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels;
using SCSS.AWSService.Interfaces;
using SCSS.AWSService.Models.SQSModels;
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
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public partial class CollectDealTransactionService : BaseService, ICollectDealTransactionService
    {
        #region Repositories

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        /// <summary>
        /// The collect deal transaction detail repository
        /// </summary>
        private readonly IRepository<CollectDealTransactionDetail> _collectDealTransactionDetailRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The promotion repository
        /// </summary>
        private readonly IRepository<Promotion> _promotionRepository;

        /// <summary>
        /// The role repository
        /// </summary>
        private readonly IRepository<Role> _roleRepository;

        /// <summary>
        /// The complaint respository
        /// </summary>
        private readonly IRepository<Complaint> _complaintRespository;

        #endregion

        #region Services

        /// <summary>
        /// The SQS publisher service
        /// </summary>
        private readonly ISQSPublisherService _SQSPublisherService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDealTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="SQSPublisherService">The SQS publisher service.</param>
        public CollectDealTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService, ISQSPublisherService SQSPublisherService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _collectDealTransactionDetailRepository = unitOfWork.CollectDealTransactionDetailRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _roleRepository = unitOfWork.RoleRepository;
            _promotionRepository = unitOfWork.PromotionRepository;
            _complaintRespository = unitOfWork.ComplaintRepository;
            _SQSPublisherService = SQSPublisherService;
        }

        #endregion

        #region Auto Complete Collector Phone

        /// <summary>
        /// Automatics the complete collector phone.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> AutoCompleteCollectorPhone()
        {
            var collectorRole = UnitOfWork.RoleRepository.Get(x => x.Key == AccountRole.COLLECTOR).Id;

            var dataQuery = _accountRepository.GetManyAsNoTracking(x => x.RoleId.Equals(collectorRole) &&
                                                                        x.Status == AccountStatus.ACTIVE);
            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new AutoCompleteCollectorPhoneModel()
            {
                Id = x.Id,
                Phone = x.Phone
            });

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Transaction Info Review

        /// <summary>
        /// Gets the transaction information review.
        /// </summary>
        /// <param name="collectorId">The collector identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionInfoReview(Guid collectorId)
        {
            var collectorRole = UnitOfWork.RoleRepository.Get(x => x.Key == AccountRole.COLLECTOR).Id;
            var collectorInfo = _accountRepository.GetAsNoTracking(x => x.Id.Equals(collectorId) &&
                                                                        x.Status == AccountStatus.ACTIVE &&
                                                                        x.RoleId.Equals(collectorRole));

            if (collectorInfo == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var serviceFee = await TransactionServiceFeePercent(CacheRedisKey.CollectDealTransactionServiceFee);

            var dataResult = new TransactionInfoReviewViewModel()
            {
                CollectorId = collectorInfo.Id,
                CollectorName = collectorInfo.Name,
                CollectorPhone = collectorInfo.Phone,
                TransactionFeePercent = serviceFee
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

        #region Get Transaction Scrap Categories

        /// <summary>
        /// Gets the scrap category transaction.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetTransactionScrapCategories()
        {
            var dataQuery = _scrapCategoryRepository.GetManyAsNoTracking(x => x.AccountId.Equals(UserAuthSession.UserSession.Id) &&
                                                                              x.Status == ScrapCategoryStatus.ACTIVE)
                                                    .GroupJoin(_promotionRepository.GetManyAsNoTracking(x => x.Status == PromotionStatus.ACTIVE), x => x.Id, y => y.DealerCategoryId,
                                                                        (x, y) => new
                                                                        {
                                                                            ScrapCategoryId = x.Id,
                                                                            ScrapCategoryName = x.Name,
                                                                            Promotion = y
                                                                        })
                                                            .SelectMany(x => x.Promotion.DefaultIfEmpty(),
                                                                        (x, y) => new
                                                                        {
                                                                            x.ScrapCategoryId,
                                                                            x.ScrapCategoryName,
                                                                            PromotionId = y.Id,
                                                                            PromotionCode = y.Code,
                                                                            y.AppliedAmount,
                                                                            y.BonusAmount
                                                                        });
            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new TransactionScrapCategoryViewModel()
            {
                Id = x.ScrapCategoryId,
                Name = x.ScrapCategoryName,
                PromotionId = x.PromotionId,
                PromotionCode = x.PromotionCode,
                BonusAmount = x.BonusAmount,
                AppliedAmount = x.AppliedAmount
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Transaction Scrap Category Detail

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

        #region Create Transaction

        /// <summary>
        /// Creates the collect deal transaction.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateCollectDealTransaction(TransactionCreateModel model)
        {
            var transactionCode = await GenerateTransactionCode();

            var transactionEntity = new CollectDealTransaction()
            {
                DealerAccountId = UserAuthSession.UserSession.Id,
                CollectorAccountId = model.CollectorId,
                TransactionCode = transactionCode,
                Total = model.Total,
                TransactionServiceFee = model.TransactionFee,
                AwardPoint = NumberConstant.Zero,
                BonusAmount = model.TotalBonus,
            };

            var awardPointForCollector = await TransactionAwardAmount(CacheRedisKey.CollectDealTransactionAwardAmount);

            if (awardPointForCollector != null)
            {
                var awardPoint = (model.Total / awardPointForCollector.AppliedAmount) * awardPointForCollector.Amount;
                transactionEntity.AwardPoint = (int)awardPoint;
            }

            var insertEntity = _collectDealTransactionRepository.Insert(transactionEntity);

            // Insert CollectDeal Transaction Detail
            var transactionDetail = model.Items.Select(x => new CollectDealTransactionDetail()
            {
                DealerCategoryDetailId = x.DealerCategoryDetailId,
                CollectDealTransactionId = insertEntity.Id,
                Quantity = x.Quantity,
                Price = x.Price,
                Total = x.Total,
                BonusAmount = x.BonusAmount,
                PromotionId = x.PromotionId
            }).ToList();

            _collectDealTransactionDetailRepository.InsertRange(transactionDetail);

            // Update Collector's Award point
            var collectorAccount = _accountRepository.GetById(model.CollectorId);
            collectorAccount.TotalPoint += transactionEntity.AwardPoint;

            _accountRepository.Update(collectorAccount);

            // Create Complaint
            var complaint = new Complaint()
            {
                CollectDealTransactionId = insertEntity.Id
            };

            _complaintRespository.Insert(complaint);

            await UnitOfWork.CommitAsync();

            // Send Notification to Collector

            var notifications = new List<NotificationMessageQueueModel>()
            {
                new NotificationMessageQueueModel()
                {
                    AccountId = UserAuthSession.UserSession.Id,
                    DeviceId = UserAuthSession.UserSession.DeviceId,
                    NotiType = CollectingRequestStatus.COMPLETED,
                    Title = "",
                    Body = "",
                    DataCustom = null
                },
                new NotificationMessageQueueModel()
                {
                    AccountId = collectorAccount.Id,
                    DeviceId = collectorAccount.DeviceId,
                    NotiType = CollectingRequestStatus.COMPLETED,
                    Title = "",
                    Body = "",
                    DataCustom = null
                }
            };

            // TODO:
            return BaseApiResponse.OK();
        }


        /// <summary>
        /// Generates the transaction code.
        /// </summary>
        /// <returns></returns>
        private async Task<string> GenerateTransactionCode()
        {
            var collectingRequestCount = await _collectDealTransactionRepository.GetAllAsNoTracking().CountAsync();
            var dateTimeCode = DateTimeUtils.GetDateTimeNowCode();
            return string.Format(GenerationCodeFormat.COLLECT_DEAL_TRANSACTION_CODE, dateTimeCode, collectingRequestCount);

        }

        #endregion

    }
}
