using System;
using System.Collections.Generic;

namespace SCSS.Application.Admin.Models.NotificationModels
{
    public class NotificationCreateModel
    {
        public Guid? AccountId { get; set; }

        public string DeviceId { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public Dictionary<string, string> DataCustom { get; set; }
    }
}
