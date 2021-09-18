using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.CollectorHubs.IHubs
{
    public interface ICollecingRequestHub
    {
        /// <summary>
        /// Receives the collecting request.
        /// </summary>
        /// <param name="collectingRequestId">The collecting request identifier.</param>
        /// <returns></returns>
        Task ReceiveCollectingRequest(Guid collectingRequestId);
    }
}
