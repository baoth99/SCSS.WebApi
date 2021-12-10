using System;

namespace SCSS.Application.ScrapSeller.Models.CollectingRequestModels
{
    public class CollectingRequestNoticeModel
    {
        public Guid Id { get; set; }

        public int? RequestType { get; set; }

        public int? Status { get; set; }
    }
}
