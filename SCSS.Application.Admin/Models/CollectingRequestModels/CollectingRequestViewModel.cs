using System;

namespace SCSS.Application.Admin.Models.CollectingRequestModels
{
    public class CollectingRequestViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string CollectingRequestDate { get; set; }

        public string CollectingRequestRangeTime { get; set; }

        public string RequestedBy { get; set; }

        public string RecevicedBy { get; set; }

        public int? Status { get; set; }
    }
}
