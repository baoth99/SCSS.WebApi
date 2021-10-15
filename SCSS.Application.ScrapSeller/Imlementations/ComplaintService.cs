using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.ComplaintModel;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapSeller.Imlementations
{
    public class ComplaintService : BaseService, IComplaintService
    {
        #region Repositories

        /// <summary>
        /// The complaint repository
        /// </summary>
        private IRepository<Complaint> _complaintRepository;

        /// <summary>
        /// The seller complaint repository
        /// </summary>
        private IRepository<SellerComplaint> _sellerComplaintRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private IRepository<CollectingRequest> _collectingRequestRepository;

        #endregion

        #region Constructor

        public ComplaintService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
        }

        #endregion



        #region Create Seller Complaint

        /// <summary>
        /// Creates the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateSellerComplaint(ComplaintCreateModel model)
        {
            var complaintEntity = _complaintRepository.GetAsNoTracking(x => x.CollectingRequestId.Equals(model.CollectingRequestId) &&
                                                                            x.CollectDealTransactionId == null);
            if (complaintEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            var colletingRequest = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(model.CollectingRequestId));

            var sellerComplaintEntity = new SellerComplaint()
            {
                ComplaintId = complaintEntity.Id,
                ComplaintContent = model.ComplaintContent,
                SellerAccountId = colletingRequest.SellerAccountId,
                ComplaintedAccountId = colletingRequest.CollectorAccountId,
            };

            _sellerComplaintRepository.Insert(sellerComplaintEntity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
