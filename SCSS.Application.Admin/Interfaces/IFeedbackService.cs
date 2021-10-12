using SCSS.Application.Admin.Models;
using SCSS.Application.Admin.Models.FeedbackModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IFeedbackService
    {
        /// <summary>
        /// Searches the sell collect transaction feedbacks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchSellCollectTransactionFeedbacks(SellCollectTransactionFeedbackSearchModel model);

        /// <summary>
        /// Searches the collect deal transaction feedbacks.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchCollectDealTransactionFeedbacks(CollectDealTransactionFeedbackSearchModel model);

        /// <summary>
        /// Gets the seller feedback to system.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetSellerFeedbackToSystem(BaseFilterModel model);

        /// <summary>
        /// Gets the collector feedback to system.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectorFeedbackToSystem(BaseFilterModel model);

        /// <summary>
        /// Replies the feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ReplyFeedback(FeedbackReplyModel model);

    }
}
