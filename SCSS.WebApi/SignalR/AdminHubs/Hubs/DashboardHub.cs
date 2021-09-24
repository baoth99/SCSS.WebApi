using SCSS.Application.Admin.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.WebApi.SignalR.AdminHubs.IHubs;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.AdminHubs.Hubs
{

    public class AmountOfCollectingRequestHub : BaseAdminHub<IAmountOfCollectingRequestHub>
    {
        #region Services

        /// <summary>
        /// The dashboard service
        /// </summary>
        private readonly IDashboardService _dashboardService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="AmountOfCollectingRequestHub"/> class.
        /// </summary>
        /// <param name="dashboardService">The dashboard service.</param>
        public AmountOfCollectingRequestHub(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        #region Get Amount Of Collecting Request In Day

        /// <summary>
        /// Gets the amount of collecting request in day.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public async Task GetAmountOfCollectingRequestInDay(int amount)
        {
            await Clients.All.GetAmountOfCollectingRequestInDay(amount);
        }

        #endregion Get Amount Of Collecting Request In Day

        #region On Connected Async

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        public async override Task OnConnectedAsync()
        {
            var amount = await _dashboardService.GetAmountOfCollectingRequestInDay(DateTimeVN.DATE_FROM, DateTimeVN.DATE_TO);
            await GetAmountOfCollectingRequestInDay(amount);
        }

        #endregion On Connected Async

    }

    public class AmountOfTransactionHub : BaseAdminHub<IAmountOfTransactionHub>
    {
        #region Services

        private readonly IDashboardService _dashboardService;

        #endregion

        #region Constructor

        public AmountOfTransactionHub(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        #region Get Amount Of Transaction In Day

        /// <summary>
        /// Gets the amount of transaction in day.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public async Task GetAmountOfTransactionInDay(int amount)
        {
            await Clients.All.GetAmountOfTransactionInDay(amount);
        }

        #endregion

        #region On Connected Async

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        public async override Task OnConnectedAsync()
        {
            var amount = await _dashboardService.GetAmountOfTransactionInDay(DateTimeVN.DATE_FROM, DateTimeVN.DATE_TO);
            await GetAmountOfTransactionInDay(amount);
        }

        #endregion On Connected Async

    }
}
