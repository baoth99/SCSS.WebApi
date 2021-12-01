using SCSS.Application.ScrapSeller.Models.PersonalSellerLocationModel;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface IPersonalSellerLocationService
    {
        /// <summary>
        /// Adds the personal location.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> AddPersonalLocation(PersonalSellerLocationCreateModel model);

        /// <summary>
        /// Removes the location.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> RemoveLocation(Guid id);

        /// <summary>
        /// Gets the locations.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetLocations();
    }
}
