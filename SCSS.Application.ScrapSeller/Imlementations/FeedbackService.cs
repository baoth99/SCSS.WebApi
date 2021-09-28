using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.FeedbackModels;
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
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public class FeedbackService : BaseService, IFeedbackService
    {
        #region Repositories

        /// <summary>
        /// The feedback repository
        /// </summary>
        private IRepository<Feedback> _feedbackRepository;

        /// <summary>
        /// The sell collect transaction repository
        /// </summary>
        private IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public FeedbackService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, ICacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
        }

        #endregion

        #region Create Sell-Collect Transaction Feedback

        /// <summary>
        /// Creates the transaction feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateSellCollectTransactionFeedback(SellCollecTransFeedbackCreateModel model)
        {
            var transaction = _sellCollectTransactionRepository.GetAsNoTracking(x => x.Id.Equals(model.SellCollectTransactionId));

            if (transaction == null)
            {
                return BaseApiResponse.NotFound();
            }

            
            var feedbackEntity = await _feedbackRepository.GetAsyncAsNoTracking(x => x.SellCollectTransactionId.Equals(model.SellCollectTransactionId));

            if (feedbackEntity == null)
            {
                // Check Time that Seller Can Give feedback
                var betweenDays = DateTimeUtils.IsMoreThanPastDays(transaction.CreatedTime, NumberConstant.Five);

                if (betweenDays)
                {
                    return BaseApiResponse.Error(SystemMessageCode.TimeUp);
                }

                var entity = new Feedback()
                {
                    SellCollectTransactionId = model.SellCollectTransactionId,
                    Type = FeedbackType.Transaction,
                    SellingAccountId = UserAuthSession.UserSession.Id,

                    Rate = model.Rate.ToFloatValue(),
                    SellingReview = StringUtils.GetString(model.SellingReview),
                };

                _feedbackRepository.Insert(entity);
            }
            else
            {
                if (!ValidatorUtil.IsBlank(feedbackEntity.SellingReview) || feedbackEntity.Rate != null)
                {
                    return BaseApiResponse.Error(SystemMessageCode.FixedData);
                }

                feedbackEntity.SellingReview = model.SellingReview;
                feedbackEntity.Rate = model.Rate;

                _feedbackRepository.Update(feedbackEntity);
            }

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion


        #region Create Feedback To Admin

        /// <summary>
        /// Creates the feedback to admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateFeedbackToAdmin(FeedbackAdminCreateModel model)
        {
            var transaction = _sellCollectTransactionRepository.GetAsNoTracking(x => x.Id.Equals(model.SellCollectTransactionId));

            if (transaction == null)
            {
                return BaseApiResponse.NotFound();
            }

            var feedbackEntity = await _feedbackRepository.GetAsyncAsNoTracking(x => x.SellCollectTransactionId.Equals(model.SellCollectTransactionId));

            if (feedbackEntity == null)
            {
                // Check Time that Seller Can Give feedback
                var betweenDays = DateTimeUtils.IsMoreThanPastDays(transaction.CreatedTime, NumberConstant.Five);

                if (betweenDays)
                {
                    return BaseApiResponse.Error(SystemMessageCode.TimeUp);
                }

                var entity = new Feedback()
                {
                    SellCollectTransactionId = model.SellCollectTransactionId,
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
