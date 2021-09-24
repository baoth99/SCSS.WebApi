using System;

namespace SCSS.Application.ScrapSeller.Models.NotificationModels
{
    public class NotificationViewModel
    {
        public Guid Id { get; set; }

        public string Title { get; set; }

        public string Body { get; set; }

        public string Date { get; set; }

        public string DataCustom { get; set; }

        public bool IsRead { get; set; }

        public string PreviousTime  { get; set; }
    }
}
