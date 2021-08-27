using SCSS.Application.Admin.Interfaces;
using SCSS.Utilities.Constants;
using SCSS.WebApi.SignalR.AdminHubs.IHubs;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.AdminHubs.Hubs
{

    public class AmountOfBookingHub : BaseAdminHub<IAmountOfBookingHub>
    {
        #region Services

        private readonly IDashboardService _dashboardService;

        #endregion

        #region Constructor

        public AmountOfBookingHub(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        #endregion

        #region Get Amount Of Booking In Day

        /// <summary>
        /// Gets the amount of booking in day.
        /// </summary>
        /// <param name="amount">The amount.</param>
        public async Task GetAmountOfBookingInDay(int amount)
        {
            await Clients.All.GetAmountOfBookingInDay(amount);
        }

        #endregion

        #region On Connected Async

        /// <summary>
        /// Called when a new connection is established with the hub.
        /// </summary>
        public async override Task OnConnectedAsync()
        {
            var amount = await _dashboardService.GetAmountOfBookingInDay(DateTimeInDay.DATEFROM, DateTimeInDay.DATETO);
            await GetAmountOfBookingInDay(amount);
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
            var amount = await _dashboardService.GetAmountOfTransactionInDay(DateTimeInDay.DATEFROM, DateTimeInDay.DATETO);
            await GetAmountOfTransactionInDay(amount);
        }

        #endregion On Connected Async

    }
}
