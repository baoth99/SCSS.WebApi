using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Models.StatisticModels
{
    public class TransactionServiceFeeViewModel
    {
        public Guid Id { get; set; }

        public string TimePeriod { get; set; }

        public long? Amount { get; set; }
    }
}
