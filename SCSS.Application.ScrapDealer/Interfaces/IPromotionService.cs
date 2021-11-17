using SCSS.Application.ScrapDealer.Models.PromotionModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface IPromotionService
    {
        /// <summary>
        /// Creates the new promotion.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateNewPromotion(PromotionCreateModel model);

        /// <summary>
        /// Gets the promotions.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetPromotions();

        /// <summary>
        /// Gets the promotion detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetPromotionDetail(Guid id);

        /// <summary>
        /// Finishes the promotion.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> FinishPromotion(Guid id);
    }
}
