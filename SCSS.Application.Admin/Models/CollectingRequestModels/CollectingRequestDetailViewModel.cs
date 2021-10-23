using System;
using System.Collections.Generic;

namespace SCSS.Application.Admin.Models.CollectingRequestModels
{
    public class CollectingRequestDetailViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string RequestedBy { get; set; }

        public string ReceivedBy { get; set; }

        public bool IsBulky { get; set; }

        public string CollectingRequestDate { get; set; }

        public string CollectingRequestRangeTime { get; set; }

        public int? Status { get; set; }

        public string Address { get; set; }

        public string Note { get; set; }

    }
}
