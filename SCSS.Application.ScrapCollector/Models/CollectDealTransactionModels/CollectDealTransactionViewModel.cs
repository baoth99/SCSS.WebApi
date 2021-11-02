using System;

namespace SCSS.Application.ScrapCollector.Models.CollectDealTransactionModels
{
    public class CollectDealTransactionViewModel
    {
        public Guid TransactionId { get; set; }

        public string TransactionCode { get; set; }

        public string DealerName { get; set; }

        public string DealerImageURL { get; set; }

        public string TransactionDate { get; set; }

        public string TransactionTime { get; set; }

        public int? DayOfWeek { get; set; }

        public long? Total { get; set; }


        public long? TransactionServiceFee { get; set; }

        public long? Bonus { get; set; }
    }
}
