using System;

namespace SCSS.Application.ScrapCollector.Models.ComplaintModels
{
    public class ComplaintToSellerCreateModel
    {
        public Guid CollectingRequestId { get; set; }

        public string ComplaintContent { get; set; }
    }
}
