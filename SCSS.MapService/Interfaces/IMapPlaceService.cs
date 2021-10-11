using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using System.Threading.Tasks;

namespace SCSS.MapService.Interfaces
{
    public interface IMapPlaceService
    {
        /// <summary>
        /// Searches the by key word.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<GoongMapAutoCompleteResponsePredictions> SearchByKeyWord(PlaceAutoCompleteRequestModel model);

        /// <summary>
        /// Gets the place detail.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <returns></returns>
        Task<GoongMapPlaceDetailResponse> GetPlaceDetail(string placeId);
    }
}
