
using System;

namespace SCSS.AWSService.Models
{
    public class TransactionAwardAmountCacheViewModel
    {
        public long AppliedAmount { get; set; }

        public float Amount { get; set; }
    }


    public class PendingCollectingRequestCacheModel
    {
        public Guid Id { get; set; }

        public DateTime? Date { get; set; }

        public TimeSpan? FromTime { get; set; }

        public TimeSpan? ToTime { get; set; }
    }
}
