using SCSS.Application.ScrapCollector.Models.ScrapCategoryModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IScrapCategoryService
    {
        /// <summary>
        /// Creates the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateScrapCategory(ScrapCategoryCreateModel model);

        /// <summary>
        /// Updates the scrap category.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateScrapCategory(ScrapCategoryUpdateModel model);

        /// <summary>
        /// Gets the scrap categories.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetScrapCategories();

        /// <summary>
        /// Gets the scrap category detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetScrapCategoryDetail(Guid id);

        /// <summary>
        /// Removes the scrap category.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RemoveScrapCategory(Guid id);
    }
}
