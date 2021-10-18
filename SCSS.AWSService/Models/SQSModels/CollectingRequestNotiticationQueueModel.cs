using System;

namespace SCSS.AWSService.Models.SQSModels
{
    public class CollectingRequestNotiticationQueueModel
    {
        public Guid CollectingRequestId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int RequestType { get; set; }
    }
}
