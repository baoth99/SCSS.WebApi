using SCSS.Validations.ValidationAttributes;

namespace SCSS.Application.ScrapCollector.Models.CollectingRequestModels
{
    public class CollectingRequestFilterModel : BaseFilterModel
    {
        [CoordinateValidation]
        public decimal? OriginLatitude { get; set; }

        [CoordinateValidation]
        public decimal? OriginLongtitude { get; set; }
    }
}
