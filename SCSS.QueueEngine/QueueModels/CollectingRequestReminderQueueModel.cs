using System;

namespace SCSS.QueueEngine.QueueModels
{
    public class CollectingRequestReminderQueueModel
    {
        public Guid Id { get; set; }

        public Guid? CollectorId { get; set; }

        public string CollectingRequestCode { get; set; }

        public DateTime? RequestDate { get; set; }

        public TimeSpan? FromTime { get; set; }

        public TimeSpan? ToTime { get; set; }
    }
}
