using SCSS.AWSService.Interfaces;
using SCSS.MapService.Interfaces;
using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.MapService.Implementations
{
    public class MapDistanceMatrixService : MapBaseService, IMapDistanceMatrixService
    {
        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapDistanceMatrixService"/> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        public MapDistanceMatrixService(ILoggerService loggerService) : base(loggerService)
        {
        }

        #endregion

        #region Gets the distance matrix from origin to multiple destinations.

        /// <summary>
        /// Gets the distance matrix from origin to multiple destinations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<List<DistanceMatrixCoordinateResponseModel>> GetDistanceFromOriginToMultiDestinations(DistanceMatrixCoordinateRequestModel model)
        {
            if (!model.DestinationItems.Any())
            {
                return CollectionConstants.Empty<DistanceMatrixCoordinateResponseModel>();
            }

            // Get Request URL 
            var requestUri = GetDistanceMatrixEndpoint(model);

            // Call to Goong Server
            var httpResponse = await ConnectToGoongMapService(requestUri);

            if ((int)httpResponse.StatusCode != HttpStatusCodes.Ok)
            {
                return CollectionConstants.Empty<DistanceMatrixCoordinateResponseModel>();
            }

            // Get Response Data (Json string)
            var responseContent = await httpResponse.Content.ReadAsStringAsync();

            // Parse JsonString into GoongMapDistanceMatrixResponseRow Model
            var resultData = responseContent.ToMapperObject<GoongMapDistanceMatrixResponseRow>();

            // Get Elements 
            var distanceElements = resultData.Rows[0].Elements;

            var result = model.DestinationItems.Select(x => new DistanceMatrixCoordinateResponseModel()
            {
                DestinationId = x.Id,
            }).ToList();

            for (int i = 0; i < distanceElements.Count; i++)
            {
                result[i].DistanceText = distanceElements[i].Distance.Text;
                result[i].DistanceVal = distanceElements[i].Distance.Value;
            }

            return result;
        }

        #endregion Gets the distance matrix from origin to multiple destinations.

    }
}
