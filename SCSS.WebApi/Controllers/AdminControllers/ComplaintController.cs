using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models;
using SCSS.Application.Admin.Models.ComplaintModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class ComplaintController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The feedback service
        /// </summary>
        private readonly IComplaintService _complaintService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplaintController"/> class.
        /// </summary>
        /// <param name="feedbackService">The feedback service.</param>
        public ComplaintController(IComplaintService feedbackService)
        {
            _complaintService = feedbackService;
        }

        #endregion

        #region Search Seller Complaint

        /// <summary>
        /// Searches the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ComplaintApiUrl.GetSellerComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchSellerComplaint([FromQuery] SellerComplaintSearchModel model)
        {
            return await _complaintService.SearchSellerComplaint(model);
        }

        #endregion

        #region Search Collector Complaint

        /// <summary>
        /// Searches the collector complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ComplaintApiUrl.GetCollectorComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchCollectorComplaint([FromQuery] CollectorComplaintSearchModel model)
        {
            return await _complaintService.SearchCollectorComplaint(model);
        }

        #endregion

        #region Search Dealer Complaint

        /// <summary>
        /// Searches the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ComplaintApiUrl.GetDealerComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchDealerComplaint([FromQuery] DealerComplaintSearchModel model)
        {
            return await _complaintService.SearchDealerComplaint(model);
        }

        #endregion

        #region Reply Seller Complaint

        /// <summary>
        /// Replies the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ComplaintApiUrl.ReplySellerComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReplySellerComplaint([FromBody] ComplaintReplyModel model)
        {
            return await _complaintService.ReplySellerComplaint(model);
        }

        #endregion

        #region Reply Collector Complaint

        /// <summary>
        /// Replies the collector complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ComplaintApiUrl.ReplyCollectorComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReplyCollectorComplaint([FromBody] ComplaintReplyModel model)
        {
            return await _complaintService.ReplyCollectorComplaint(model);
        }

        #endregion

        #region Reply Dealer Complaint

        /// <summary>
        /// Replies the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.ComplaintApiUrl.ReplyDealerComplaint)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReplyDealerComplaint([FromBody] ComplaintReplyModel model)
        {
            return await _complaintService.ReplyDealerComplaint(model);
        }

        #endregion
    }
}


