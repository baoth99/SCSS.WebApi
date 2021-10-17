using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using System.Threading.Tasks;

namespace SCSS.MapService.Interfaces
{
    public interface IMapGeocodingService
    {
        /// <summary>
        /// Reverses the geocoding.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<GoongMapGeocodingResponse> ReverseGeocoding(GeocodeRequestModel model);

    }
}
