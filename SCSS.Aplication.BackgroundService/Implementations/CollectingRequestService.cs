using Dapper;
using SCSS.Aplication.BackgroundService.Interfaces;
using SCSS.Data.EF.UnitOfWork;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
using System;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Implementations
{
    public class CollectingRequestService : BaseService, ICollectingRequestService
    {
        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectingRequestService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="dapperService">The dapper service.</param>
        public CollectingRequestService(IUnitOfWork unitOfWork, IDapperService dapperService) : base(unitOfWork)
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

            await _dapperService.SqlExecuteAsync(sql, parameters);
        }

        #endregion

    }
}
