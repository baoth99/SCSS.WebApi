using System;


namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestResponseModel
    {
        public Guid Id { get; set; }

        public Guid? SellerAccountId { get; set; }

        public string CollectingRequestCode { get; set; }

        public int? RequestType { get; set; }

        public int? Status { get; set; }
    }
}
