using System.Collections.Generic;


namespace SCSS.Application.Admin.Models.TransactionAwardAmountModels
{
    public class TransactionAwardAmountViewModel
    {
        public long? AppliedAmount { get; set; }

        public float? Amount { get; set; }

        public List<TransactionAwardAmountHistoryViewModel> Histories { get; set; }
    }

    public class TransactionAwardAmountHistoryViewModel
    {
        public long? AppliedAmount { get; set; }

        public float? Amount { get; set; }

        public string DeActiveTime { get; set; }
    }
}
