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
    public class StatisticController :  BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The statistic service
        /// </summary>
        private readonly IStatisticService _statisticService;

        #endregion

        #region Constructor

        public StatisticController(IStatisticService statisticService)
        {
            _statisticService = statisticService;
        }

        #endregion


    }
}
