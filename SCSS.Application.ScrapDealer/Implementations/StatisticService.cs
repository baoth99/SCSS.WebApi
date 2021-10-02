using Dapper;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.StatisticModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.FirebaseService.Interfaces;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class StatisticService : BaseService, IStatisticService
    {
        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        /// <param name="cacheService">The cache service.</param>
        /// <param name="dapperService">The dapper service.</param>
        public StatisticService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, 
                                IFCMService fcmService, IStringCacheService cacheService, IDapperService dapperService) : base(unitOfWork, userAuthSession, logger, fcmService, cacheService)
        {
            _dapperService = dapperService;
        }

        #endregion

        #region Get Statistic In Range Time

        /// <summary>
        /// Gets the statistic in range time.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetStatisticInRangeTime(StatisticDateFilterModel model)
        {
            var dateFrom = model.FromDate.ToDateTime();
            var dateTo = model.ToDate.ToDateTime();

            // Handle DateTime
            if (dateTo.IsCompareDateTimeGreaterThan(DateTimeVN.DATE_NOW))
            {
                dateTo = DateTimeVN.DATE_NOW;
            }

            if (dateFrom.IsCompareDateTimeGreaterThan(dateTo))
            {
                // Swap 
                var temp = dateFrom;
                dateFrom = dateTo;
                dateTo = temp;
            }
            var sql = AppFileHelper.ReadContent(AppSettingValues.StatisticSQLCommand, "DealerDashboardStastistic.sql");

            var parameters = new DynamicParameters();
            parameters.Add("@DealerId", UserAuthSession.UserSession.Id);
            parameters.Add("@FromDate", dateFrom);
            parameters.Add("@ToDate", dateTo);

            var sqlResult = await _dapperService.SqlQueryAsync<StatisticSQLModel>(sql, parameters);

            var sqlModel = sqlResult.FirstOrDefault();

            var dataResult = new StatisticResponseViewModel()
            {
                TotalCollecting = sqlModel.TotalCollecting.ToLongValue(),
                NumOfCompletedTrans = sqlModel.NumOfCompletedTrans.ToIntValue(),
                BonusAmount = sqlModel.BonusAmount.ToLongValue(),
                TotalFee = sqlModel.TotalFee.ToLongValue()
            };

            return BaseApiResponse.OK(dataResult);
        }

        #endregion


    }
}
