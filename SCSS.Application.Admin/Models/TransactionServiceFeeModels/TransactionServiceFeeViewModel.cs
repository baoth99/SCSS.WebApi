
using System.Collections.Generic;

namespace SCSS.Application.Admin.Models.TransactionServiceFeeModels
{
    public class TransactionServiceFeeViewModel
    {
        public float? Percent { get; set; }

        public List<TransactionServiceFeeHistoryViewModel> Histories { get; set; }
    }


    public class TransactionServiceFeeHistoryViewModel
    {
        public float? Percent { get; set; }

        public string DeActiveTime { get; set; }
    }
}
