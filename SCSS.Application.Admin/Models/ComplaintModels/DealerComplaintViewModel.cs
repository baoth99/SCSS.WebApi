using System;

namespace SCSS.Application.Admin.Models.ComplaintModels
{
    public class DealerComplaintViewModel
    {
        public Guid Id { get; set; }

        public string TransactionCode { get; set; }

        public string ComplaintContent { get; set; }

        public string SellingAccountInfo { get; set; }

        public string BuyingAccountName { get; set; }

        public string DealerInfo { get; set; }

        public string RepliedContent { get; set; }

        public bool WasReplied { get; set; }

        public string ComplaintTime { get; set; }
    }
}
