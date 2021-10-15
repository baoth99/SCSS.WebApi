using System;

namespace SCSS.Application.ScrapSeller.Models.ComplaintModel
{
    public class ComplaintCreateModel
    {
        public Guid? CollectingRequestId { get; set; }

        public string ComplaintContent { get; set; }
    }
}
