using SCSS.Application.ScrapCollector.Interfaces;
using SCSS.Application.ScrapCollector.Models.ComplaintModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Constants;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Implementations
{
    public class ComplaintService : BaseService, IComplaintService
    {
        #region Repositories

        /// <summary>
        /// The complaint repository
        /// </summary>
        private readonly IRepository<Complaint> _complaintRepository;

        /// <summary>
        /// The collector complaint repository
        /// </summary>
        private readonly IRepository<CollectorComplaint> _collectorComplaintRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplaintService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="cacheService">The cache service.</param>
        public ComplaintService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _collectorComplaintRepository = unitOfWork.CollectorComplaintRepository;
            _complaintRepository = unitOfWork.ComplaintRepository;
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
        }

        #endregion



        #region Create Complaint To Dealer

        /// <summary>
        /// Creates the complaint to dealer.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateComplaintToDealer(ComplaintToDealerCreateModel model)
        {
            var complaint = _complaintRepository.GetAsNoTracking(x => x.CollectDealTransactionId.Equals(model.CollectDealTransactionId) && x.CollectingRequestId == null);

            if (complaint == null)
            {
                return BaseApiResponse.NotFound();
            }

            if (_collectorComplaintRepository.IsExisted(x => x.ComplaintId.Equals(complaint.Id)))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataAlreadyExists);
            }

            var collectDealTrans = _collectDealTransactionRepository.GetById(model.CollectDealTransactionId);

            var collectorComplaint = new CollectorComplaint()
            {
                CollectorAccountId = collectDealTrans.CollectorAccountId,
                ComplaintedAccountId = collectDealTrans.DealerAccountId,
                ComplaintContent = model.ComplaintContent,
                ComplaintId = complaint.Id
            };

            _collectorComplaintRepository.Insert(collectorComplaint);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion


        #region Create Complaint To Seller

        /// <summary>
        /// Creates the complaint to seller.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateComplaintToSeller(ComplaintToSellerCreateModel model)
        {
            var complaint = _complaintRepository.GetAsNoTracking(x => x.CollectingRequestId.Equals(model.CollectingRequestId) && x.CollectDealTransactionId == null);

            if (complaint == null)
            {
                return BaseApiResponse.NotFound();
            }

            if (_collectorComplaintRepository.IsExisted(x => x.ComplaintId.Equals(complaint.Id)))
            {
                return BaseApiResponse.Error(SystemMessageCode.DataAlreadyExists);
            }

            var collectingRequest = _collectingRequestRepository.GetAsNoTracking(x => x.Id.Equals(model.CollectingRequestId));

            var collectorComplaint = new CollectorComplaint()
            {
                CollectorAccountId = collectingRequest.CollectorAccountId,
                ComplaintedAccountId = collectingRequest.SellerAccountId,
                ComplaintContent = model.ComplaintContent,
                ComplaintId = complaint.Id
            };

            _collectorComplaintRepository.Insert(collectorComplaint);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
