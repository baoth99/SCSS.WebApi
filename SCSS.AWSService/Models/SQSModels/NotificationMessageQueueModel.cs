using System;
using System.Collections.Generic;

namespace SCSS.AWSService.Models.SQSModels
{
    public class NotificationMessageQueueModel
    {
        public Guid? AccountId { get; set; }

        public string DeviceId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string NotiType { get; set; }

        public Guid? ReferenceRecordId { get; set; }

        public Dictionary<string, string> DataCustom { get; set; }
    }
}
