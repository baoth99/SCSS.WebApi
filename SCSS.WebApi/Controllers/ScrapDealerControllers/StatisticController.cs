using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.StatisticModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class StatisticController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The statistic service
        /// </summary>
        private readonly IStatisticService _statisticService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="StatisticController"/> class.
        /// </summary>
        /// <param name="statisticService">The statistic service.</param>
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
        [Route(ScrapDealerApiUrlDefinition.StatisticApiUrl.GetStatistic)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetStatistic([FromQuery] StatisticDateFilterModel model)
        {
            return await _statisticService.GetStatisticInRangeTime(model);
        }

        #endregion

    }
}
