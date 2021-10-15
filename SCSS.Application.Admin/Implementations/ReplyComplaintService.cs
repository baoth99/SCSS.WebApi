using SCSS.Application.Admin.Models.ComplaintModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public partial class ComplaintService
    {
        #region Reply Collector Complaint

        /// <summary>
        /// Replies the collector complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReplyCollectorComplaint(ComplaintReplyModel model)
        {
            return BaseApiResponse.OK();
        }

        #endregion

        #region Reply Dealer Complaint

        /// <summary>
        /// Replies the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReplyDealerComplaint(ComplaintReplyModel model)
        {
            return BaseApiResponse.OK();
        }

        #endregion

        #region Reply Seller Complaint

        /// <summary>
        /// Replies the complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReplySellerComplaint(ComplaintReplyModel model)
        {
            return BaseApiResponse.OK();
        }

        #endregion

    }
}
