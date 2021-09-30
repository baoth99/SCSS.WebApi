﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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


        Task ScanToCancelCollectingRequest();
    }
}