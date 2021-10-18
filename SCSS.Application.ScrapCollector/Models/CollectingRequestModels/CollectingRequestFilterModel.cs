using SCSS.Validations.ValidationAttributes;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestFilterModel : BaseFilterModel
    {
        public double Radius { get; set; }

        [CoordinateValidation]
        public decimal? OriginLatitude { get; set; }

        [CoordinateValidation]
        public decimal? OriginLongtitude { get; set; }
    }
}
