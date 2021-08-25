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
        /// Registers the specified model.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> Register(AccountRegistrationModel model);

        /// <summary>
        /// Updates the device identifier.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateDeviceId(AccountUpdateDeviceIdModel model);

        /// <summary>
        /// Updates the account.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateAccount(AccountUpdateProfileModel model);
    }
}
