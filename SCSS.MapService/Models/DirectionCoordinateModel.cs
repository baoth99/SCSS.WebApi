using SCSS.Utilities.Constants;
using System.Collections.Generic;

namespace SCSS.MapService.Models
{
    public class DirectionCoordinateModel
    {
        public decimal OriginLatitude { get; set; }

        public decimal OriginLongtitude { get; set; }

        public List<DestinationCoordinateModel> DestinationItems { get; set; }

        public bool Alternative { get; set; } = true;

        public Vehicle Vehicle { get; set; }
    }
}
