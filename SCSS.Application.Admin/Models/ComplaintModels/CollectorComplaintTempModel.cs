using System;


namespace SCSS.Application.Admin.Models.ComplaintModels
{
    public class CollectorComplaintTempModel
    {
        public Guid? CollectorAccountId { get; set; }

        public string ComplaintContent { get; set; }

        public string AdminReply { get; set; }

        public DateTime? CreatedTime { get; set; }

        public Guid? ComplaintedAccountId { get; set; }

        public Guid? CollectorComplaintId { get; set; }

        public Guid? CollectingRequestId { get; set; }

        public Guid? CollectDealTransactionId { get; set; }
    }
}
