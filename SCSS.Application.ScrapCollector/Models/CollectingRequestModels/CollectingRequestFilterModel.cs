﻿namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestFilterModel
    {
        public int ScreenSize { get; set; }

        public decimal? OriginLatitude { get; set; }

        public decimal? OriginLongtitude { get; set; }

        public string FilterDate { get; set; }
    }
}