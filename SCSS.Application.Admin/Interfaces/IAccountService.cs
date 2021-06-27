using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IAccountService
    {
        Task<BaseApiResponseModel> GetAccountDetail(Guid Id);

        Task<BaseApiResponseModel> ChangeStatus(Guid Id, int? Status);

    }
}
