
namespace SCSS.Application.Admin.Models.FeedbackModels
{
    public class SellCollectTransactionFeedbackSearchModel : BaseFilterModel
    {
        public string TransactionCode { get; set; }

        public string SellerName { get; set; }

        public string CollectorName { get; set; }

        public int Rate { get; set; }
    }

    public class CollectDealTransactionFeedbackSearchModel : BaseFilterModel
    {
        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string CollectorName { get; set; }

        public int Rate { get; set; }
    }
}
