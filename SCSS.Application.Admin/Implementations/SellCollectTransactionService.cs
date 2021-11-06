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
    public class SellCollectTransactionService : BaseService, ISellCollectTransactionService
    {
        #region Repositories

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The sell collect transaction repository
        /// </summary>
        private readonly IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        /// <summary>
        /// The collect request respository/
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectRequestRespository;

        /// <summary>
        /// The sell collect transaction detail repository
        /// </summary>
        private readonly IRepository<SellCollectTransactionDetail> _sellCollectTransactionDetailRepository;

        /// <summary>
        /// The scrap category repository
        /// </summary>
        private readonly IRepository<ScrapCategory> _scrapCategoryRepository;

        /// <summary>
        /// The scrap category detail repository
        /// </summary>
        private readonly IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        /// <summary>
        /// The location repository
        /// </summary>
        private readonly IRepository<Location> _locationRepository;

        /// <summary>
        /// The feedback repository
        /// </summary>
        private readonly IRepository<Feedback> _feedbackRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="SellCollectTransactionService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public SellCollectTransactionService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger) : base(unitOfWork, userAuthSession, logger)
        {
            _accountRepository = unitOfWork.AccountRepository;
            _sellCollectTransactionRepository = unitOfWork.SellCollectTransactionRepository;
            _collectRequestRespository = unitOfWork.CollectingRequestRepository;
            _locationRepository = unitOfWork.LocationRepository;
            _sellCollectTransactionDetailRepository = unitOfWork.SellCollectTransactionDetailRepository;
            _scrapCategoryRepository = unitOfWork.ScrapCategoryRepository;
            _scrapCategoryDetailRepository = unitOfWork.ScrapCategoryDetailRepository;
            _feedbackRepository = unitOfWork.FeedbackRepository;
        }

        #endregion

        #region Search Sell-Collect Transactions

        /// <summary>
        /// Searches the sell collect transactions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchSellCollectTransactions(SellCollectTransactionSearchModel model)
        {
            var transactionData = await  _collectRequestRespository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.TransactionCode) || x.CollectingRequestCode.Contains(model.TransactionCode)))
                                                      .Join(_sellCollectTransactionRepository.GetAllAsNoTracking(), x => x.Id, y => y.CollectingRequestId,
                                                           (x, y) => new
                                                           {
                                                               TransId = y.Id,
                                                               TransCode = x.CollectingRequestCode,
                                                               x.CollectorAccountId,
                                                               x.SellerAccountId,
                                                               TransDate = y.CreatedTime,
                                                               CreatedDate = y.CreatedTime.Value.Date,
                                                               CreatedTime = y.CreatedTime.Value.TimeOfDay,
                                                               y.Total,
                                                           }).ToListAsync();
             var dataQuery = transactionData.Where(x => (ValidatorUtil.IsBlank(model.FromDate.ToDateTime()) || x.CreatedDate.IsCompareDateTimeGreaterOrEqual(model.FromDate.ToDateTime())) &&
                                                        (ValidatorUtil.IsBlank(model.ToDate.ToDateTime()) || x.CreatedDate.IsCompareDateTimeLessThanOrEqual(model.ToDate.ToDateTime())) &&
                                                        (ValidatorUtil.IsBlank(model.FromTime.ToTimeSpan()) || x.CreatedTime.IsCompareTimeSpanGreaterOrEqual(model.ToTime.ToTimeSpan())) &&
                                                        (ValidatorUtil.IsBlank(model.ToTime.ToTimeSpan()) || x.CreatedTime.IsCompareTimeSpanLessOrEqual(model.ToTime.ToTimeSpan())))
                                            .Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.SellerName) || x.Name.Contains(model.SellerName)) &&
                                                                                            (ValidatorUtil.IsBlank(model.SellerPhone) || x.Phone.Contains(model.SellerPhone))),
                                                        x => x.SellerAccountId, y => y.Id, (x, y) => new // Get Seller Information
                                                        {
                                                            x.TransId,
                                                            x.TransCode,
                                                            x.TransDate,
                                                            x.Total,
                                                            SellerName = y.Name,
                                                            SellerPhone = y.Phone,
                                                            x.CollectorAccountId
                                                        })
                                            .Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.CollectorName) || x.Name.Contains(model.CollectorName)) &&
                                                                                            (ValidatorUtil.IsBlank(model.CollectorPhone) || x.Phone.Contains(model.CollectorPhone))),
                                                        x => x.CollectorAccountId, y => y.Id, (x, y) => new // Get Colector Information
                                                        {
                                                            x.TransId,
                                                            x.TransCode,
                                                            x.TransDate,
                                                            x.Total,
                                                            x.SellerName,
                                                            x.SellerPhone,
                                                            CollectorName = y.Name,
                                                            CollectorPhone = y.Phone
                                                        })
                                            .OrderByDescending(x => x.TransDate);

            var totalRecord = dataQuery.Count();

            var page = model.Page <= NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize <= NumberConstant.Zero ? NumberConstant.Ten : model.PageSize;

            var dataResult = dataQuery.Skip((page - 1) * pageSize).Take(pageSize).Select(x => new SellCollectTransactionViewModel()
            {
                Id = x.TransId,
                TransactionCode = x.TransCode,
                CollectorName = x.CollectorName,
                CollectorPhone = x.CollectorPhone,
                SellerName = x.SellerName,
                SellerPhone = x.SellerPhone,
                TotalPrice = x.Total,
                TransactionTime = x.TransDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt)
            }).ToList();

            return BaseApiResponse.OK(dataResult, totalRecord);
        }

        #endregion


        #region Get Sell-Collect Transaction Detail

        /// <summary>
        /// Gets the sell collect transaction detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetSellCollectTransactionDetail(Guid id)
        {
            var transaction = await _sellCollectTransactionRepository.GetByIdAsync(id);

            if (transaction == null)
            {
                return BaseApiResponse.NotFound();
            }

            var collectingRequest = _collectRequestRespository.GetManyAsNoTracking(x => x.Id.Equals(transaction.CollectingRequestId))
                                                              .Join(_accountRepository.GetAllAsNoTracking(), x => x.SellerAccountId, y => y.Id,
                                                                    (x, y) => new
                                                                    {
                                                                        x.CollectingRequestCode,
                                                                        x.CollectingRequestDate,
                                                                        x.LocationId,
                                                                        x.CollectorAccountId,
                                                                        SellerName = y.Name,
                                                                        SellerPhone = y.Phone
                                                                    })
                                                              .Join(_accountRepository.GetAllAsNoTracking(), x => x.CollectorAccountId, y => y.Id,
                                                                    (x, y) => new
                                                                    {
                                                                        x.CollectingRequestCode,
                                                                        x.CollectingRequestDate,
                                                                        x.LocationId,
                                                                        x.SellerName,
                                                                        x.SellerPhone,
                                                                        CollectorName = y.Name,
                                                                        CollectorPhone = y.Phone,
                                                                    })
                                                              .Join(_locationRepository.GetAllAsNoTracking(), x => x.LocationId, y => y.Id,
                                                                    (x, y) => new
                                                                    {
                                                                        x.CollectingRequestCode,
                                                                        x.CollectingRequestDate,
                                                                        y.Address,
                                                                        x.SellerName,
                                                                        x.SellerPhone,
                                                                        x.CollectorName,
                                                                        x.CollectorPhone
                                                                    })
                                                              .FirstOrDefault();



            var transactionDetailItems = _sellCollectTransactionDetailRepository.GetManyAsNoTracking(x => x.SellCollectTransactionId.Equals(transaction.Id))
                                                                               .GroupJoin(_scrapCategoryDetailRepository.GetAllAsNoTracking(), x => x.CollectorCategoryDetailId, y => y.Id,
                                                                                         (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.ScrapCategoryName,
                                                                                             x.Total,
                                                                                             ScrapCategoryDetail = y
                                                                                         }).SelectMany(x => x.ScrapCategoryDetail.DefaultIfEmpty(), (x, y) => new
                                                                                         {
                                                                                             x.Quantity,
                                                                                             x.Total,
                                                                                             y.ScrapCategoryId,
                                                                                             y.Unit,
                                                                                             x.ScrapCategoryName
                                                                                         })
                                                                                         .Select(x => new SellCollectTransactionDetailViewModel()
                                                                                         {
                                                                                             Quantity = x.Quantity == NumberConstant.Zero ? CommonConstants.Null : $"{x.Quantity} {x.Unit}",
                                                                                             ScrapCategoryName = StringUtils.GetString(x.ScrapCategoryName),
                                                                                             Unit = x.Unit,
                                                                                             Total = x.Total,
                                                                                         }).ToList();
            var feedback = _feedbackRepository.GetAsNoTracking(x => x.CollectDealTransactionId.Equals(transaction.Id) && x.SellCollectTransactionId == null);


            var dataResult = new SellCollectTransactionViewDetailModel()
            {
                TransactionCode = collectingRequest?.CollectingRequestCode,
                TransactionDate = transaction.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                Address = collectingRequest?.Address,
                CollectingRequestDate = collectingRequest?.CollectingRequestDate.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                AwardPoint = transaction.AwardPoint,
                Total = transaction.Total,
                SellerName = collectingRequest?.SellerName,
                SellerPhone = collectingRequest?.SellerPhone,
                CollectorPhone = collectingRequest?.CollectorPhone,
                CollectorName = collectingRequest?.CollectorName,
                TransDetails = transactionDetailItems,
                Feedback = feedback?.SellingReview,
                Rating = feedback?.Rate
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion
    }
}
