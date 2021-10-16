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
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System.Collections.Generic;
using System.Linq;
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

        /// <summary>
        /// The collector complaint repository
        /// </summary>
        private readonly IRepository<CollectorComplaint> _collectorComplaintRepository;

        /// <summary>
        /// The dealer complaint repository
        /// </summary>
        private readonly IRepository<DealerComplaint> _dealerComplaintRepository;

        /// <summary>
        /// The seller complaint repository
        /// </summary>
        private readonly IRepository<SellerComplaint> _sellerComplaintRepository;

        /// <summary>
        /// The collecting request repository
        /// </summary>
        private readonly IRepository<CollectingRequest> _collectingRequestRepository;

        /// <summary>
        /// The collect deal transaction repository
        /// </summary>
        private readonly IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        /// <summary>
        /// The account repository
        /// </summary>
        private readonly IRepository<Account> _accountRepository;

        /// <summary>
        /// The dealer information repository
        /// </summary>
        private readonly IRepository<DealerInformation> _dealerInformationRepository;


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
            _collectorComplaintRepository = unitOfWork.CollectorComplaintRepository;
            _sellerComplaintRepository = unitOfWork.SellerComplaintRepository;
            _dealerComplaintRepository = unitOfWork.DealerComplaintRepository;
            _collectingRequestRepository = unitOfWork.CollectingRequestRepository;
            _collectDealTransactionRepository = unitOfWork.CollectDealTransactionRepository;
            _dealerInformationRepository = unitOfWork.DealerInformationRepository;
            _accountRepository = UnitOfWork.AccountRepository;
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
            var sql = AppFileHelper.ReadContent(AppSettingValues.ComplaintSQLCommands, "SellerComplaint.sql");

            model.Page = model.Page < NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize < NumberConstant.Zero ? NumberConstant.Seven : model.PageSize;
            var page = (model.Page - 1) * pageSize;


            var parameters = new DynamicParameters();
            parameters.Add("@SellerPhone", model.SellerPhone);
            parameters.Add("@SellerName", model.SellerName);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            var sqlQuery = await _dapperService.SqlQueryAsync<SellerComplaintSQLModel>(sql, parameters);

            var firstRecord = sqlQuery.FirstOrDefault();

            var totalRecord = firstRecord == null ? NumberConstant.Zero : firstRecord.TotalRecord;

            var dataResult = sqlQuery.Select(x => new SellerComplaintViewModel()
            {
                Id = x.SellerComplaintId,
                CollectingRequestCode = x.CollectingRequestCode,
                BuyingInfo = string.Format("{0}-{1}", x.BuyingAccountPhone, x.BuyingAccountName),
                SellingInfo = string.Format("{0}-{1}", x.SellingAccountPhone, x.SellingAccountName),
                ComplaintContent = x.ComplaintContent,
                RepliedContent = x.RepliedContent,
                WasReplied = !ValidatorUtil.IsBlank(x.RepliedContent),
                ComplaintTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt)
            });

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
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

            var collectorComplaint = _collectorComplaintRepository.GetAllAsNoTracking().Join(_complaintRepository.GetAllAsNoTracking(), x => x.ComplaintId, y => y.Id,
                                                                                  (x, y) => new CollectorComplaintTempModel()
                                                                                  {
                                                                                      CollectorAccountId = x.CollectorAccountId,
                                                                                      ComplaintContent = x.ComplaintContent,
                                                                                      AdminReply = x.AdminReply,
                                                                                      CreatedTime = x.CreatedTime,
                                                                                      ComplaintedAccountId = x.ComplaintedAccountId,
                                                                                      CollectorComplaintId = y.Id,
                                                                                      CollectingRequestId = y.CollectingRequestId,
                                                                                      CollectDealTransactionId = y.CollectDealTransactionId
                                                                                  }).ToList();

            var complaintToSellerTask = ComplaintToSeller(collectorComplaint, model);

            var complaintToDealerTask = ComplaintToDealer(collectorComplaint, model);


            await Task.WhenAll(complaintToSellerTask, complaintToDealerTask);

            var combinedData = complaintToSellerTask.Result.Concat(complaintToDealerTask.Result).OrderByDescending(x => x.CreatedTime);

            var totalRecord = combinedData.Count();

            var dataResult = combinedData.Skip((model.Page - 1) * model.PageSize).Take(model.PageSize).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #region Get Complaints To Seller

        /// <summary>
        /// Complaints to seller.
        /// </summary>
        /// <param name="collectorComplaint">The collector complaint.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private async Task<IEnumerable<CollectorComplainViewModel>> ComplaintToSeller(List<CollectorComplaintTempModel> collectorComplaint, CollectorComplaintSearchModel model)
        {
            return await Task.Run(() =>
            {
                var result = collectorComplaint.Join(_collectingRequestRepository.GetAllAsNoTracking(), x => x.CollectingRequestId, y => y.Id,
                                                            (x, y) => new
                                                            {
                                                                x.CollectorComplaintId,
                                                                Code = y.CollectingRequestCode,
                                                                            //
                                                                            x.ComplaintContent,
                                                                x.AdminReply,
                                                                            //
                                                                            x.CollectorAccountId,
                                                                x.ComplaintedAccountId,
                                                                            //
                                                                            ComplaintTime = x.CreatedTime
                                                            })
                                                        .Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.CollectorName) || x.Name.Contains(model.CollectorName)) &&
                                                                                                            (ValidatorUtil.IsBlank(model.CollectorPhone) || x.Phone.Contains(model.CollectorPhone))),
                                                                                                            x => x.CollectorAccountId, y => y.Id, (x, y) => new
                                                                                                            {
                                                                                                                x.ComplaintedAccountId,
                                                                                                                x.CollectorComplaintId,
                                                                                                                x.Code,
                                                                                                                x.ComplaintTime,
                                                                                                                            //
                                                                                                                            x.ComplaintContent,
                                                                                                                x.AdminReply,
                                                                                                                            //
                                                                                                                            CollectorInfo = string.Format("{0}-{1}", y.Phone, y.Name),
                                                                                                            })
                                                        .Join(_accountRepository.GetAllAsNoTracking(), x => x.ComplaintedAccountId, y => y.Id, (x, y) => new
                                                        {
                                                            x.Code,
                                                                        //
                                                                        x.CollectorComplaintId,
                                                            x.ComplaintContent,
                                                            x.AdminReply,
                                                            x.ComplaintTime,
                                                                        //
                                                                        x.CollectorInfo,
                                                            ComplaintedAccountInfo = string.Format("{0}-{1}", y.Phone, y.Name)
                                                        })
                                                                                                        .Select(x => new CollectorComplainViewModel()
                                                                                                        {
                                                                                                            Id = x.CollectorComplaintId,
                                                                                                            Code = x.Code,
                                                                                                            CollectorInfo = x.CollectorInfo,
                                                                                                            ComplaintedAccountInfo = x.ComplaintedAccountInfo,
                                                                                                            ComplaintContent = x.ComplaintContent,
                                                                                                            ComplaintTime = x.ComplaintTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                                                                                                            RepliedContent = x.AdminReply,
                                                                                                            WasReplied = !ValidatorUtil.IsBlank(x.AdminReply),
                                                                                                            CreatedTime = x.ComplaintTime
                                                                                                        });
                return result;
            });
        }

        #endregion

        #region Get Complain To Dealer

        /// <summary>
        /// Complaints to dealer.
        /// </summary>
        /// <param name="collectorComplaint">The collector complaint.</param>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        private async Task<IEnumerable<CollectorComplainViewModel>> ComplaintToDealer(List<CollectorComplaintTempModel> collectorComplaint, CollectorComplaintSearchModel model)
        {
            return await Task.Run(() =>
            {
                var complaintToDealer = collectorComplaint.Join(_collectDealTransactionRepository.GetAllAsNoTracking(), x => x.CollectDealTransactionId, y => y.Id,
                                                        (x, y) => new
                                                        {
                                                            x.CollectorComplaintId,
                                                            Code = y.TransactionCode,
                                                            //
                                                            x.ComplaintContent,
                                                            x.AdminReply,
                                                            //
                                                            x.CollectorAccountId,
                                                            x.ComplaintedAccountId,
                                                            //
                                                            ComplaintTime = x.CreatedTime
                                                        })
                                                      .Join(_accountRepository.GetManyAsNoTracking(x => (ValidatorUtil.IsBlank(model.CollectorName) || x.Name.Contains(model.CollectorName)) &&
                                                                                                        (ValidatorUtil.IsBlank(model.CollectorPhone) || x.Phone.Contains(model.CollectorPhone))),
                                                                                                        x => x.CollectorAccountId, y => y.Id, (x, y) => new
                                                                                                        {
                                                                                                            x.ComplaintedAccountId,
                                                                                                            x.CollectorComplaintId,
                                                                                                            x.Code,
                                                                                                            x.ComplaintTime,
                                                                                                            //
                                                                                                            x.ComplaintContent,
                                                                                                            x.AdminReply,
                                                                                                            //
                                                                                                            CollectorInfo = string.Format("{0}-{1}", y.Phone, y.Name),
                                                                                                        })
                                                      .Join(_dealerInformationRepository.GetAllAsNoTracking(), x => x.CollectorComplaintId, y => y.Id, (x, y) => new
                                                      {
                                                          x.Code,
                                                          //
                                                          x.CollectorComplaintId,
                                                          x.ComplaintContent,
                                                          x.AdminReply,
                                                          x.ComplaintTime,
                                                          //
                                                          x.CollectorInfo,
                                                          ComplaintedAccountInfo = string.Format("{0}-{1}", y.DealerPhone, y.DealerName)
                                                      })
                                                                                                         .Select(x => new CollectorComplainViewModel()
                                                                                                         {
                                                                                                             Id = x.CollectorComplaintId,
                                                                                                             Code = x.Code,
                                                                                                             CollectorInfo = x.CollectorInfo,
                                                                                                             ComplaintedAccountInfo = x.ComplaintedAccountInfo,
                                                                                                             ComplaintContent = x.ComplaintContent,
                                                                                                             ComplaintTime = x.ComplaintTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                                                                                                             RepliedContent = x.AdminReply,
                                                                                                             WasReplied = !ValidatorUtil.IsBlank(x.AdminReply),
                                                                                                             CreatedTime = x.ComplaintTime
                                                                                                         });
                return complaintToDealer;
            });
        }

        #endregion


        #endregion

        #region Search Dealer Complaint

        /// <summary>
        /// Searches the dealer complaint.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> SearchDealerComplaint(DealerComplaintSearchModel model)
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.ComplaintSQLCommands, "DealerComplaint.sql");

            model.Page = model.Page < NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize < NumberConstant.Zero ? NumberConstant.Seven : model.PageSize;
            var page = (model.Page - 1) * pageSize;

            var parameters = new DynamicParameters();
            parameters.Add("@DealerPhone", model.DealerPhone);
            parameters.Add("@DealerName", model.DealerName);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            var sqlQuery = await _dapperService.SqlQueryAsync<DealerComplaintSQLModel>(sql, parameters);

            var firstRecord = sqlQuery.FirstOrDefault();

            var totalRecord = firstRecord == null ? NumberConstant.Zero : firstRecord.TotalRecord;

            var dataResult = sqlQuery.Select(x => new DealerComplaintViewModel()
            {
                Id = x.DealerComplaintId,
                TransactionCode = x.CollectDealTransactionCode,
                ComplaintContent = x.ComplaintContent,
                SellingAccountInfo = string.Format("{0}-{1}", x.SellingAccountPhone, x.SellingAccountName),
                DealerInfo = string.Format("{0}-{1}", x.DealerPhone, x.DealerName),
                BuyingAccountName = x.BuyingAccountName,
                ComplaintTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt),
                RepliedContent = x.RepliedContent,
                WasReplied = !ValidatorUtil.IsBlank(x.RepliedContent),
            }).ToList();


            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

    }
}
