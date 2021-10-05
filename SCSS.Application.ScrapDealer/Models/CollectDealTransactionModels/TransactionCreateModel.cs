using System;
using System.Collections.Generic;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class TransactionCreateModel
    {
        public Guid CollectorId { get; set; }

        public long? TransactionFee { get; set; }

        public long? Total { get; set; }

        public long? TotalBonus { get; set; }

        public List<ScrapCategoryTransactionDetailCreateModel> Items { get; set; }
    }

    public class ScrapCategoryTransactionDetailCreateModel
    {
        public Guid? DealerCategoryDetailId { get; set; }

        public float? Quantity { get; set; }

        public Guid PromotionId { get; set; }

        public long? BonusAmount { get; set; }

        public long? Total { get; set; }

        public long? Price { get; set; }
    }
}
