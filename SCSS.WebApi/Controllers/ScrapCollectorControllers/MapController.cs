using Microsoft.AspNetCore.Mvc;
using SCSS.MapService.Interfaces;
using SCSS.MapService.Models;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class MapController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The map distance matrix service
        /// </summary>
        private readonly IMapDistanceMatrixService _mapDistanceMatrixService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapController"/> class.
        /// </summary>
        /// <param name="mapDistanceMatrixService">The map distance matrix service.</param>
        public MapController(IMapDistanceMatrixService mapDistanceMatrixService)
        {
            _mapDistanceMatrixService = mapDistanceMatrixService;
        }

        #endregion

        #region Get Directions

        /// <summary>
        /// Gets the directions.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(DirectionCoordinateResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.MapApiUrl.GetDirection)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<DirectionCoordinateResponseModel> GetDirections([FromQuery] DirectionRequestModel model)
        {
            var directionModel = new DirectionCoordinateRequestModel()
            {
                OriginLatitude = model.OriginLatitude,
                OriginLongtitude = model.OriginLongtitude,
                DestinationItems = new List<DirectionCoordinateModel>() { model.DestinationItem},
                Alternative = BooleanConstants.TRUE,
                Vehicle = model.Vehicle
            };

            return await _mapDistanceMatrixService.GetDirections(directionModel);
        }

        #endregion

    }
}
