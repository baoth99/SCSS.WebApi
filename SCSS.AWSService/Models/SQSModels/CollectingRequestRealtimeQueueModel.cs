using System;

namespace SCSS.AWSService.Models.SQSModels
{
    public class CollectingRequestRealtimeQueueModel
    {
        public Guid CollectingRequestId { get; set; }

        public int? RequestType { get; set; }

        public int? Status { get; set; }

    }
}
