using SCSS.Application.Admin.Models;
using SCSS.Application.Admin.Models.ComplaintModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IComplaintService
    {
        /// <summary>
        /// Searches the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchSellerComplaint(SellerComplaintSearchModel model);

        /// <summary>
        /// Searches the collector complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchCollectorComplaint(CollectorComplaintSearchModel model);

        /// <summary>
        /// Searches the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchDealerComplaint(DealerComplaintSearchModel model);


        /// <summary>
        /// Replies the complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ReplySellerComplaint(ComplaintReplyModel model);

        /// <summary>
        /// Replies the collector complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ReplyCollectorComplaint(ComplaintReplyModel model);

        /// <summary>
        /// Replies the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ReplyDealerComplaint(ComplaintReplyModel model);

    }
}
