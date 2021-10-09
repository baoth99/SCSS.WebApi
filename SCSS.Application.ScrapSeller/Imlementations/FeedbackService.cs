using Microsoft.EntityFrameworkCore;
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
using System;
using System.Linq;
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
        /// The feedback to system repository
        /// </summary>
        private IRepository<FeedbackToSystem> _feedbackToSystemRepository;

        /// <summary>
        /// The sell collect transaction repository
        /// </summary>
        private IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private IRepository<Account> _accountRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private IRepository<CollectingRequest> _collectingRequestRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public FeedbackService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _feedbackToSystemRepository = unitOfWork.FeedbackToSystemRepository;
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _accountRepository = unitOfWork.AccountRepository;
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
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

            var collectingRequest = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(transaction.CollectingRequestId));

            var isExisted = await _feedbackRepository.IsExistedAsync(x => x.SellCollectTransactionId.Equals(model.SellCollectTransactionId) &&
                                                                          x.CollectDealTransactionId == null);

            if (isExisted)
            {
                return BaseApiResponse.Error(SystemMessageCode.DuplicateData);
            }

            // Check Time that Seller Can Give feedback
            var deadline = await FeedbackDeadline();
            var isMoreThan = DateTimeUtils.IsMoreThanPastDays(transaction.CreatedTime, deadline);

            if (isMoreThan)
            {
                return BaseApiResponse.Error(SystemMessageCode.TimeUp);
            }

            var entity = new Feedback()
            {
                SellCollectTransactionId = model.SellCollectTransactionId,
                SellingAccountId = UserAuthSession.UserSession.Id,
                BuyingAccountId = collectingRequest.CollectorAccountId,
                Rate = model.Rate.ToFloatValue(),
                SellingReview = StringUtils.GetString(model.SellingReview),
            };

            _feedbackRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            await UpdateCollectorRating(collectingRequest.CollectorAccountId);

            return BaseApiResponse.OK();
        }


        /// <summary>
        /// Updates the collector rating.
        /// </summary>
        /// <param name="collectorId">The collector identifier.</param>
        private async Task UpdateCollectorRating(Guid? collectorId)
        {
            var feedbacks = _feedbackRepository.GetManyAsNoTracking(x => x.BuyingAccountId.Equals(collectorId)).Select(x => x.Rate);
            var rating = await feedbacks.AverageAsync();

            var collectorAccount = _accountRepository.GetById(collectorId);
            collectorAccount.Rating = rating;

            _accountRepository.Update(collectorAccount);

            await UnitOfWork.CommitAsync();
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
            var collectingRequest = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(model.CollectingRequestId));

            if (collectingRequest == null)
            {
                return BaseApiResponse.NotFound();
            }

            var isExisted = _feedbackToSystemRepository.IsExisted(x => x.CollectingRequestId.Equals(model.CollectingRequestId));

            if (isExisted)
            {
                return BaseApiResponse.Error(SystemMessageCode.DuplicateData);
            }
            var entity = new FeedbackToSystem()
            {
                CollectingRequestId = model.CollectingRequestId,
                SellingAccountId = UserAuthSession.UserSession.Id,
                BuyingAccountId = collectingRequest.CollectorAccountId,
                SellingFeedback = model.SellingFeedback,
            };

            _feedbackToSystemRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
