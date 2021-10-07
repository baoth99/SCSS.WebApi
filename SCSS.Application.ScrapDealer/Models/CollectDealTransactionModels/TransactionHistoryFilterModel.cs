using System;

namespace SCSS.Application.ScrapDealer.Models.CollectDealTransactionModels
{
    public class TransactionHistoryFilterModel : BaseFilterModel
    {
        public Guid? DealerAccountId { get; set; }

        public string FromDate { get; set; }

        public string ToDate { get; set; }

        public long FromTotal { get; set; }

        public long ToTotal { get; set; }
    }
}
