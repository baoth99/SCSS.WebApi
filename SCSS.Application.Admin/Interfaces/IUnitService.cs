using SCSS.Application.Admin.Models.UnitModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IUnitService
    {
        Task<BaseApiResponseModel> SearchUnit(SearchUnitModel model);

        Task<BaseApiResponseModel> CreateUnit(CreateUnitModel model);

        Task<BaseApiResponseModel> RemoveUnit(Guid id);

        Task<BaseApiResponseModel> UpdateUnit(EditUnitModel model);

    }
}
