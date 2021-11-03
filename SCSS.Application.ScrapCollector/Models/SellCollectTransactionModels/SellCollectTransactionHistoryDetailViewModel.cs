using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels
{
    public class SellCollectTransactionHistoryDetailViewModel
    {
        public string CollectingRequestCode { get; set; }

        public int? Status { get; set; }

        public string SellerName { get; set; }

        public int? DayOfWeek { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public ComplaintViewModel Complaint { get; set; }

        public List<TransactionDetailHistoryViewModel> Items { get; set; }

        public long? Total { get; set; }

        public long? TransactionFee { get; set; }

    }


    public class ComplaintViewModel
    {
        public Guid? ComplaintId { get; set; }

        public int ComplaintStatus { get; set; }

        public string ComplaintContent { get; set; }

        public string AdminReply { get; set; }
    }


    public class TransactionDetailHistoryViewModel
    {
        public string ScrapCategoryName { get; set; }

        public float? Quantity { get; set; }

        public string Unit { get; set; }

        public long? Total { get; set; }

    }
}
