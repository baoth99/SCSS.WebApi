using SCSS.Application.ScrapSeller.Models.CollectingRequestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.SignalR.CollectorHubs.IHubs
{
    public interface ICollecingRequestHub
    {
        Task ReceiveCollectingRequest(CollectingRequestNoticeModel model);
    }
}
