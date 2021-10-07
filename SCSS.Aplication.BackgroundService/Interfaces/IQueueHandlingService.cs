using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface IQueueHandlingService
    {
        /// <summary>
        /// Handles the collecting request reminder queue.
        /// </summary>
        /// <returns></returns>
        Task HandleCollectingRequestReminderQueue();
    }
}
