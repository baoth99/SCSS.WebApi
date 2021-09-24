using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapSeller.Models.ActivityModels
{
    public class ActivityDetailViewModel
    {
        public Guid Id { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedTime { get; set; }

        public string CollectingRequestCode { get; set; }

        public int? Status { get; set; }

        // Collector Information
        public CollectorInformation CollectorInfo { get; set; }

        // Collecting Request Information Detail
        public string AddressName { get; set; }

        public string Address { get; set; }

        public string CollectingRequestDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public bool IsBulky { get; set; }

        public string ScrapCategoryImageUrl { get; set; }

        public string Note { get; set; }

        // Transaction Information
        public TransactionInformation Transaction { get; set; }

    }

    public class CollectorInformation
    {
        public string Name { get; set; }

        public string  Phone { get; set; }
    }

    public class TransactionInformation
    {
        public string TransactionDate { get; set; }

        public string TransactionTime { get; set; }

        public long? Total { get; set; }

        public long? Fee { get; set; }

        public int? AwardPoint { get; set; }

        public List<TransactionInformationDetail> Details { get; set; }
    }

    public class TransactionInformationDetail
    {
        public string ScrapCategoryName { get; set; }

        public float? Quantity { get; set; }

        public string Unit { get; set; }

        public long? Total { get; set; }
    }
}
