using SCSS.Application.ScrapDealer.Models.AccountModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Registers the dealer account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RegisterDealerAccount(DealerAccountRegisterRequestModel model);

        /// <summary>
        /// Updates the dealer account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateDealerAccount(DealerAccountUpdateRequestModel model);

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateDeviceId(string deviceId);
    }
}
