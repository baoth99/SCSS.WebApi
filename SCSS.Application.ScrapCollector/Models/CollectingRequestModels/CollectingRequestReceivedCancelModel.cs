using System;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestReceivedCancelModel
    {
        public Guid Id { get; set; }

        public string CancelReason { get; set; }
    }
}
