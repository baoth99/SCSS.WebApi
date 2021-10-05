using System;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class CollectDealTransactionHistoryViewModel
    {
        public Guid Id { get; set; }

        public string CollectorImage { get; set; }

        public string CollectorName { get; set; }

        public DateTime? TransactionDateTime { get; set; }

        public long? Total { get; set; }
    }
}
