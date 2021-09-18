using SCSS.MapService.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
