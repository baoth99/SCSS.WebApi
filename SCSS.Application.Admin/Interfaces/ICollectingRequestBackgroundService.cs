using System;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ICollectingRequestBackgroundService
    {
        /// <summary>
        /// Trails the collecting request in day background.
        /// </summary>
        /// <returns></returns>
        Task TrailCollectingRequestInDayBackground();
    }
}
