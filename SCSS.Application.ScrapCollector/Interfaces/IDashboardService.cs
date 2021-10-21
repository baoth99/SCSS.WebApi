using SCSS.Application.ScrapCollector.Models.DashboardModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IDashboardService
    {
        /// <summary>
        /// Gets the first approved collecting request.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetFirstApprovedCollectingRequest(DashboardCollectingRequestFilterModel model);
    }
}
