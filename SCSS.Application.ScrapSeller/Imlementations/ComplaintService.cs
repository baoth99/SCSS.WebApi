using SCSS.Application.ScrapSeller.Interfaces;
using SCSS.Application.ScrapSeller.Models.ComplaintModel;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplaintService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService"></param>
        public ComplaintService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _sellerComplaintRepository = unitOfWork.SellerComplaintRepository;
            _complaintRepository = unitOfWork.ComplaintRepository;
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

            if (_sellerComplaintRepository.IsExisted(x => x.ComplaintId.Equals(complaintEntity.Id)))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataAlreadyExists);
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
