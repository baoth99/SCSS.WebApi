using System;

namespace SCSS.Application.Admin.Models.FeedbackModels
{
    public class FeedbackReplyModel
    {
        public Guid FeedbackId { get; set; }

        public string RepliedContent { get; set; }
    }
}
