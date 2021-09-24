using System;


namespace SCSS.Application.ScrapSeller.Models.ActivityModels
{
    public class ActivityViewModel
    {
        public Guid CollectingRequestId { get; set; }

        public string CollectingRequestCode { get; set; }

        public string CollectingRequestDate { get; set; }

        public string CreatedDate { get; set; }

        public string CreatedTime { get; set; }

        public int? Status { get; set; }

        public bool IsBulky { get; set; }

        public string AddressName { get; set; }

        public long? Total { get; set; }

    }
}
