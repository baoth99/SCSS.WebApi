using System;


namespace SCSS.Application.ScrapCollector.Models.StatisticModels
{
    public class TransactionServiceFeeViewModel
    {
        public Guid Id { get; set; }

        public string TimePeriod { get; set; }

        public DateTime? DateTimeFrom { get; set; }

        public DateTime? DateTimeTo { get; set; }

        public bool IsFinished { get; set; }

        public long? Amount { get; set; }
    }
}
