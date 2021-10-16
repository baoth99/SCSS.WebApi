using System;

namespace SCSS.Application.ScrapCollector.Models.ComplaintModels
{
    public class ComplaintToDealerCreateModel
    {
        public Guid CollectDealTransactionId { get; set; }

        public string ComplaintContent { get; set; }
    }
}
