using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestDetailReceivingViewModel
    {
        // Collecting Request Information
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string CollectingRequestDate { get; set; }

        public int? DayOfWeek { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public bool IsBulky { get; set; }

        public string Note { get; set; }

        public string ScrapImgUrl { get; set; }

        // Location
        public string CollectingAddressName { get; set; }

        public string CollectingAddress { get; set; }

        public decimal? Latitude { get; set; }

        public decimal? Longtitude { get; set; }

        // Seller Information

        public string SellerImgUrl { get; set; }

        public string SellerName { get; set; }

        public string SellerPhone { get; set; }

        public int? SellerGender { get; set; }

    }
}
