using System;

namespace SCSS.Application.ScrapCollector.Models.DealerInformationModels
{
    public class DealerInformationViewModel
    {
        public Guid Id { get; set; }

        public string DealerName { get; set; }

        public bool IsActive { get; set; }
        // Location
        public string DealerAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public string DealerImageUrl { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public float Distance { get; set; }

        public string DistanceText { get; set; }

    }
}
