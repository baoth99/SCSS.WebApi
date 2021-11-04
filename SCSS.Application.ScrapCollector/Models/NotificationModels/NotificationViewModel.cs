using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Models.NotificationModels
{
    public class NotificationViewModel
    {
        public Guid? Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Date { get; set; }

        public string DataCustom { get; set; }

        public bool? IsRead { get; set; }

        public int? NotiType { get; set; }

        public string PreviousTime { get; set; }
    }
}
