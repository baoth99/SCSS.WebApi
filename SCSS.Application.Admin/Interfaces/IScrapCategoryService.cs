using SCSS.Application.Admin.Models.ScrapCategoryModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface IScrapCategoryService
    {
        /// <summary>
        /// Searches the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchScrapCategory(ScrapCategorySearchModel model);

        /// <summary>
        /// Gets the scrap category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetScrapCategory(Guid id);
    }
}
