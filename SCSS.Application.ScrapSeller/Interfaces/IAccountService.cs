using SCSS.Application.ScrapSeller.Models.AccountModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface IAccountService
    {
        /// <summary>
        /// Sends the otp to register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SendOtpToRegister(SendOTPRequestModel model);

        /// <summary>
        /// Sends the otp restore pass.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SendOtpRestorePass(SendOTPRequestModel model);



        /// <summary>
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> Register(SellerAccountRegistrationModel model);

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateDeviceId(DeviceIdUpdateModel model);

        /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateAccount(SellerAccountUpdateProfileModel model);

        /// <summary>
        /// Gets the seller account information.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetSellerAccountInfo();
    }
}
