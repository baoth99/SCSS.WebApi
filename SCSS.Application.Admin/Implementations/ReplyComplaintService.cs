using SCSS.Application.Admin.Models.ComplaintModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Helper;
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
            var collectorComplant = _collectorComplaintRepository.GetById(model.Id);

            if (collectorComplant == null)
            {
                return BaseApiResponse.NotFound();
            }

            if (!ValidatorUtil.IsBlank(collectorComplant.AdminReply))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataAlreadyExists);
            }

            collectorComplant.AdminReply = model.ReplyContent;

            _collectorComplaintRepository.Update(collectorComplant);

            await UnitOfWork.CommitAsync();

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
            var dealerComplant = _dealerComplaintRepository.GetById(model.Id);

            if (dealerComplant == null)
            {
                return BaseApiResponse.NotFound();
            }

            if (!ValidatorUtil.IsBlank(dealerComplant.AdminReply))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataAlreadyExists);
            }

            dealerComplant.AdminReply = model.ReplyContent;

            _dealerComplaintRepository.Update(dealerComplant);

            await UnitOfWork.CommitAsync();

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

            var sellerComplant = _sellerComplaintRepository.GetById(model.Id);

            if (sellerComplant == null)
            {
                return BaseApiResponse.NotFound();
            }

            if (!ValidatorUtil.IsBlank(sellerComplant.AdminReply))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataAlreadyExists);
            }

            sellerComplant.AdminReply = model.ReplyContent;

            _sellerComplaintRepository.Update(sellerComplant);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
