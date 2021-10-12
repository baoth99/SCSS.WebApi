using System;

namespace SCSS.Application.Admin.Models.FeedbackModels
{
    public class SellerFeedbackToSystemViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string FeedbackContent { get; set; }

        public string SellingInfo { get; set; }

        public string BuyingInfo { get; set; }

        public string RepliedContent { get; set; }

        public bool WasReplied { get; set; }

    }

    public class CollectorFeedbackToSystemViewModel
    {
        public Guid Id { get; set; }

        public string TransactionCode { get; set; }

        public string FeedbackContent { get; set; }

        public string SellingAccountInfo { get; set; }

        public string BuyingAccountName { get; set; }

        public string DealerInfo { get; set; }

        public string RepliedContent { get; set; }

        public bool WasReplied { get; set; }
    }
}
