using Microsoft.AspNetCore.Mvc;
using SCSS.AWSService.Interfaces;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapDealerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV3)]
    public class DataController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The storage BLOB s3 service
        /// </summary>
        private readonly IStorageBlobS3Service _storageBlobS3Service;

        #endregion

        #region Constructor

        public DataController(IStorageBlobS3Service storageBlobS3Service)
        {
            _storageBlobS3Service = storageBlobS3Service;
        }


        #endregion

        #region Get Image

        /// <summary>
        /// Gets the image.
        /// </summary>
        /// <param name="imageUrl">The image URL.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.DataApiUrl.GetImage)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetImage([FromQuery] string imageUrl)
        {
            var file = await _storageBlobS3Service.GetFile(imageUrl);
            if (file == null)
            {
                return BaseApiResponse.NotFound(SystemMessageCode.DataNotFound);
            }
            var image = file.Stream.ToBitmap();
            return BaseApiResponse.OK(image);
        }

        #endregion
    }
}
