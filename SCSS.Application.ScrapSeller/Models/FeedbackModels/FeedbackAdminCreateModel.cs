using System;

namespace SCSS.Application.ScrapSeller.Models.FeedbackModels
{
    public class FeedbackAdminCreateModel
    {
        public Guid? CollectingRequestId { get; set; }

        public string SellingFeedback { get; set; }

    }
}
