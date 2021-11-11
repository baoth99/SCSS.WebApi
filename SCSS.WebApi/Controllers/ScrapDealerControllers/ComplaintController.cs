using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.ComplaintModels;
using SCSS.Utilities.Constants;
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
    public class ComplaintController : BaseScrapDealerController
    {
        #region Services

        /// <summary>
        /// The complaint service
        /// </summary>
        private readonly IComplaintService _complaintService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplaintController"/> class.
        /// </summary>
        /// <param name="complaintService">The complaint service.</param>
        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        #endregion

        #region Create COmplaint

        /// <summary>
        /// Creates the complaint to collector.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapDealerApiUrlDefinition.ComplaintApiUrl.ComplaintToCollector)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateComplaintToCollector([FromBody] ComplaintToCollectorCreateModel model)
        {
            return await _complaintService.CreateComplaintToCollector(model);
        }

        #endregion

    }
}
