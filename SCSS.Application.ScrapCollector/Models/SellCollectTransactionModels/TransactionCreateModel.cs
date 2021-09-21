using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels
{
    public class SellCollectTransactionCreateModel
    {
        public Guid CollectingRequestId { get; set; }

        public long TransactionServiceFee { get; set; }

        public long Total { get; set; }

        public List<ScrapCategoryTransactionDetailCreateModel> ScrapCategoryItems { get; set; }
    }

    public class ScrapCategoryTransactionDetailCreateModel
    {
        public Guid? CollectorCategoryDetailId { get; set; }

        public float? Quantity { get; set; }

        public long? Total { get; set; }

        public long? Price { get; set; }
    }
}
