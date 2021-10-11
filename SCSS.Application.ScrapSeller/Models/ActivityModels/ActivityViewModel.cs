using System;
using System.Text.Json.Serialization;

namespace SCSS.Application.ScrapSeller.Models.ActivityModels
{
    public class ActivityViewModel
    {
        public Guid CollectingRequestId { get; set; }

        public string CollectingRequestCode { get; set; }

        public string CollectingRequestDate { get; set; }

        public string FromTime { get; set; }

        public string ToTime { get; set; }   

        public int? Status { get; set; }

        public bool IsBulky { get; set; }

        public string AddressName { get; set; }

        public string Address { get; set; }

        public long? Total { get; set; }

        [JsonIgnore]
        public DateTime? CompletedTime { get; set; }
    }
}
