using Microsoft.EntityFrameworkCore;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.PromotionModels;
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

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class PromotionService : BaseService, IPromotionService
    {
        #region Repositories

        /// <summary>
        /// The promotion repository
        /// </summary>
        private readonly IRepository<Promotion> _promotionRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="PromotionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public PromotionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _promotionRepository = unitOfWork.PromotionRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
        }

        #endregion

        #region Create New Promotion

        /// <summary>
        /// Creates the new promotion.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateNewPromotion(PromotionCreateModel model)
        {

            var promotionFromTime = model.AppliedFromTime.ToDateTime();
            var promotionToTime = model.AppliedToTime.ToDateTime();

            //  Check AppliedFromTime with AppliedToTime valid
            if (promotionFromTime.IsCompareDateTimeEqual(promotionToTime))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }

            var dealerAccountId = UserAuthSession.UserSession.Id;

            var scrapCategory = await _scrapCategoryRepository.GetAsyncAsNoTracking(x => x.Id.Equals(model.PromotionScrapCategoryId) && x.AccountId.Equals(dealerAccountId));

            // Check Scrap Category is existed
            if (scrapCategory == null)
            {
                return BaseApiResponse.Error(SystemMessageCode.DataInvalid);
            }
            //  Check AppliedFromTime with AppliedToTime
            //  AppliedFromTime is greater than AppliedToTime => Swapping
            if (promotionFromTime.IsCompareDateTimeGreaterThan(promotionToTime))
            {
                var temp = promotionFromTime;
                promotionFromTime = promotionToTime; // 2021-09-12 12:03:00
                promotionToTime = temp;
            }

            // Get GeneratePromotionCodeParamsModel from promotionFromTime, promotionToTime, BonusAmount, scrapCategoryName
            var generateParamModel = new GeneratePromotionCodeParamsModel()
            {
                FromDateTime = promotionFromTime,
                ToDateTime = promotionToTime,
                BonusAmount = model.BonusAmount,
                ScrapCategoryName = scrapCategory.Name
            };

            // Auto Generate Promotion Code by GeneratePromotionCodeParamsModel
            var promotionCode = await GeneratePromotionCode(generateParamModel);

            var entity = new Promotion()
            {
                Code = promotionCode,
                Name = model.PromotionName,
                AppliedAmount = model.AppliedAmount,
                BonusAmount = model.BonusAmount,
                FromTime = promotionFromTime,
                ToTime = promotionToTime,
                DealerCategoryId = model.PromotionScrapCategoryId,
                DealerAccountId = dealerAccountId
            };

            var entityResult = _promotionRepository.Insert(entity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK(entityResult.Code);
        }


        /// <summary>
        /// Generates the promotion code.
        /// </summary>
        /// <param name="paramModel">The parameter model.</param>
        /// <returns></returns>
        private async Task<string> GeneratePromotionCode(GeneratePromotionCodeParamsModel paramModel)
        {
            var accountId = UserAuthSession.UserSession.Id;
            var fromDateTimeVal = paramModel.FromDateTime.Value;
            var toDateTimeVal = paramModel.ToDateTime.Value;

            // 
            var scrapCategoryName = paramModel.ScrapCategoryName.RemoveWhiteSpace().RemoveSignVietnameseString().ToUpper();

            // Get Promotion Amount of Dealer
            var promotionAmount = await _promotionRepository.GetManyAsNoTracking(x => x.DealerAccountId.Equals(accountId)).CountAsync();

            var fromDateTime = fromDateTimeVal.ToDateCode(DateCodeFormat.DDMM);
            var fromToTime = toDateTimeVal.ToDateCode(DateCodeFormat.DDMM);

            var promotionCode = string.Format(GenerationCodeFormat.PROMOTION_CODE, scrapCategoryName, 
                                                                fromDateTime,
                                                                fromToTime,
                                                                promotionAmount);
            return promotionCode;
        }

        #endregion

        #region Get Promotions

        /// <summary>
        /// Gets the promotions.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetPromotions()
        {
            // Get Dealer Account Id from UserAuthSession
            var dealerAccountId = UserAuthSession.UserSession.Id;

            var dataQuery = _promotionRepository.GetManyAsNoTracking(x => x.DealerAccountId.Equals(dealerAccountId))
                                              .Join(_scrapCategoryRepository.GetAllAsNoTracking(), x => x.DealerCategoryId, y => y.Id,
                                                    (x, y) => new
                                                    {
                                                        PromotionId = x.Id,
                                                        x.Code,
                                                        PromotionName = x.Name,
                                                        AppliedScrapCategory = y.Name,
                                                        AppliedScrapCategoryImageUrl = y.ImageUrl,
                                                        x.AppliedAmount,
                                                        x.BonusAmount,
                                                        x.FromTime,
                                                        x.ToTime,
                                                    });

            var totalRecord = await dataQuery.CountAsync();

            var dataResult = dataQuery.Select(x => new PromotionViewModel()
            {
                Id = x.PromotionId,
                Code = x.Code,
                PromotionName = x.PromotionName,
                AppliedScrapCategory = x.AppliedScrapCategory,
                AppliedScrapCategoryImageUrl = x.AppliedScrapCategoryImageUrl,
                AppliedAmount = x.AppliedAmount,
                BonusAmount = x.BonusAmount,
                AppliedFromTime = x.FromTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                AppliedToTime = x.ToTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy)
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Promotion Detail

        /// <summary>
        /// Gets the promotion detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetPromotionDetail(Guid id)
        {
            // Get Dealer Account Id from UserAuthSession
            var dealerAccountId = UserAuthSession.UserSession.Id;

            var promotion = await _promotionRepository.GetAsyncAsNoTracking(x => x.Id.Equals(id) && x.DealerAccountId.Equals(dealerAccountId));

            if (promotion == null)
            {
                return BaseApiResponse.NotFound();
            }

            var promotionScrapCategory = _scrapCategoryRepository.GetById(promotion.DealerCategoryId);

            var dataResult = new PromotionDetailViewModel()
            {
                Code = promotion.Code,
                PromotionName = promotion.Name,
                AppliedAmount = promotion.AppliedAmount,
                BonusAmount = promotion.BonusAmount,
                AppliedScrapCategory = promotionScrapCategory.Name,
                AppliedFromTime = promotion.FromTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy),
                AppliedToTime = promotion.ToTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy)
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
