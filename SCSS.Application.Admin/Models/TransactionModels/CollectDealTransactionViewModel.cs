using System;

namespace SCSS.Application.Admin.Models.TransactionModels
{
    public class CollectDealTransactionViewModel
    {
        public Guid Id { get; set; }

        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string DealerPhone { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public string TransactionTime { get; set; }

        public long? TotalPrice { get; set; }
    }
}
