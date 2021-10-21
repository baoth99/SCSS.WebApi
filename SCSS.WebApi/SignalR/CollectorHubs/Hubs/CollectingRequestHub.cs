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
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        public async Task ReceiveCollectingRequest(Guid collectingRequestId)
        {
            await Clients.All.ReceiveCollectingRequest(collectingRequestId);
        }

        #endregion
    }
}
