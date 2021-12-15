using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using SCSS.Utilities.Extensions;
using SCSS.WebApi.SignalR.CollectorHubs.IHubs;
using System;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.CollectorHubs.Hubs
{
    public class CollectingRequestHub : BaseCollectorHub<ICollecingRequestHub>
    {
        #region Collecting Request Id To another Collector

        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="model">The model.</param>
        public async Task ReceiveCollectingRequest(string noticeJsonModel)
        {
            await Clients.All.ReceiveCollectingRequest(noticeJsonModel);
        }

        #endregion
    }
}
