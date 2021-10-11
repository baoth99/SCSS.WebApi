namespace SCSS.MapService.Models.GoongMapResponseModels
{
    public class PlaceDetailResponseModel
    {
        public string Place_id { get; set; }

        public string Formatted_address { get; set; }

        public Geometry Geometry { get; set; }

        public string Name { get; set; }
    }
}
