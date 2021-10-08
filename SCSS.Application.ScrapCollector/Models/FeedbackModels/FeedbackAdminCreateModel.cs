using System;

namespace SCSS.Application.ScrapCollector.Models.FeedbackModels
{
    public class FeedbackAdminCreateModel
    {
        public Guid CollectDealTransactionId { get; set; }

        public string SellingFeedback { get; set; }
    }
}
