using System;
using System.Collections.Generic;

namespace SCSS.Aplication.BackgroundService.Models.MessageNotificationModels
{
    public class MessageNotificationPushModel
    {
        public Guid? AccountId { get; set; }

        public string DeviceId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public Dictionary<string, string> DataCustom { get; set; }
    }
}
