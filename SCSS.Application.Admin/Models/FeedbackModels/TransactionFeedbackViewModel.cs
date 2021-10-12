
namespace SCSS.Application.Admin.Models.FeedbackModels
{
    public class SellCollectTransactionFeedbackViewModel
    {
        public string TransactionCode { get; set; }

        public string SellerInfo { get; set; }

        public string CollectorInfo { get; set; }

        public string FeedbackContent { get; set; }

        public float? Rate { get; set; }
    }

    public class CollectDealTransactionFeedbackViewModel
    {
        public string TransactionCode { get; set; }

        public string DealerInfo { get; set; }

        public string DealerAccountName { get; set; }

        public string CollectorInfo { get; set; }

        public string FeedbackContent { get; set; }

        public float? Rate { get; set; }
    }
}
