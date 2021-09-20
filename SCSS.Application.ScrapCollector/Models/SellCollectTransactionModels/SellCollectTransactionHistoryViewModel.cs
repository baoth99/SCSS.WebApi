using System;

namespace SCSS.Application.ScrapCollector.Models.SellCollectTransactionModels
{
    public class SellCollectTransactionHistoryViewModel
    {
        public Guid CollectingRequestId { get; set; }

        public string CollectingRequestCode { get; set; }

        public int? DayOfWeek { get; set; }

        public string Date { get; set; }

        public string Time { get; set; }

        public long? Total { get; set; }

        public int? Status { get; set; }

    }
}
