using System;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class TransactionScrapCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public Guid? PromotionId { get; set; }

        public string PromotionCode { get; set; }

        public long? AppliedAmount { get; set; }

        public long? BonusAmount { get; set; }
    }

    public class TransactionScrapCategoryDetailViewModel
    {
        public Guid Id { get; set; }

        public string Unit { get; set; }

        public long Price { get; set; }
    }
}
