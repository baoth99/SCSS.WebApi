using System;

namespace SCSS.Application.ScrapSeller.Models.CollectingRequestModels
{
    public class CollectingRequestViewModel
    {
        public Guid Id { get; set; }
        public string CollectingRequestCode { get; set; }

        public string CollectingRequestDate { get; set; }

        public int? Status { get; set; }

        public string AddressName { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

    }
}
