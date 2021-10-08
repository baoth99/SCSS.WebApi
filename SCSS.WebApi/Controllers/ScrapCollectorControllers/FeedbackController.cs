using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.FeedbackModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapCollectorControllers
{
    [ApiVersion(ApiVersions.ApiVersionV4)]
    public class FeedbackController : BaseScrapCollectorController
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

        #region Create Dealer Feedback

        /// <summary>
        /// Creates the dealer feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.FeedbackApiUrl.CreateTransFeedback)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateDealerFeedback([FromBody] FeedbackTransactionCreateModel model)
        {
            return await _feedbackService.CreateDealerFeedback(model);
        }

        #endregion Create Dealer Feedback

        #region Create Feedback To Admin

        /// <summary>
        /// Creates the feedback to admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapCollectorApiUrlDefinition.FeedbackApiUrl.CreateFeedbackToAdmin)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateFeedbackToAdmin([FromBody] FeedbackAdminCreateModel model)
        {
            return await _feedbackService.CreateFeedbackToAdmin(model);
        }

        #endregion Create Feedback To Admin
    }
}
