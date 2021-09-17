using System;


namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestRejectModel
    {
        public Guid Id { get; set; }

        public string Reason { get; set; }
    }
}
