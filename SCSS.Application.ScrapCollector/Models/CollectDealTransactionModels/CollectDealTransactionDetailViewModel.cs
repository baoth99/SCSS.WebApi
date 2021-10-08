using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.CollectDealTransactionModels
{
    public class CollectDealTransactionDetailViewModel
    {
        public Guid TransId { get; set; }

        public string TransactionCode { get; set; }

        public DateTime? TrasactionDateTime { get; set; }

        public TransDealerInformationViewModel DealerInfo { get; set; }

        public CollectDealTransFeedbackViewModel Feedback { get; set; }

        public List<TransHistoryScrapCategoryViewModel> ItemDetails { get; set; }

        public long? Total { get; set; }

        public long? TotalBonus { get; set; }

        public float? AwardPoint { get; set; }

    }

    public class TransDealerInformationViewModel
    {
        public string DealerName { get; set; }

        public string DealerPhone { get; set; }
    }

    public class CollectDealTransFeedbackViewModel
    {
        public int FeedbackStatus { get; set; }

        public float? RatingFeedback { get; set; }
    }

    public class TransHistoryScrapCategoryViewModel
    {
        public string ScrapCategoryName { get; set; }

        public float? Quantity { get; set; }

        public long? Total { get; set; }

        public bool IsBonus { get; set; }

        public long? BonusAmount { get; set; }
    }
}
