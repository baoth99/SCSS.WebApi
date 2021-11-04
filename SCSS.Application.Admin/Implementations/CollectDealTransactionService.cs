using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.TransactionModels;
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

namespace SCSS.Application.Admin.Implementations
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
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        /// <summary>
        /// The feedback repository
        /// </summary>
        private readonly IRepository<Feedback> _feedbackRepository;

        /// <summary>
        /// The promotion repository
        /// </summary>
        private readonly IRepository<Promotion> _promotionRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectDealTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public CollectDealTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _collectDealTransactionDetailRepository = unitOfWork.CollectDealTransactionDetailRepository;
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _feedbackRepository = unitOfWork.FeedbackRepository;
            _promotionRepository = unitOfWork.PromotionRepository;
        }

        #endregion

        #region Search 

        /// <summary>
        /// Searches the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> Search(CollectDealTransactionSearchModel model)
        {
            var transactionData = await _collectDealTransactionRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.TransactionCode) || x.TransactionCode.Contains(model.TransactionCode)))
                                                                         .Select(x => new
                                                                         {
                                                                             TransId = x.Id,
                                                                             x.TransactionCode,
                                                                             TransDate = x.CreatedTime.Value.Date,
                                                                             TransTime = x.CreatedTime.Value.TimeOfDay,
                                                                             TransDateTime = x.CreatedTime,
                                                                             x.CollectorAccountId,
                                                                             x.DealerAccountId,
                                                                             x.Total
                                                                         }).ToListAsync();


            var dataQuery = transactionData.Where(x => (ValidatorUtil.IsBlank(model.FromDate.ToDateTime()) || x.TransDate.IsCompareDateTimeGreaterOrEqual(model.FromDate.ToDateTime())) &&
                                                       (ValidatorUtil.IsBlank(model.ToDate.ToDateTime()) || x.TransDate.IsCompareDateTimeLessThanOrEqual(model.ToDate.ToDateTime())) &&
                                                       (ValidatorUtil.IsBlank(model.FromTime.ToTimeSpan()) || x.TransTime.IsCompareTimeSpanGreaterOrEqual(model.FromTime.ToTimeSpan())) &&
                                                       (ValidatorUtil.IsBlank(model.ToTime.ToTimeSpan()) || x.TransTime.IsCompareTimeSpanLessOrEqual(model.FromTime.ToTimeSpan())))
                                            .Join(_accountRepository.GetAllAsNoTracking(), x => x.CollectorAccountId, y => y.Id,
                                                (x, y) => new
                                                {
                                                    x.TransId,
                                                    x.TransactionCode,
                                                    x.TransDateTime,
                                                    CollectorName = y.Name,
                                                    CollectorPhone = y.Phone,
                                                    x.DealerAccountId,
                                                    x.Total
                                                })
                                            .Join(_accountRepository.GetAllAsNoTracking(), x => x.DealerAccountId, y => y.Id,
                                                (x, y) => new
                                                {
                                                    x.TransId,
                                                    x.TransactionCode,
                                                    x.TransDateTime,
                                                    x.CollectorName,
                                                    x.CollectorPhone,
                                                    DealerName = y.Name,
                                                    DealerPhone = y.Phone,
                                                    x.Total
                                                })
                                            .OrderByDescending(x => x.TransDateTime);

            var totalRecord = dataQuery.Count();

            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;


            var dataResult = dataQuery.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new CollectDealTransactionViewModel()
            {
                Id = x.TransId,
                TransactionCode = x.TransactionCode,
                TransactionTime = x.TransDateTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                CollectorName = x.CollectorName,
                CollectorPhone = x.CollectorPhone,
                DealerName = x.DealerName,
                DealerPhone = x.DealerPhone,
                TotalPrice = x.Total
            }).ToList();

            return BaseApiResponse.OK(dataResult, totalRecord);
        }

        #endregion


        #region Get Detail

        /// <summary>
        /// Gets the detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDetail(Guid id)
        {
            if (!_collectDealTransactionRepository.IsExisted(x => x.Id.Equals(id)))
            {
                return BaseApiResponse.NotFound();
            }

            var transInfo = await _collectDealTransactionRepository.GetManyAsNoTracking(x => x.Id.Equals(id))
                                                             .Join(_accountRepository.GetAllAsNoTracking(), x => x.CollectorAccountId, y => y.Id,
                                                                    (x, y) => new
                                                                    {
                                                                        TransId = x.Id,
                                                                        x.TransactionCode,
                                                                        x.CreatedTime,
                                                                        x.AwardPoint,
                                                                        x.Total,
                                                                        CollectorName = y.Name,
                                                                        CollectorPhone = y.Phone,
                                                                        x.DealerAccountId
                                                                    })
                                                             .Join(_accountRepository.GetAllAsNoTracking(), x => x.DealerAccountId, y => y.Id,
                                                                    (x, y) => new
                                                                    {
                                                                        x.TransId,
                                                                        x.TransactionCode,
                                                                        x.CreatedTime,
                                                                        x.AwardPoint,
                                                                        x.CollectorName,
                                                                        x.CollectorPhone,
                                                                        x.Total,
                                                                        DealerOwnerName = y.Name,
                                                                        AccId = y.Id,
                                                                    })
                                                             .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.AccId, y => y.DealerAccountId,
                                                                    (x, y) => new
                                                                    {
                                                                        x.TransId,
                                                                        x.TransactionCode,
                                                                        x.CreatedTime,
                                                                        x.AwardPoint,
                                                                        x.CollectorName,
                                                                        x.CollectorPhone,
                                                                        x.DealerOwnerName,
                                                                        x.Total,
                                                                        y.DealerName,
                                                                        y.DealerPhone,
                                                                        y.LocationId
                                                                    })
                                                             .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                                    (x, y) => new
                                                                    {
                                                                        x.TransId,
                                                                        x.TransactionCode,
                                                                        x.CreatedTime,
                                                                        x.AwardPoint,
                                                                        x.CollectorName,
                                                                        x.CollectorPhone,
                                                                        x.DealerOwnerName,
                                                                        x.DealerName,
                                                                        x.DealerPhone,
                                                                        x.Total,
                                                                        y.Address
                                                                    })
                                                             .FirstOrDefaultAsync();

            var feedback = _feedbackRepository.GetAsNoTracking(x => x.CollectDealTransactionId.Equals(transInfo.TransId));

            var transactionDetails = _collectDealTransactionDetailRepository.GetManyAsNoTracking(x => x.CollectDealTransactionId.Equals(transInfo.TransId))
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
                                                                                    })
                                                                                .GroupJoin(_promotionRepository.GetAllAsNoTracking(), x => x.PromotionId, y => y.Id,
                                                                                    (x, y) => new
                                                                                    {
                                                                                        x.Quantity,
                                                                                        x.Total,
                                                                                        x.BonusAmount,
                                                                                        x.Unit,
                                                                                        x.PromotionId,
                                                                                        x.ScrapCategoryName,
                                                                                        Promotion = y
                                                                                    }).SelectMany(x => x.Promotion.DefaultIfEmpty(), (x, y) => new
                                                                                    {
                                                                                        x.Quantity,
                                                                                        x.Total,
                                                                                        x.BonusAmount,
                                                                                        x.Unit,
                                                                                        x.PromotionId,
                                                                                        x.ScrapCategoryName,
                                                                                        PromotionName = y.Name,
                                                                                    });

            var promotions = string.Join(MarkConstant.COMMA, transactionDetails.Select(x => x.PromotionName).ToList());

            var bonusAmountTotal = transactionDetails.Select(x => x.BonusAmount).Sum();

            var transDetailResponse = transactionDetails.Select(x => new CollectDealTransactionDetailViewModel()
            {
                ScrapCategoryName = StringUtils.GetString(x.ScrapCategoryName),
                Quantity = x.Quantity == NumberConstant.Zero ? CommonConstants.Null : $"{x.Quantity} {x.Unit}",
                Total = x.Total,
                Unit = x.Unit,
                BonusAmount = x.BonusAmount == NumberConstant.Zero ? CommonConstants.Null : $"{x.BonusAmount} VND"
            }).ToList();

            var dataResult = new CollectDealTransactionViewDetailModel()
            {
                TransactionCode = transInfo?.TransactionCode,
                TransactionDateTime = transInfo?.CreatedTime.Value.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                AwardPoint = transInfo?.AwardPoint,
                CollectorName = transInfo?.CollectorName,
                CollectorPhone = transInfo?.CollectorPhone,
                DealerAddress = transInfo?.Address,
                DealerOwnerName = transInfo?.DealerOwnerName,
                DealerPhone = transInfo?.DealerPhone,
                CollectorFeedback = feedback?.SellingReview,
                TransactionDetails = transDetailResponse,
                Total = transInfo?.Total,
                BonusAmountTotal = bonusAmountTotal,
                Promotions = promotions == MarkConstant.COMMA ? string.Empty : promotions
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion

    }
}
