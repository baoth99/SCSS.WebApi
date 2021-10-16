using System;


namespace SCSS.Application.Admin.Models.ComplaintModels
{
    public class SellerComplaintViewModel
    {
        public Guid Id { get; set; }

        public string CollectingRequestCode { get; set; }

        public string ComplaintContent { get; set; }

        public string SellingInfo { get; set; }

        public string BuyingInfo { get; set; }

        public string RepliedContent { get; set; }

        public bool WasReplied { get; set; }

        public string ComplaintTime { get; set; }
    }
}
