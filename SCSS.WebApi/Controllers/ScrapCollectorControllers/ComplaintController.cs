using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.ComplaintModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class ComplaintController : BaseScrapCollectorController
    {
        #region Services

        /// <summary>
        /// The complaint service
        /// </summary>
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

        #region Create Complaint About Collecting Request

        /// <summary>
        /// Creates the cr complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ComplaintApiUrl.CollectingRequest)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateCRComplaint([FromBody] ComplaintToSellerCreateModel model)
        {
            return await _complaintService.CreateComplaintToSeller(model);
        }

        #endregion

        #region Create Complaint About CollectDeal Transaction

        /// <summary>
        /// Creates the collect deal complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.ComplaintApiUrl.CollectDealTrans)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateCollectDealComplaint([FromBody] ComplaintToDealerCreateModel model)
        {
            return await _complaintService.CreateComplaintToDealer(model);
        }

        #endregion

    }
}
