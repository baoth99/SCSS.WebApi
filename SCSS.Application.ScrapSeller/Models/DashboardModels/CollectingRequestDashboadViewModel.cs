using System;

namespace SCSS.Application.ScrapSeller.Models.DashboardModels
{
    public class CollectingRequestDashboadViewModel
    {
        public Guid CollectingRequestId { get; set; }

        public string CollectingRequestCode { get; set; }

        public string CollectingRequestDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }

        public string AddressName { get; set; }

        public string Address { get; set; }

        public bool IsBulky { get; set; }

        public int? Status { get; set; }

    }
}
