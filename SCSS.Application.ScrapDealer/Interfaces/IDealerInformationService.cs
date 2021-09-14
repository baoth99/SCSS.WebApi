using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface IDealerInformationService
    {
        Task<BaseApiResponseModel> GetDealerInformation(Guid id);
    }
}
