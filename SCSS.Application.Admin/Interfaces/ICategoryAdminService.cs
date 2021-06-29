using SCSS.Application.Admin.Models.AdminCategoryModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ICategoryAdminService
    {
        Task<BaseApiResponseModel> SearchCategoryAdmin(SearchCategoryAdminModel model);

        Task<BaseApiResponseModel> CreateCategoryAdmin(CreateCategoryAdminModel model);

        Task<BaseApiResponseModel> GetCategoryAdminDetail(Guid id);

        Task<BaseApiResponseModel> GetUnitList();
    }
}
