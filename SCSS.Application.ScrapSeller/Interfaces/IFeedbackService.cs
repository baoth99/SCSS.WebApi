using SCSS.Application.ScrapSeller.Models.FeedbackModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface IFeedbackService
    {
        /// <summary>
        /// Creates the transaction feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateSellCollectTransactionFeedback(SellCollecTransFeedbackCreateModel model);

        /// <summary>
        /// Creates the feedback to admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateFeedbackToAdmin(FeedbackAdminCreateModel model);
    }
}
