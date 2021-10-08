using System;

namespace SCSS.Application.ScrapCollector.Models.CollectDealTransactionModels
{
    public class CollectDealTransactionViewModel
    {
        public Guid TransactionId { get; set; }

        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string DealerImageURL { get; set; }

        public DateTime TransactionDate { get; set; }

        public string TransactionTime { get; set; }

        public long? Total { get; set; }
    }
}
