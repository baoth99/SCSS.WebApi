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
        /// <summary>
        /// Searches the category admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> SearchCategoryAdmin(SearchCategoryAdminModel model);

        /// <summary>
        /// Creates the category admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateCategoryAdmin(CreateCategoryAdminModel model);

        /// <summary>
        /// Gets the category admin detail.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCategoryAdminDetail(Guid id);

        /// <summary>
        /// Removes the category admin.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RemoveCategoryAdmin(Guid id);

        /// <summary>
        /// Edits the category admin.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> EditCategoryAdmin(CategoryAdminEditModel model);

        /// <summary>
        /// Gets the unit list.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetUnitList();
    }
}
