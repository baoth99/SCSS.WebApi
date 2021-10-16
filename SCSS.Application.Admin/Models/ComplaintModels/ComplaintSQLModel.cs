using System;


namespace SCSS.Application.Admin.Models.ComplaintModels
{
    public class SellerComplaintSQLModel
    {
        public Guid SellerComplaintId { get; set; }

        public string CollectingRequestCode { get; set; }

        public string ComplaintContent { get; set; }

        public string SellingAccountName { get; set; }

        public string SellingAccountPhone { get; set; }

        public string BuyingAccountName { get; set; }

        public string BuyingAccountPhone { get; set; }

        public string RepliedContent { get; set; }

        public DateTime? CreatedTime { get; set; }

        public int TotalRecord { get; set; }
    }

    public class DealerComplaintSQLModel
    {
        public Guid DealerComplaintId { get; set; }

        public string CollectDealTransactionCode { get; set; }

        public string ComplaintContent { get; set; }

        public string SellingAccountName { get; set; }

        public string SellingAccountPhone { get; set; }

        public string BuyingAccountName { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string RepliedContent { get; set; }

        public DateTime? CreatedTime { get; set; }

        public int TotalRecord { get; set; }
    }
}
