using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>
        /// Gets the nearest collecting request.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetNearestCollectingRequest();
    }
}
