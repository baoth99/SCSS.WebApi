using System;

namespace SCSS.Application.Admin.Models.FeedbackModels
{
    public class SellCollectTransactionFeedbackSqlModel
    {
        public string TransactionCode { get; set; }

        public string SellerName { get; set; }

        public string SellerPhone { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public string FeedbackContent { get; set; }

        public float? Rate { get; set; }

        public int TotalRecord { get; set; }
    }

    public class CollectDealTransactionFeedbackSqlModel
    {
        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string DealerAccountName { get; set; }

        public string DealerAccountPhone { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public string FeedbackContent { get; set; }

        public float? Rate { get; set; }

        public int TotalRecord { get; set; }
    }

    public class SellerFeedbackToSystemSqlModel
    {
        public Guid FeedbackId { get; set; }

        public string CollectingRequestCode { get; set; }

        public string FeedbackContent { get; set; }

        public string SellingAccountName { get; set; }

        public string SellingAccountPhone { get; set; }

        public string BuyingAccountName { get; set; }

        public string BuyingAccountPhone { get; set; }

        public string RepliedContent { get; set; }

        public int TotalRecord { get; set; }

        public DateTime? CreatedTime { get; set; }

    }

    public class CollectorFeedbackToSystemSqlModel
    {
        public Guid FeedbackId { get; set; }

        public string TransactionCode { get; set; }

        public string FeedbackContent { get; set; }

        public string SellingAccountName { get; set; }

        public string SellingAccountPhone { get; set; }

        public string BuyingAccountName { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string RepliedContent { get; set; }

        public int TotalRecord { get; set; }

        public DateTime? CreatedTime { get; set; }

    }
}
