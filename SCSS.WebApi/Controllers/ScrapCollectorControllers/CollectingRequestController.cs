using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class CollectingRequestController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The collecting request service
        /// </summary>
        private readonly ICollectingRequestService _collectingRequestService;

        #endregion

        #region Constructor

        public CollectingRequestController(ICollectingRequestService collectingRequestService)
        {
            _collectingRequestService = collectingRequestService;
        }

        #endregion



    }
}
