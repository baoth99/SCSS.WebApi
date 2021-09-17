using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;


namespace SCSS.MapService.Models
{
    public class DistanceMatrixCoordinateRequestModel
    {
        public decimal OriginLatitude { get; set; }

        public decimal OriginLongtitude { get; set; }

        public List<DestinationCoordinateModel> DestinationItems { get; set; }

        public Vehicle Vehicle { get; set; }
    }

    public class DistanceMatrixCoordinateResponseModel
    {
        public Guid DestinationId { get; set; }

        public string DistanceText { get; set; }

        public float DistanceVal { get; set; }

    }

}
