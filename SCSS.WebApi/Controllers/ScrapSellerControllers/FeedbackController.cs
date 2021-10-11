using Microsoft.AspNetCore.Mvc;
using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.FeedbackModels;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using SCSS.WebApi.AuthenticationFilter;
using SCSS.WebApi.SystemConstants;
using System.Threading.Tasks;

namespace SCSS.WebApi.Controllers.ScrapSellerControllers
{
    [ApiVersion(ApiVersions.ApiVersionV2)]
    public class FeedbackController : BaseScrapSellerControllers
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

        #region Create Sell-Collect Transaction Feedback 

        /// <summary>
        /// Creates the sell collect transaction feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.FeedbackApiUrl.CreateTransFeedback)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateSellCollectTransactionFeedback([FromBody] SellCollecTransFeedbackCreateModel model)
        {
            return await _feedbackService.CreateSellCollectTransactionFeedback(model);
        }

        #endregion

        #region Create Sell-Collect Transaction Feedback

        /// <summary>
        /// Creates the feedback to admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        [ProducesResponseType(typeof(BaseApiResponseModel), HttpStatusCodes.Ok)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Forbidden)]
        [ProducesResponseType(typeof(ErrorResponseModel), HttpStatusCodes.Unauthorized)]
        [Route(ScrapSellerApiUrlDefinition.FeedbackApiUrl.CreateFeedbackToAdmin)]
        [ServiceFilter(typeof(ApiAuthenticateFilterAttribute))]
        public async Task<BaseApiResponseModel> CreateFeedbackToAdmin([FromBody] FeedbackAdminCreateModel model)
        {
            return await _feedbackService.CreateFeedbackToAdmin(model);
        }

        #endregion
    }
}
