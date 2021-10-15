using SCSS.Application.ScrapSeller.Models.ComplaintModel;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Interfaces
{
    public interface IComplaintService
    {
        /// <summary>
        /// Creates the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> CreateSellerComplaint(ComplaintCreateModel model);
    }
}
