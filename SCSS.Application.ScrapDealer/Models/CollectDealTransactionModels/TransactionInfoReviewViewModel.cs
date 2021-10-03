using System;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class TransactionInfoReviewViewModel
    {
        public Guid CollectorId { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public float? TransactionFeePercent { get; set; }
    }
}
