﻿using System;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string SellerName { get; set; }

        // Date
        public int? DayOfWeek { get; set; }

        public string CollectingRequestDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        // Address
        public string CollectingAddressName { get; set; }

        public string CollectingAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        public bool IsBulky { get; set; }

        public float Distance { get; set; }

        public string DistanceText { get; set; }
    }
}