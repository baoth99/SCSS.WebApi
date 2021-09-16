using System;


namespace SCSS.Application.ScrapSeller.Models.CollectingRequestModels
{
    public class CollectingRequestCancelModel
    {
        public Guid Id { get; set; }

        public string CancelReason { get; set; }
    }
}
