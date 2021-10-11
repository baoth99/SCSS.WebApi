using SCSS.AWSService.Interfaces;
using SCSS.MapService.Interfaces;
using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using SCSS.Utilities.Extensions;
using System.Threading.Tasks;

namespace SCSS.MapService.Implementations
{
    public class MapPlaceService : MapBaseService, IMapPlaceService
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapPlaceService"/> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        public MapPlaceService(ILoggerService loggerService) : base(loggerService)
        {
        }

        #endregion

        #region Search by keyword with autocomplete 

        /// <summary>
        /// Searches the by key word.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<GoongMapAutoCompleteResponsePredictions> SearchByKeyWord(PlaceAutoCompleteRequestModel model)
        {
            // Get Request URL 
            var requestUri = GetAutoCompleteEndpoint(model);

            // Call to Goong Server
            var httpResponse = await ConnectToGoongMapService(requestUri);

            // Get Response Data (Json string)
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Parse JsonString into GoongMapAutoCompleteResponsePredictions Model
            var resultData = responseContent.ToMapperObject<GoongMapAutoCompleteResponsePredictions>();

            return resultData;
        }

        #endregion

        #region Get Place Detail

        /// <summary>
        /// Gets the place detail.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <returns></returns>
        public async Task<GoongMapPlaceDetailResponse> GetPlaceDetail(string placeId)
        {
            // Get Request URL 
            var requestUri = GetPlaceDetailEndpoint(placeId);

            // Call to Goong Server
            var httpResponse = await ConnectToGoongMapService(requestUri);

            // Get Response Data (Json string)
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Parse JsonString into GoongMapPlaceDetailResponse Model
            var resultData = responseContent.ToMapperObject<GoongMapPlaceDetailResponse>();

            return resultData;
        }

        #endregion
    }
}
