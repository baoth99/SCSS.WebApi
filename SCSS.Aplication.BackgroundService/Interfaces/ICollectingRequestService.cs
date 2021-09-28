using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface ICollectingRequestService
    {
        Task TrailCollectingRequestInDayBackground();
    }
}
