using SCSS.Application.ScrapDealer.Models.AccountModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface IAccountService
    {
        Task<BaseApiResponseModel> RegisterDealerAccount(DealerAccountRegisterRequestModel model);
    }
}
