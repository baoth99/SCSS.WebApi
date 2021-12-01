using Dapper;
using Microsoft.EntityFrameworkCore;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.DashboadModels;
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class DashboardService : BaseService, IDashboardService
    {
        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="DashboardService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="dapperService">The dapper service.</param>
        public DashboardService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IDapperService dapperService) : base(unitOfWork, userAuthSession, logger)
        {
            _dapperService = dapperService;

        }

        #endregion

        #region Get Statistic In Day

        /// <summary>
        /// Gets the statistic in day.
        /// </summary>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetStatisticInDay()
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.StatisticSQLCommand, "AdminDashboardStatistic.sql");

            var parameters = new DynamicParameters();
            parameters.Add("@FromDate", DateTimeVN.DATE_FROM);
            parameters.Add("@ToDate", DateTimeVN.DATE_TO);
            parameters.Add("@CompletedCollectingRequest", CollectingRequestStatus.COMPLETED);
            parameters.Add("@CancelCollectingRequestByUser", CollectionConstants.CancelCollectingRequestByUser);
            parameters.Add("@CancelCollectingRequestBySystem", CollectingRequestStatus.CANCEL_BY_SYSTEM);

            var sqlResult = await _dapperService.SqlQueryAsync<StatisticDashboadViewModel>(sql, parameters);

            var sqlModel = sqlResult.FirstOrDefault();

            return BaseApiResponse.OK(sqlModel);
        }

        #endregion

    }
}
