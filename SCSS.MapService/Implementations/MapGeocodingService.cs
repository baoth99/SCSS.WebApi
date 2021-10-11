using SCSS.AWSService.Interfaces;
using SCSS.MapService.Interfaces;
using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using SCSS.Utilities.Extensions;
using System.Threading.Tasks;

namespace SCSS.MapService.Implementations
{
    public class MapGeocodingService : MapBaseService, IMapGeocodingService
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapGeocodingService"/> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        public MapGeocodingService(ILoggerService loggerService) : base(loggerService)
        {
        }

        #endregion

        #region Reverse Geocoding

        /// <summary>
        /// Reverses the geocoding.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<GoongMapGeocodingResponse> ReverseGeocoding(GeocodeRequestModel model)
        {
            // Get Request URL 
            var requestUri = GetGeocodeEndpoint(model);

            // Call to Goong Server
            var httpResponse = await ConnectToGoongMapService(requestUri);

            // Get Response Data (Json string)
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Parse JsonString into GoongMapGeocodingResponse Model
            var resultData = responseContent.ToMapperObject<GoongMapGeocodingResponse>();

            return resultData;
        }

        #endregion
    }
}
