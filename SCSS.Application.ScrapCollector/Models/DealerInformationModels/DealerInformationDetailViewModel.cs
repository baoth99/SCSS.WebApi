using System;

namespace SCSS.Application.ScrapCollector.Models.DealerInformationModels
{
    public class DealerInformationDetailViewModel
    {
        public Guid DealerId { get; set; }

        public string DealerName { get; set; }

        public string DealerImageUrl { get; set; }

        public string DealerPhone { get; set; }

        public float Rating { get; set; }

        public string OpenTime { get; set; }

        public string CloseTime { get; set; }

        public string DealerAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

    }
}
