
namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestFilterModel
    {
        public int ScreenSize { get; set; }

        public decimal? GeocodeLatitude { get; set; }

        public decimal? GeocodeLongtitude { get; set; }

        public string FilterDate { get; set; }
    }
}
