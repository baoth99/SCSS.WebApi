using SCSS.AWSService.Interfaces;
using SCSS.MapService.Models;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace SCSS.MapService
{
    public class MapBaseService : IDisposable
    {
        #region Disponse

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        /// <summary>
        /// Gets the logger service.
        /// </summary>
        /// <value>
        /// The logger service.
        /// </value>
        protected ILoggerService LoggerService { get; private set; }

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="MapBaseService"/> class.
        /// </summary>
        /// <param name="loggerService">The logger service.</param>
        public MapBaseService(ILoggerService loggerService)
        {
            LoggerService = loggerService;
        }

        #endregion

        #region Connect To GoongMap Server

        /// <summary>
        /// Connects to goong map service.
        /// </summary>
        /// <param name="restApiEndpoint">The rest API endpoint.</param>
        /// <returns></returns>
        protected async Task<HttpResponseMessage> ConnectToGoongMapService(string restApiEndpoint)
        {
            HttpClientHandler clientHandler = new();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };

            HttpClient httpClient = new HttpClient();
            try
            {
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(ApplicationRestfulApi.ApplicationProduce));

                var responseResult = await httpClient.GetAsync(restApiEndpoint);

                return responseResult;
            }
            catch (Exception ex)
            {
                LoggerService.LogError(ex, "Fail !");
                return null;
            }
            finally
            {
                httpClient.Dispose();
            }
        }

        #endregion

        #region Get Distance Matrix Endpoint

        /// <summary>
        /// Gets the distance matrix endpoint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string GetDistanceMatrixEndpoint(DistanceMatrixCoordinateRequestModel model)
        {
            var destinations = GetDestinationCoordinates(model.DestinationItems);

            return string.Format(GoongMapRestApiFormat.DistanceMatrixEndpoint(destinations), AppSettingValues.GoongMapApiURL,
                                                                                             AppSettingValues.GoongDistanceMatrixEndpoint,
                                                                                             model.OriginLatitude,
                                                                                             model.OriginLongtitude,
                                                                                             model.Vehicle.ToString(),
                                                                                             AppSettingValues.GoongMapApiKey);
        }

        #endregion

        #region Get Direction Endpoint

        /// <summary>
        /// Gets the direction endpoint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string GetDirectionEndpoint(DirectionCoordinateModel model)
        {
            var destinations = GetDestinationCoordinates(model.DestinationItems);

            return string.Format(GoongMapRestApiFormat.DirectionEndpoint(destinations), AppSettingValues.GoongMapApiURL,
                                                                                        AppSettingValues.GoongDirectionEndpoint,
                                                                                        model.OriginLatitude,
                                                                                        model.OriginLongtitude,
                                                                                        model.Alternative ? "true" : "false",
                                                                                        model.Vehicle.ToString(),
                                                                                        AppSettingValues.GoongMapApiKey);
        }

        #endregion

        #region Get Destination Coordinates

        /// <summary>
        /// Gets the destination coordinates.
        /// </summary>
        /// <param name="destinationItems">The destination items.</param>
        /// <returns></returns>
        private string GetDestinationCoordinates(List<DestinationCoordinateModel> destinationItems)
        {
            var destinationCoordinates =  destinationItems.Select(x => string.Format(GoongMapRestApiFormat.DestinationCoordinate, x.DestinationLatitude, x.DestinationLongtitude));
            return destinationCoordinates.ToStringFormat(MarkConstant.SLASH);
        }

        #endregion

        #region Get AutoComplete Endpoint

        /// <summary>
        /// Gets the automatic complete endpoint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string GetAutoCompleteEndpoint(PlaceAutoCompleteRequestModel model)
        {
            return string.Format(GoongMapRestApiFormat.AutoComplete, AppSettingValues.GoongMapApiURL,
                                                                              AppSettingValues.GoongAutoCompleteEndpoint,
                                                                              AppSettingValues.GoongMapApiKey,
                                                                              model.Latitude + MarkConstant.COMMA + model.Longtitude,
                                                                              model.KeyWord,
                                                                              BooleanConstants.TRUE ? "true" : "false"
                                                                              );
        }

        #endregion

        #region Get Place Detail Endpoint

        /// <summary>
        /// Gets the place detail endpoint.
        /// </summary>
        /// <param name="placeId">The place identifier.</param>
        /// <returns></returns>
        protected string GetPlaceDetailEndpoint(string placeId)
        {
            return string.Format(GoongMapRestApiFormat.PlaceDetail, AppSettingValues.GoongMapApiURL,
                                                                    AppSettingValues.GoongPlaceDetailEndpoint,
                                                                    placeId,
                                                                    AppSettingValues.GoongMapApiKey);
        }

        #endregion

        #region Get Getcode Endpoint

        /// <summary>
        /// Gets the geocode endpoint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        protected string GetGeocodeEndpoint(GeocodeRequestModel model)
        {
            return string.Format(GoongMapRestApiFormat.Geocode, AppSettingValues.GoongMapApiURL,
                                                                AppSettingValues.GoongGeocodeEndpoint,
                                                                model.Latitude + MarkConstant.COMMA + model.Longtitude,
                                                                AppSettingValues.GoongMapApiKey
                                                                );
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        public void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }
            }
            this.Disposed = true;
        }

        #endregion
    }
}
