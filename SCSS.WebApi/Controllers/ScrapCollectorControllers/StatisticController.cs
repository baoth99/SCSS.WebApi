using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.StatisticModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
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

        #region Get Statistic

        /// <summary>
        /// Gets the statistic.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.StatisticApiUrl.GetStatistic)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetStatistic([FromQuery] StatisticDateFilterModel model)
        {
            return await _statisticService.GetStatisticInTimeRange(model);
        }

        #endregion


        #region Get Statistic

        /// <summary>
        /// Gets the statistic.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.StatisticApiUrl.GetServiceFee)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetServiceFee()
        {
            return await _statisticService.GetServiceFeeInMonth();
        }

        #endregion
    }
}
