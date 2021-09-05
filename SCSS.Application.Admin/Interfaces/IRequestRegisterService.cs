using SCSS.Application.Admin.Models.RequestRegisterModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IRequestRegisterService
    {
        /// <summary>
        /// Searches the collector request register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchCollectorRequestRegister(CollectorRequestRegisterSearchModel model);

        /// <summary>
        /// Searches the dealer request register.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchDealerRequestRegister(DealerRequestRegisterSearchModel model);

        /// <summary>
        /// Gets the collector request register.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCollectorRequestRegister(Guid id);

        /// <summary>
        /// Gets the dealer request register.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerRequestRegister(Guid id);
    }
}
