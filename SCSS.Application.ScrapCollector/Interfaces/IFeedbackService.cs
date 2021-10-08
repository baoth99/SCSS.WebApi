using SCSS.Application.ScrapCollector.Models.FeedbackModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IFeedbackService
    {
        /// <summary>
        /// Creates the dealer feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateDealerFeedback(FeedbackTransactionCreateModel model);

        /// <summary>
        /// Creates the feedback to admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateFeedbackToAdmin(FeedbackAdminCreateModel model);
    }
}
