using System.Collections.Generic;

namespace SCSS.Application.Admin.Models.TransactionModels
{
    public class SellCollectTransactionViewDetailModel
    {
        public string TransactionCode { get; set; }

        public string TransactionDate { get; set; }

        public string Address { get; set; }

        public string SellerName { get; set; }

        public string SellerPhone { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public string CollectingRequestDate { get; set; }

        public int? AwardPoint { get; set; }

        public long? Total { get; set; }

        public List<SellCollectTransactionDetailViewModel> TransDetails { get; set; }
    }

    public class SellCollectTransactionDetailViewModel
    {
        public string ScrapCategoryName { get; set; }

        public float? Quantity { get; set; }

        public string Unit { get; set; }

        public long? Total { get; set; }
    }
}
