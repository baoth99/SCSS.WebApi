using System;

namespace SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels
{
    public class TransactionScrapCategoryViewModel
    {
        public Guid Id { get; set; }

        public string Name { get; set; }
    }

    public class TransactionScrapCategoryDetailViewModel
    {
        public Guid Id { get; set; }

        public string Unit { get; set; }

        public long Price { get; set; }
    }
}
