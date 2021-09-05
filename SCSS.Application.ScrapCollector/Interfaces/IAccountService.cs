using SCSS.Application.ScrapCollector.Models.AccountModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IAccountService
    {
        Task<BaseApiResponseModel> UpdateAccountInformation(AccountUpdateRequestModel model);

        Task<BaseApiResponseModel> RegisterCollectorAccount(AccountRegisterRequestModel model);
    }
}
