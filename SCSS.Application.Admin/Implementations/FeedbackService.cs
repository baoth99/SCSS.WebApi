using Dapper;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.FeedbackModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public partial class FeedbackService : BaseService, IFeedbackService
    {
        #region Repositories

        /// <summary>
        /// The feedback to system repository
        /// </summary>
        private readonly IRepository<FeedbackToSystem> _feedbackToSystemRepository;

        #endregion

        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public FeedbackService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IDapperService dapperService) : base(unitOfWork, userAuthSession, logger)
        {
            _feedbackToSystemRepository = unitOfWork.FeedbackToSystemRepository;
            _dapperService = dapperService;
        }

        #endregion

        #region Search Sell Collect Transaction Feedbacks

        /// <summary>
        /// Searches the sell collect transaction feedbacks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchSellCollectTransactionFeedbacks(SellCollectTransactionFeedbackSearchModel model)
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.FeedbackSQLCommand, "SellCollectTransFeedback.sql");

            model.Page = model.Page < NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize < NumberConstant.Zero ? NumberConstant.Seven : model.PageSize;
            var page = (model.Page - 1) * pageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@TransactionCode", model.TransactionCode);
            parameters.Add("@SellerName", model.SellerName);
            parameters.Add("@CollectorName", model.CollectorName);
            parameters.Add("@Rate", model.Rate);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            var dataQuery = await _dapperService.SqlQueryAsync<SellCollectTransactionFeedbackSqlModel>(sql, parameters);

            var firstRecord = dataQuery.FirstOrDefault();

            var totalRecord = firstRecord == null ? NumberConstant.Zero : firstRecord.TotalRecord;

            var dataResult = dataQuery.Select(x => new SellCollectTransactionFeedbackViewModel()
            {
                TransactionCode = x.TransactionCode,
                SellerInfo = string.Format("{0}-{1}", x.SellerPhone, x.SellerName),
                CollectorInfo = string.Format("{0}-{1}", x.CollectorPhone, x.CollectorName),
                FeedbackContent = x.FeedbackContent,
                Rate = x.Rate
            }).ToList();
            
            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Search Collect Deal Transaction Feedbacks

        /// <summary>
        /// Searches the collect deal transaction feedbacks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchCollectDealTransactionFeedbacks(CollectDealTransactionFeedbackSearchModel model)
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.FeedbackSQLCommand, "CollectDealTransactionFeedback.sql");

            model.Page = model.Page < NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize < NumberConstant.Zero ? NumberConstant.Seven : model.PageSize;
            var page = (model.Page - 1) * pageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@TransactionCode", model.TransactionCode);
            parameters.Add("@DealerName", model.DealerName);
            parameters.Add("@CollectorName", model.CollectorName);
            parameters.Add("@Rate", model.Rate);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            var dataQuery = await _dapperService.SqlQueryAsync<CollectDealTransactionFeedbackSqlModel>(sql, parameters);

            var firstRecord = dataQuery.FirstOrDefault();
            var totalRecord = firstRecord == null ? NumberConstant.Zero : firstRecord.TotalRecord;

            var dataResult = dataQuery.Select(x => new CollectDealTransactionFeedbackViewModel()
            {
                TransactionCode = x.TransactionCode,
                CollectorInfo = string.Format("{0}-{1}", x.CollectorPhone, x.CollectorName),
                DealerInfo = string.Format("{0}-{1}", x.DealerPhone, x.DealerName),
                DealerAccountName = x.DealerAccountName,
                FeedbackContent = x.FeedbackContent,
                Rate = x.Rate,
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion
    }
}
