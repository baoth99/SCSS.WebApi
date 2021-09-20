using System;

namespace SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels
{
    public class TransactionInfoReviewViewModel
    {
        public Guid CollectingRequestId { get; set; }

        public string CollectingRequestCode { get; set; }

        public string SellerName { get; set; }

        public float? TransactionFeePercent { get; set; }

        public string SellerPhone { get; set; }
    }
}
