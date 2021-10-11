using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SCSS.MapService.Interfaces
{
    public interface IMapDistanceMatrixService
    {
        /// <summary>
        /// Gets the distance matrix from origin to multiple destinations.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<List<DistanceMatrixCoordinateResponseModel>> GetDistanceFromOriginToMultiDestinations(DistanceMatrixCoordinateRequestModel model);

    }
}
