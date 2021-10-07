
using SCSS.Validations.ValidationAttributes;

namespace SCSS.Application.ScrapCollector.Models.DealerInformationModels
{
    public class DealerInformationFilterModel : BaseFilterModel
    {
        public string SearchWord { get; set; }

        public double Radius { get; set; }

        [CoordinateValidation]
        public decimal? OriginLatitude { get; set; }

        [CoordinateValidation]
        public decimal? OriginLongtitude { get; set; }
    }
}
