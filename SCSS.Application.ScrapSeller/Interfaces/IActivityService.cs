using SCSS.Application.ScrapSeller.Models.ActivityModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface IActivityService
    {
        Task<BaseApiResponseModel> GetActivities(ActivityFilterModel model);

        Task<BaseApiResponseModel> GetActivityDetail(Guid id);
    }
}
