using SCSS.Application.Admin.Models.AccountModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IAccountService
    {
        Task<BaseApiResponseModel> Search(SearchAccountRequestModel model);

        Task<AccountDetailViewModel> GetAccountDetail(Guid Id);

        Task<BaseApiResponseModel> ChangeStatus(AccountStatusRequestModel model);

        Task<BaseApiResponseModel> GetRoleList();
    }
}
