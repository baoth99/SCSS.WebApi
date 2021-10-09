using SCSS.Application.Admin.Models.CollectorCancelReasonModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ICollectorCancelReasonService
    {
        /// <summary>
        /// Creates the new cancel reason.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateNewCancelReason(CollectorCancelReasonCreateModel model);

        /// <summary>
        /// Updates the cancel reason.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> UpdateCancelReason(CollectorCancelReasonUpdateModel model);

        /// <summary>
        /// Deletes the cancel reason.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> DeleteCancelReason(Guid id);

        /// <summary>
        /// Gets the cancel reasons.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetCancelReasons();
    }
}
