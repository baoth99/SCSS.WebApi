using SCSS.MapService.Models.GoongMapResponseModels;
using SCSS.Utilities.Constants;
using System.Collections.Generic;

namespace SCSS.MapService.Models
{

    public class DirectionRequestModel
    {
        public decimal OriginLatitude { get; set; }

        public decimal OriginLongtitude { get; set; }

        public DirectionCoordinateModel DestinationItem { get; set; }

        public Vehicle Vehicle { get; set; } = Vehicle.hd;
    }

    public class DirectionCoordinateRequestModel
    {
        public decimal OriginLatitude { get; set; }

        public decimal OriginLongtitude { get; set; }

        public List<DirectionCoordinateModel> DestinationItems { get; set; }

        public bool Alternative { get; set; } = true;

        public Vehicle Vehicle { get; set; }
    }


    public class DirectionCoordinateResponseModel
    {
        public List<object> Geocoded_waypoints { get; set; }

        public List<DirectionRouteModel> Routes { get; set; }
    }

    public class DirectionRouteModel
    {
        public object Bounds { get; set; }

        public List<DirectionLegsModel> Legs { get; set; }

        public Overview_Polyline Overview_polyline { get; set; }

        public List<object> Warnings { get; set; }

        public List<object> Waypoint_order { get; set; }
    }

    public class DirectionLegsModel
    {
        public ResponseDetail Distance { get; set; }

        public ResponseDetail Duration { get; set; }

        public List<string> Steps { get; set; }
    }

    public class Overview_Polyline
    {
        public string Points { get; set; }
    } 
}
