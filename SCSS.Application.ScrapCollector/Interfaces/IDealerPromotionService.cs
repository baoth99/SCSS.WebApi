using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IDealerPromotionService
    {
        /// <summary>
        /// Gets the dealer promotion.
        /// </summary>
        /// <param name="dealerId">The dealer identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerPromotions(Guid dealerId);

        /// <summary>
        /// Gets the dealer promotion detail.
        /// </summary>
        /// <param name="promotionId">The promotion identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetDealerPromotionDetail(Guid promotionId);
    }
}
