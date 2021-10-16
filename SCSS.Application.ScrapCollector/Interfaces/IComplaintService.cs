using SCSS.Application.ScrapCollector.Models.ComplaintModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IComplaintService
    {
        /// <summary>
        /// Creates the complaint to seller.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateComplaintToSeller(ComplaintToSellerCreateModel model);

        /// <summary>
        /// Creates the complaint to dealer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateComplaintToDealer(ComplaintToDealerCreateModel model);
    }
}
