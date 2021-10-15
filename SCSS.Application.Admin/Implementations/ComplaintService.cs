using Dapper;
using SCSS.Application.Admin.Interfaces;
using SCSS.Application.Admin.Models.ComplaintModels;
using SCSS.AWSService.Interfaces;
using SCSS.Data.EF.Repositories;
using SCSS.Data.EF.UnitOfWork;
using SCSS.Data.Entities;
using SCSS.ORM.Dapper.Interfaces;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public partial class ComplaintService : BaseService, IComplaintService
    {
        #region Repositories

        /// <summary>
        /// The feedback to system repository
        /// </summary>
        private readonly IRepository<Complaint> _complaintRepository;

        #endregion

        #region Services

        /// <summary>
        /// The dapper service
        /// </summary>
        private readonly IDapperService _dapperService;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="ComplaintService"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        /// <param name="userAuthSession">The user authentication session.</param>
        /// <param name="logger">The logger.</param>
        public ComplaintService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IDapperService dapperService) : base(unitOfWork, userAuthSession, logger)
        {
            _complaintRepository = unitOfWork.ComplaintRepository;
            _dapperService = dapperService;
        }

        #endregion

        #region Search Seller Complaint

        /// <summary>
        /// Searches the seller complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchSellerComplaint(SellerComplaintSearchModel model)
        {
            return BaseApiResponse.OK();
        }

        #endregion

        #region Search Collector Complaint

        /// <summary>
        /// Searches the collector complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchCollectorComplaint(CollectorComplaintSearchModel model)
        {
            return BaseApiResponse.OK();
        }

        #endregion

        #region Search Dealer Complaint

        /// <summary>
        /// Searches the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchDealerComplaint(DealerComplaintSearchModel model)
        {
            return BaseApiResponse.OK();
        }

        #endregion

    }
}
