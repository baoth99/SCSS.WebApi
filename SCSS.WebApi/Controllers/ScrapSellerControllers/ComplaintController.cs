using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.ComplaintModel;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class ComplaintController : BaseScrapSellerControllers
    {
        #region Services

        private readonly IComplaintService _complaintService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplaintController"/> class.
        /// </summary>
        /// <param name="complaintService">The complaint service.</param>
        public ComplaintController(IComplaintService complaintService)
        {
            _complaintService = complaintService;
        }

        #endregion

        #region Create Seller Complaint

        /// <summary>
        /// Creates the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.ComplaintApiUrl.CreateSellerComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateSellerComplaint([FromBody] ComplaintCreateModel model )
        {
            return await _complaintService.CreateSellerComplaint(model);
        }

        #endregion
    }
}
