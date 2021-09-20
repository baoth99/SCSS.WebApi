using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.DealerPromotionModels;
using SCSS.AWSService.Interfaces;
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
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class DealerPromotionService : BaseService, IDealerPromotionService
    {
        #region Repositories

        /// <summary>
        /// The promotion repository
        /// </summary>
        private readonly IRepository<Promotion> _promotionRepository;

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DealerPromotionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public DealerPromotionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _promotionRepository = unitOfWork.PromotionRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
        }

        #endregion  Constructor

        #region Get Dealer Promotions

        /// <summary>
        /// Gets the dealer promotion.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerPromotions(Guid dealerId)
        {
            var dealerAccountId = _dealerInformationRepository.GetById(dealerId).DealerAccountId;

            // Get Promotions going on
            var promotionsGoingOn = _promotionRepository.GetMany(x => x.DealerAccountId.Equals(dealerAccountId) &&
                                                                           x.FromTime.Value.Date.CompareTo(DateTimeInDay.DATE_NOW) <= NumberConstant.Zero &&
                                                                           x.ToTime.Value.Date.CompareTo(DateTimeInDay.DATE_NOW) >= NumberConstant.Zero &&
                                                                           x.Status == PromotionStatus.ACTIVE);

            if (!promotionsGoingOn.Any())
            {
                return BaseApiResponse.OK(totalRecord: NumberConstant.Zero, resData: CollectionConstants.Empty<DealerPromotionViewModel>());
            }

            var promotionScrapCateory = promotionsGoingOn.Join(_scrapCategoryRepository.GetManyAsNoTracking(x => x.AccountId.Equals(dealerAccountId)), x => x.DealerCategoryId, y => y.Id,
                                                              (x, y) => new
                                                              {
                                                                  PromotionId = x.Id,
                                                                  PromotionCode = x.Code,
                                                                  PromotionName = x.Name,
                                                                  AppliedScrapCategory = y.Name,
                                                                  AppliedScrapCategoryImageUrl = y.ImageUrl,
                                                                  x.AppliedAmount,
                                                                  x.BonusAmount,
                                                                  AppliedFromTime =  x.FromTime,
                                                                  AppliedToTime = x.ToTime
                                                              }).OrderBy(x => x.BonusAmount);

            var totalRecord = await promotionScrapCateory.CountAsync();

            var dataResult = promotionScrapCateory.Select(x => new DealerPromotionViewModel()
            {
                Id = x.PromotionId,
                PromotionName = x.PromotionName,
                Code = x.PromotionCode,
                AppliedScrapCategoryImageUrl = x.AppliedScrapCategoryImageUrl,
                AppliedAmount = x.AppliedAmount,
                BonusAmount = x.BonusAmount,
                AppliedFromTime = x.AppliedFromTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                AppliedToTime = x.AppliedToTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                AppliedScrapCategory = x.AppliedScrapCategory
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Dealer Promotion Detail

        /// <summary>
        /// Gets the dealer promotion detail.
        /// </summary>
        /// <param name="promotionId">The promotion identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetDealerPromotionDetail(Guid promotionId)
        {
            var dealerPromotion = await _promotionRepository.GetByIdAsync(promotionId);

            if (dealerPromotion == null)
            {
                return BaseApiResponse.NotFound();
            }

            var scrapCategoryName = _scrapCategoryRepository.GetById(dealerPromotion.DealerCategoryId).Name;

            var dataResult = new DealerPromotionDetailViewModel()
            {
                Code = dealerPromotion.Code,
                AppliedAmount = dealerPromotion.AppliedAmount,
                AppliedScrapCategory = scrapCategoryName,
                AppliedFromTime = dealerPromotion.FromTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                AppliedToTime = dealerPromotion.ToTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                BonusAmount = dealerPromotion.BonusAmount,
                PromotionName = dealerPromotion.Name
            };
            return BaseApiResponse.OK(dataResult);
        }

        #endregion

    }
}
