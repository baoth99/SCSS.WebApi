using Microsoft.AspNetCore.Mvc;
using SCSS.MapService.Interfaces;
using SCSS.MapService.Models;
using SCSS.MapService.Models.GoongMapResponseModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class MapController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The map place service
        /// </summary>
        private readonly IMapPlaceService _mapPlaceService;

        /// <summary>
        /// The map geocoding service
        /// </summary>
        private readonly IMapGeocodingService _mapGeocodingService;

        #endregion

        #region Constructor        

        /// <summary>
        /// Initializes a new instance of the <see cref="MapController"/> class.
        /// </summary>
        /// <param name="mapPlaceService">The map place service.</param>
        /// <param name="mapGeocodingService">The map geocoding service.</param>
        public MapController(IMapPlaceService mapPlaceService, IMapGeocodingService mapGeocodingService)
        {
            _mapPlaceService = mapPlaceService;
            _mapGeocodingService = mapGeocodingService;
        }

        #endregion

        #region Places Search by keyword with autocomplete

        /// <summary>
        /// Placeses the search.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GoongMapAutoCompleteResponsePredictions), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.MapApiUrl.AutoComplete)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<GoongMapAutoCompleteResponsePredictions> PlacesSearch([FromQuery] PlaceAutoCompleteRequestModel model)
        {
            return await _mapPlaceService.SearchByKeyWord(model);
        }

        #endregion

        #region Get place detail by Id

        /// <summary>
        /// Gets the place detail.
        /// </summary>
        /// <param name="placeid">The placeid.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GoongMapPlaceDetailResponse), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.MapApiUrl.PlaceDetail)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<GoongMapPlaceDetailResponse> GetPlaceDetail([FromQuery] string placeid)
        {
            return await _mapPlaceService.GetPlaceDetail(placeid);
        }

        #endregion

        #region Reverse Geocoding

        /// <summary>
        /// Reverses the geocoding.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(GoongMapGeocodingResponse), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.MapApiUrl.ReverseGeocoding)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<GoongMapGeocodingResponse> ReverseGeocoding([FromQuery] GeocodeRequestModel model)
        {
            return await _mapGeocodingService.ReverseGeocoding(model);
        }

        #endregion

    }
}
