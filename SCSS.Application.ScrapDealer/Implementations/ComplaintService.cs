using SCSS.Application.ScrapDealer.Interfaces;
using SCSS.Application.ScrapDealer.Models.ComplaintModels;
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

namespace SCSS.Application.ScrapDealer.Implementations
{
    public class ComplaintService : BaseService, IComplaintService
    {
        #region Repositories

        /// <summary>
        /// The complaint repository
        /// </summary>
        private readonly IRepository<Complaint> _complaintRepository;

        /// <summary>
        /// The dealer complaint repository
        /// </summary>
        private readonly IRepository<DealerComplaint> _dealerComplaintRepository;

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        #endregion


        public ComplaintService(IUnitOfWork unitOfWork, IAuthSession userAuthSession, ILoggerService logger, IStringCacheService cacheService) : base(unitOfWork, userAuthSession, logger, cacheService)
        {
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _complaintRepository = unitOfWork.ComplaintRepository;
            _dealerComplaintRepository = unitOfWork.DealerComplaintRepository;
        }

        #region Create Complaint To Collector

        /// <summary>
        /// Creates the complaint to collector.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> CreateComplaintToCollector(ComplaintToCollectorCreateModel model)
        {
            var complaint = _complaintRepository.GetAsNoTracking(x => x.CollectDealTransactionId.Equals(model.CollectDealTransactionId) && x.CollectingRequestId == null);

            if (complaint == null)
            {
                return BaseApiResponse.NotFound();
            }

            var collectDealTrans = _collectDealTransactionRepository.GetById(model.CollectDealTransactionId);

            var dealerComplaint = new DealerComplaint()
            {
                DealerAccountId = collectDealTrans.DealerAccountId,
                ComplaintContent = model.ComplaintContent,
                ComplaintedAccountId = collectDealTrans.CollectorAccountId,
                ComplaintId = complaint.Id
            };

            _dealerComplaintRepository.Insert(dealerComplaint);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion

    }
}
