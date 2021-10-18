using System;

namespace SCSS.Aplication.BackgroundService.Models.RequestNotifierModels
{
    public class RequestNotifierRequestModel
    {
        public Guid CollectingRequestId { get; set; }

        public decimal Latitude { get; set; }

        public decimal Longitude { get; set; }

        public int RequestType { get; set; }
    }
}
