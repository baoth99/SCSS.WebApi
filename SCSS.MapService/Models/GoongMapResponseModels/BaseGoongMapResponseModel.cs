using System.Collections.Generic;


namespace SCSS.MapService.Models.GoongMapResponseModels
{
    public class GoongMapDistanceMatrixResponseRow
    {
        public List<GoongMapDistanceMatrixResponseElement> Rows { get; set; }

    }

    public class GoongMapDistanceMatrixResponseElement
    {
        public List<DistanceMatrixResponseModel> Elements { get; set; }
    }

    public class ResponseDetail
    {
        public string Text { get; set; }

        public float Value { get; set; }
    }


    public class GoongMapAutoCompleteResponsePredictions
    {
        public List<AutoCompleteResponseModel> Predictions { get; set; }

        public int Executed_time { get; set; }

        public int Executed_time_all { get; set; }

        public string Status { get; set; }
    }

    public class GoongMapGeocodingResponse
    {
        public object Plus_code { get; set; }

        public List<GeocodingResponseModel> Results { get; set; }
    }

    public class GoongMapPlaceDetailResponse
    {
        public PlaceDetailResponseModel Result { get; set; }

        public string Status { get; set; }
    }
}
