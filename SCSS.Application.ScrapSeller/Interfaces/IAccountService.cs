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
        Task<BaseApiResponseModel> Register(AccountRegistrationModel model);

        Task<BaseApiResponseModel> UpdateDeviceId(AccountUpdateDeviceIdModel model);

        Task<BaseApiResponseModel> UpdateAccount(AccountUpdateProfileModel model);
    }
}
