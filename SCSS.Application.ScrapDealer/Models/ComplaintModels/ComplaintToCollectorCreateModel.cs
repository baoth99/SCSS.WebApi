using System;

namespace SCSS.Application.ScrapDealer.Models.ComplaintModels
{
    public class ComplaintToCollectorCreateModel
    {
        public Guid CollectDealTransactionId { get; set; }

        public string ComplaintContent { get; set; }
    }
}
