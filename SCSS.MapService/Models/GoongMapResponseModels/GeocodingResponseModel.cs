using System.Collections.Generic;


namespace SCSS.MapService.Models.GoongMapResponseModels
{
    public class GeocodingResponseModel
    {
        public List<AddressComponents> Address_components { get; set; }

        public string Formatted_address { get; set; }

        public Geometry Geometry { get; set; }

        public string Place_id { get; set; }

        public string Reference { get; set; }

        public PlusCode Plus_code { get; set; }

        public List<string> Types { get; set; }
    }

    public class AddressComponents
    {
        public string Long_name { get; set; }

        public string Short_name { get; set; }
    }

    public class Geometry
    {
        public Location Location { get; set; }
    }

    public class Location
    {
        public decimal Lat { get; set; }

        public decimal Lng { get; set; }
    }
}

