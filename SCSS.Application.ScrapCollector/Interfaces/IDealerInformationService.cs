using SCSS.Application.ScrapCollector.Models.DealerInformationModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IDealerInformationService
    {
        Task<BaseApiResponseModel> SearchDealerInfo(DealerInformationFilterModel model);

        Task<BaseApiResponseModel> GetDealerInformationDetail(Guid id);
    }
}
