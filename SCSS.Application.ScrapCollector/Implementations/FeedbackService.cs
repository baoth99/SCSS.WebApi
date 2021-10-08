using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.FeedbackModels;
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
    public class FeedbackService : BaseService, IFeedbackService
    {
        #region Repositories

        /// <summary>
        /// The feedback repository
        /// </summary>
        private readonly IRepository<Feedback> _feedbackRepository;

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private IRepository<Account> _accountRepository;

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private IRepository<DealerInformation> _dealerInformationRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public FeedbackService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
        }

        #endregion

        #region Create Dealer Feedback

        /// <summary>
        /// Creates the dealer feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateDealerFeedback(FeedbackTransactionCreateModel model)
        {
            var transaction = _collectDealTransactionRepository.GetAsNoTracking(x => x.Id.Equals(model.CollectDealTransId));

            if (transaction == null)
            {
                return BaseApiResponse.NotFound();
            }

            var feedbackEntity = await _feedbackRepository.GetAsyncAsNoTracking(x => x.CollectDealTransactionId.Equals(model.CollectDealTransId));

            if (feedbackEntity == null)
            {
                // Check Time that Seller Can Give feedback
                var isMoreThan = DateTimeUtils.IsMoreThanPastDays(transaction.CreatedTime, NumberConstant.Five);

                if (isMoreThan)
                {
                    return BaseApiResponse.Error(SystemMessageCode.TimeUp);
                }

                var entity = new Feedback()
                {
                    CollectDealTransactionId = model.CollectDealTransId,
                    Type = FeedbackType.Transaction,
                    SellingAccountId = transaction.CollectorAccountId,
                    BuyingAccountId = transaction.DealerAccountId,
                    Rate = model.Rate.ToFloatValue(),
                    SellingReview = model.Review
                };

                _feedbackRepository.Insert(entity);
            }
            else
            {
                if (!ValidatorUtil.IsBlank(feedbackEntity.SellingReview) || feedbackEntity.Rate != null)
                {
                    return BaseApiResponse.Error(SystemMessageCode.FixedData);
                }

                feedbackEntity.SellingReview = model.Review;
                feedbackEntity.Rate = model.Rate;

                _feedbackRepository.Update(feedbackEntity);
            }

            await UnitOfWork.CommitAsync();

            await UpdateDealerRating(transaction.DealerAccountId);

            return BaseApiResponse.OK();
        }

        /// <summary>
        /// Updates the dealer rating.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        private async Task UpdateDealerRating(Guid? dealerId)
        {
            var feedbacks = _feedbackRepository.GetManyAsNoTracking(x => x.BuyingAccountId.Equals(dealerId)).Select(x => x.Rate);
            var rating = await feedbacks.AverageAsync();

            var dealer = _dealerInformationRepository.GetAsNoTracking(x => x.DealerAccountId.Equals(dealerId));
            dealer.Rating = rating.Value;

            _dealerInformationRepository.Update(dealer);

            await UnitOfWork.CommitAsync();
        }

        #endregion Create Dealer Feedback

        #region Create Feedback To Admin

        /// <summary>
        /// Creates the feedback to admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateFeedbackToAdmin(FeedbackAdminCreateModel model)
        {
            var transaction = _collectDealTransactionRepository.GetAsNoTracking(x => x.Id.Equals(model.CollectDealTransactionId));

            if (transaction == null)
            {
                return BaseApiResponse.NotFound();
            }

            var feedbackEntity = await _feedbackRepository.GetAsyncAsNoTracking(x => x.CollectDealTransactionId.Equals(model.CollectDealTransactionId));

            if (feedbackEntity == null)
            {
                // Check Time that Collector Can Give feedback
                var betweenDays = DateTimeUtils.IsMoreThanPastDays(transaction.CreatedTime, NumberConstant.Five);

                if (betweenDays)
                {
                    return BaseApiResponse.Error(SystemMessageCode.TimeUp);
                }

                var entity = new Feedback()
                {
                    CollectDealTransactionId = model.CollectDealTransactionId,
                    Type = FeedbackType.FeedbackToAdmin,
                    SellingAccountId = UserAuthSession.UserSession.Id,
                    SellingFeedback = StringUtils.GetString(model.SellingFeedback)
                };

                _feedbackRepository.Insert(entity);
            }
            else
            {
                if (!ValidatorUtil.IsBlank(feedbackEntity.SellingFeedback))
                {
                    return BaseApiResponse.Error(SystemMessageCode.FixedData);
                }

                feedbackEntity.Type = FeedbackType.FeedbackToAdmin;
                feedbackEntity.SellingFeedback = model.SellingFeedback;

                _feedbackRepository.Update(feedbackEntity);
            }

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
