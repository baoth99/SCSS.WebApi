using SCSS.Application.ScrapCollector.Models.AccountModels;
using SCSS.Utilities.ResponseModel;
using System.IO;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Updates the account information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateAccountInformation(CollectorAccountUpdateRequestModel model);

        /// <summary>
        /// Registers the collector account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RegisterCollectorAccount(CollectorAccountRegisterRequestModel model);

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateDeviceId(DeviceIdUpdateModel model);

        /// <summary>
        /// Gets the collector account information.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectorAccountInfo();

        /// <summary>
        /// Gets the qr code.
        /// </summary>
        /// <returns></returns>
        Task<MemoryStream> GetQRCode();

        /// <summary>
        /// Updates the coordinate.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateCoordinate(CollectorCoordinateUpdateModel model);
    }
}
