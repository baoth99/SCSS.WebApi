
namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestReceivingFilterModel : BaseFilterModel
    {
        public string SellerPhone { get; set; }

        public decimal? OriginLatitude { get; set; }

        public decimal? OriginLongtitude { get; set; }

    }
}
