using System;


namespace SCSS.Application.ScrapSeller.Models.FeedbackModels
{
    public class SellCollecTransFeedbackCreateModel
    {
        public Guid SellCollectTransactionId { get; set; }

        public float? Rate { get; set; }

        public string SellingReview { get; set; }
    }
}
