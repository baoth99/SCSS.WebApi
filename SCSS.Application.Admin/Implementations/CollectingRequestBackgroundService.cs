using Dapper;
using SCSS.Application.Admin.Interfaces;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.FirebaseService.Interfaces;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public class CollectingRequestBackgroundService : BaseService, ICollectingRequestBackgroundService
    {
        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestBackgroundService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="fcmService">The FCM service.</param>
        /// <param name="dapperService">The dapper service.</param>
        public CollectingRequestBackgroundService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, 
                                                    IFCMService fcmService, IDapperService dapperService) : base(unitOfWork, userAuthSession, logger, fcmService)
        {
            _dapperService = dapperService;
        }

        #endregion

        #region Trail Collecting Request In Day (Background Task)

        /// <summary>
        /// Trails the collecting request in day.
        /// </summary>
        public async Task TrailCollectingRequestInDayBackground()
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.CollectingRequestSQLCommand, "TrailCollectingRequestInDay.sql");

            var parameters = new DynamicParameters();
            parameters.Add("@CancelBySystemStatus", CollectingRequestStatus.CANCEL_BY_SYSTEM);
            parameters.Add("@UpdatedBy", Guid.Empty);
            parameters.Add("@DateNow", DateTimeVN.DATE_NOW);
            parameters.Add("@PendingStatus", CollectingRequestStatus.PENDING);
            parameters.Add("@ApprovedStatus", CollectingRequestStatus.APPROVED);

            //await _dapperService.SqlExecuteAsync(sql, parameters);
        }

        #endregion
    }
}
