using System;

namespace SCSS.MapService.Models
{
    public class DestinationCoordinateModel
    {
        public Guid Id { get; set; }

        public decimal DestinationLatitude { get; set; }

        public decimal DestinationLongtitude { get; set; }
    }
}
