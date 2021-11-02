using System.Collections.Generic;


namespace SCSS.Application.Admin.Models.TransactionModels
{
    public class CollectDealTransactionViewDetailModel
    {
        public string TransactionCode { get; set; }

        public string TransactionDateTime { get; set; }

        public string DealerAddress { get; set; }
        
        public string DealerPhone { get; set; }

        public string DealerOwnerName { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public float? AwardPoint { get; set; }

        public string CollectorFeedback { get; set; }

        public List<CollectDealTransactionDetailViewModel> TransactionDetails { get; set; }

    }

    public class CollectDealTransactionDetailViewModel
    {
        public string ScrapCategoryName { get; set; }

        public float? Quantity { get; set; }

        public long? BonusAmount { get; set; }

        public string Unit { get; set; }

        public long? Total { get; set; }
    }
}
