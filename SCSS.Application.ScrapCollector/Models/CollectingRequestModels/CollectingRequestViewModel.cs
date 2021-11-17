using System;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string SellerName { get; set; }

        // Date
        public int? DayOfWeek { get; set; }

        public DateTime? CollectingRequestDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        // Address
        public string Area { get; set; }

        public string DurationTimeText { get; set; }

        public float DurationTimeVal { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public bool IsBulky { get; set; }

        public int? RequestType { get; set; }

        public float Distance { get; set; }

        public string DistanceText { get; set; }
    }
}
