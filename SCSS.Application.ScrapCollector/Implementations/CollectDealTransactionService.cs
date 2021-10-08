using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models;
using SCSS.Application.ScrapCollector.Models.CollectDealTransactionModels;
using SCSS.AWSService.Interfaces;
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
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class CollectDealTransactionService : BaseService, ICollectDealTransactionService
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
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The feedback repository
        /// </summary>
        private readonly IRepository<Feedback> _feedbackRepository;

        /// <summary>
        /// The scrap categort repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        #endregion Repositories

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDealTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        /// <param name="cacheService">The cache service.</param>
        public CollectDealTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger,  IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _collectDealTransactionDetailRepository = unitOfWork.CollectDealTransactionDetailRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;

        }

        #endregion Constructor

        #region Get Collect-Deal Transactions

        /// <summary>
        /// Gets the collect deal transactions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectDealTransactions(BaseFilterModel model)
        {
            var collectorId = UserAuthSession.UserSession.Id;
            var dataQuery = _collectDealTransactionRepository.GetManyAsNoTracking(x => x.CollectorAccountId.Equals(collectorId))
                                                             .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.DealerAccountId, y => y.DealerAccountId,
                                                                (x, y) => new
                                                                {
                                                                    TransId = x.Id,
                                                                    x.TransactionCode,
                                                                    y.DealerName,
                                                                    y.DealerImageUrl,
                                                                    x.CreatedTime,
                                                                    x.Total,
                                                                    x.TransactionServiceFee,
                                                                    x.BonusAmount
                                                                }).OrderByDescending(x => x.CreatedTime);

            var totalRecord = await dataQuery.CountAsync();

            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;

            var dataResult = dataQuery.Skip((page - 1) * pageSize).Select(x => new CollectDealTransactionViewModel()
            {
                TransactionId = x.TransId,
                TransactionCode = x.TransactionCode,
                DealerName = x.DealerName,
                DealerImageURL = x.DealerImageUrl,
                Total = ((x.Total - x.TransactionServiceFee) + x.BonusAmount),
                TransactionDate = x.CreatedTime.Value.Date,
                TransactionTime = x.CreatedTime.Value.TimeOfDay.ToStringFormat(TimeSpanFormat.HH_MM),
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion Get Collect-Deal Transactions

        #region Get Collect-Deal Transaction Detail

        /// <summary>
        /// Gets the collect deal transaction detail.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetCollectDealTransactionDetail(Guid transId)
        {
            var transEntity = await _collectDealTransactionRepository.GetAsyncAsNoTracking(x => x.Id.Equals(transId) &&
                                                                                          x.CollectorAccountId.Equals(UserAuthSession.UserSession.Id));

            if (transEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var dealerInfo = _dealerInformationRepository.GetAsNoTracking(x => x.DealerAccountId.Equals(transEntity.DealerAccountId));

            var feedbackInfo = GetFeedbackInfo(transEntity.Id, transEntity.CreatedTime);

            var itemDetails = await GetTransactionInfoDetails(transId);

            var entityResponse = new CollectDealTransactionDetailViewModel()
            {
                TransId = transEntity.Id,
                TransactionCode = transEntity.TransactionCode,
                AwardPoint = transEntity.AwardPoint,
                Total = (transEntity.Total - transEntity.TransactionServiceFee),
                TrasactionDateTime = transEntity.CreatedTime,
                TotalBonus = transEntity.BonusAmount,
                DealerInfo = new TransDealerInformationViewModel()
                {
                    DealerName = dealerInfo?.DealerName,
                    DealerPhone = dealerInfo?.DealerPhone
                },
                Feedback = feedbackInfo,
                ItemDetails = itemDetails
            };

            return BaseApiResponse.OK(entityResponse);
        }

        #endregion

        #region Get Transaction Information Details

        /// <summary>
        /// Gets the transaction information details.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <returns></returns>
        private async Task<List<TransHistoryScrapCategoryViewModel>> GetTransactionInfoDetails(Guid transId)
        {
            var transactionDetailItems = await _collectDealTransactionDetailRepository.GetManyAsNoTracking(x => x.CollectDealTransactionId.Equals(transId))
                                                                                .GroupJoin(_scrapCategoryDetailRepository.GetAllAsNoTracking(), x => x.DealerCategoryDetailId, y => y.Id,
                                                                                    (x, y) => new
                                                                                    {
                                                                                        x.Quantity,
                                                                                        x.Total,
                                                                                        x.BonusAmount,
                                                                                        x.PromotionId,
                                                                                        ScrapCategoryDetail = y
                                                                                    }).SelectMany(x => x.ScrapCategoryDetail.DefaultIfEmpty(), (x, y) => new
                                                                                    {
                                                                                        x.Quantity,
                                                                                        x.Total,
                                                                                        x.BonusAmount,
                                                                                        x.PromotionId,
                                                                                        y.ScrapCategoryId,
                                                                                        y.Unit
                                                                                    })
                                                                                .GroupJoin(_scrapCategoryRepository.GetAllAsNoTracking(), x => x.ScrapCategoryId, y => y.Id,
                                                                                    (x, y) => new
                                                                                    {
                                                                                        x.Quantity,
                                                                                        x.Total,
                                                                                        x.Unit,
                                                                                        x.BonusAmount,
                                                                                        x.PromotionId,
                                                                                        ScrapCategory = y
                                                                                    }).SelectMany(x => x.ScrapCategory.DefaultIfEmpty(), (x, y) => new
                                                                                    {
                                                                                        x.Quantity,
                                                                                        x.Total,
                                                                                        x.BonusAmount,
                                                                                        x.Unit,
                                                                                        x.PromotionId,
                                                                                        ScrapCategoryName = y.Name
                                                                                    }).Select(x => new TransHistoryScrapCategoryViewModel()
                                                                                    {
                                                                                        ScrapCategoryName = x.ScrapCategoryName,
                                                                                        Quantity = x.Quantity,
                                                                                        Total = x.Total,
                                                                                        BonusAmount = x.BonusAmount,
                                                                                        IsBonus = !ValidatorUtil.IsBlank(x.PromotionId)
                                                                                    }).ToListAsync();
            return transactionDetailItems;
        }

        #endregion


        #region Get Feedback Info

        /// <summary>
        /// Gets the feedback information.
        /// </summary>
        /// <param name="transId">The trans identifier.</param>
        /// <param name="createdTransTime">The created trans time.</param>
        /// <returns></returns>
        private CollectDealTransFeedbackViewModel GetFeedbackInfo(Guid transId, DateTime? createdTransTime)
        {
            var betweenDays = DateTimeUtils.IsMoreThanPastDays(createdTransTime, NumberConstant.Five);
            if (betweenDays)
            {
                return new CollectDealTransFeedbackViewModel()
                {
                    FeedbackStatus = FeedbackStatus.TimeUpToGiveFeedback
                };
            }

            var feedback = _feedbackRepository.GetAsNoTracking(x => x.CollectDealTransactionId.Equals(transId));

            if (feedback == null)
            {
                return new CollectDealTransFeedbackViewModel()
                {
                    FeedbackStatus = FeedbackStatus.HaveNotGivenFeedbackYet
                };
            }

            return new CollectDealTransFeedbackViewModel()
            {
                FeedbackStatus = FeedbackStatus.HaveGivenFeedback,
                RatingFeedback = feedback.Rate
            };
        }

        #endregion
    }
}
