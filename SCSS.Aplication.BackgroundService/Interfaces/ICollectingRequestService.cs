using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface ICollectingRequestService
    {
        /// <summary>
        /// Trails the collecting request in day background.
        /// </summary>
        /// <returns></returns>
        Task TrailCollectingRequestInDayBackground();

        /// <summary>
        /// Scans to cancel collecting request.
        /// </summary>
        /// <returns></returns>
        Task ScanToCancelCollectingRequest();
    }
}
