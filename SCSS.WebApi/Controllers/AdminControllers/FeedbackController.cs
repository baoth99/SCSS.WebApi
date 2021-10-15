using Microsoft.AspNetCore.Mvc;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models;
using SCSS.Application.Admin.Models.FeedbackModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.AdminControllers
{
    [ApiVersion(ApiVersions.ApiVersionV1)]
    public class FeedbackController : BaseAdminController
    {
        #region Services

        /// <summary>
        /// The feedback service
        /// </summary>
        private readonly IFeedbackService _feedbackService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="FeedbackController"/> class.
        /// </summary>
        /// <param name="feedbackService">The feedback service.</param>
        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        #endregion

        #region Search Sell Collect Transaction Feedback

        /// <summary>
        /// Searches the sell collect transaction feedbacks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.FeedbackApiUrl.SearchSellCollectTransactionFeedbacks)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchSellCollectTransactionFeedbacks([FromQuery] SellCollectTransactionFeedbackSearchModel model)
        {
            return await _feedbackService.SearchSellCollectTransactionFeedbacks(model);
        }


        #endregion

        #region Search Sell Collect Transaction Feedback

        /// <summary>
        /// Searches the collect deal transaction feedbacks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.FeedbackApiUrl.SearchCollectDealTransactionFeedbacks)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> SearchCollectDealTransactionFeedbacks([FromQuery] CollectDealTransactionFeedbackSearchModel model)
        {
            return await _feedbackService.SearchCollectDealTransactionFeedbacks(model);
        }

        #endregion

        #region Get Seller Feedback To System

        /// <summary>
        /// Gets the seller feedback to system.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.FeedbackApiUrl.GetSellerFeedbackToSystem)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetSellerFeedbackToSystem([FromQuery] SystemFeedbackSellerRequestModel model)
        {
            return await _feedbackService.GetSellerFeedbackToSystem(model);
        }

        #endregion

        #region Get Collector Feedback To System

        /// <summary>
        /// Gets the collector feedback to system.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.FeedbackApiUrl.GetCollectorFeedbackToSystem)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> GetCollectorFeedbackToSystem([FromQuery] SystemFeedbackCollectorRequestModel model)
        {
            return await _feedbackService.GetCollectorFeedbackToSystem(model);
        }

        #endregion

        #region Reply Feedback        

        /// <summary>
        /// Replies the feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(AdminApiUrlDefinition.FeedbackApiUrl.AdminRepliesFeedback)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> ReplyFeedback([FromBody] FeedbackReplyModel model)
        {
            return await _feedbackService.ReplyFeedback(model);
        }

        #endregion
    }
}


