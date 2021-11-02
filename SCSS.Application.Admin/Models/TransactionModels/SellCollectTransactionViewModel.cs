using System;

namespace SCSS.Application.Admin.Models.TransactionModels
{
    public class SellCollectTransactionViewModel
    {
        public Guid Id { get; set; }

        public string TransactionCode { get; set; }

        public string SellerName { get; set; }

        public string SellerPhone { get; set; }

        public string CollectorName { get; set; }

        public string CollectorPhone { get; set; }

        public string TransactionTime { get; set; }

        public long? TotalPrice { get; set; }

    }
}
