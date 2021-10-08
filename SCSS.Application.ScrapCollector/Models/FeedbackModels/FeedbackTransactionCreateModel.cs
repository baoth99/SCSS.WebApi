using System;

namespace SCSS.Application.ScrapCollector.Models.FeedbackModels
{
    public class FeedbackTransactionCreateModel
    {
        public Guid CollectDealTransId { get; set; }

        public float? Rate { get; set; }

        public string Review { get; set; }
    }
}
