using System;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestDetailViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string SellerName { get; set; }

        public string ScrapImageUrl { get; set; }

        // Date
        public int? DayOfWeek { get; set; }

        public string CollectingRequestDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        // Location

        public string Area { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public bool IsBulky { get; set; }

        public string Note { get; set; }
    }
}
