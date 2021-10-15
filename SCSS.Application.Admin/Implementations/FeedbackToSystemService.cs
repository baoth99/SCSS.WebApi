using Dapper;
using SCSS.Application.Admin.Models;
using SCSS.Application.Admin.Models.FeedbackModels;
using SCSS.Utilities.BaseResponse;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using SCSS.Utilities.Extensions;
using SCSS.Utilities.Helper;
using SCSS.Utilities.ResponseModel;
using System.Linq;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Implementations
{
    public partial class FeedbackService
    {
        #region Get Seller Feedback To System

        /// <summary>
        /// Gets the seller feedback to system.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> GetSellerFeedbackToSystem(SystemFeedbackSellerRequestModel model)
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.FeedbackSQLCommand, "SellerFeedbackToSystem.sql");

            model.Page = model.Page < NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize < NumberConstant.Zero ? NumberConstant.Seven : model.PageSize;
            var page = (model.Page - 1) * pageSize;
            var parameters = new DynamicParameters();

            parameters.Add("@TransactionCode", model.TransactionCode);
            parameters.Add("@SellerName", model.SellerName);
            parameters.Add("@CollectorName", model.CollectorName);
            parameters.Add("@CollectorPhone", model.CollectorPhone);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);

            var dataQuery = await _dapperService.SqlQueryAsync<SellerFeedbackToSystemSqlModel>(sql, parameters);

            var firstRecord = dataQuery.FirstOrDefault();

            var totalRecord = firstRecord == null ? NumberConstant.Zero : firstRecord.TotalRecord;

            var dataResult = dataQuery.Select(x => new SellerFeedbackToSystemViewModel()
            {
                Id = x.FeedbackId,
                CollectingRequestCode = x.CollectingRequestCode,
                FeedbackContent = x.FeedbackContent,
                SellingInfo = string.Format("{0}-{1}", x.SellingAccountPhone, x.SellingAccountName),
                BuyingInfo = string.Format("{0}-{1}", x.BuyingAccountPhone, x.BuyingAccountName),
                RepliedContent = StringUtils.GetString(x.RepliedContent),
                WasReplied = !ValidatorUtil.IsBlank(x.RepliedContent),
                FeedbackTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt)
            }).ToList();

            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion

        #region Get Collector Feedback ToSystem

        public async Task<BaseApiResponseModel> GetCollectorFeedbackToSystem(SystemFeedbackCollectorRequestModel model)
        {
            var sql = AppFileHelper.ReadContent(AppSettingValues.FeedbackSQLCommand, "CollectorFeedbackToSystem.sql");

            model.Page = model.Page < NumberConstant.Zero ? NumberConstant.One : model.Page;
            var pageSize = model.PageSize < NumberConstant.Zero ? NumberConstant.Seven : model.PageSize;
            var page = (model.Page - 1) * pageSize;
            var parameters = new DynamicParameters();

            parameters.Add("@TransactionCode", model.TransactionCode);
            parameters.Add("@DealerName", model.DealerName);
            parameters.Add("@DealerPhone", model.DealerPhone);
            parameters.Add("@CollectorName", model.CollectorName);
            parameters.Add("@Page", page);
            parameters.Add("@PageSize", pageSize);


            var dataQuery = await _dapperService.SqlQueryAsync<CollectorFeedbackToSystemSqlModel>(sql, parameters);

            var firstRecord = dataQuery.FirstOrDefault();

            var totalRecord = firstRecord == null ? NumberConstant.Zero : firstRecord.TotalRecord;

            var dataResult = dataQuery.Select(x => new CollectorFeedbackToSystemViewModel()
            {
                Id = x.FeedbackId,
                TransactionCode = x.TransactionCode,
                FeedbackContent = x.FeedbackContent,
                SellingAccountInfo = string.Format("{0}-{1}", x.SellingAccountPhone, x.SellingAccountName),
                BuyingAccountName = x.BuyingAccountName,
                DealerInfo = string.Format("{0}-{1}", x.DealerPhone, x.DealerName),
                RepliedContent = StringUtils.GetString(x.RepliedContent),
                WasReplied = !ValidatorUtil.IsBlank(x.RepliedContent),
                FeedbackTime = x.CreatedTime.ToStringFormat(DateTimeFormat.DD_MM_yyyy_time_tt)
            }).ToList();


            return BaseApiResponse.OK(totalRecord: totalRecord, resData: dataResult);
        }

        #endregion


        #region Admin Replies Feedback

        /// <summary>
        /// Replies the feedback.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public async Task<BaseApiResponseModel> ReplyFeedback(FeedbackReplyModel model)
        {
            var sysFeedbackEntity = _feedbackToSystemRepository.GetById(model.FeedbackId);

            if (sysFeedbackEntity == null)
            {
                return BaseApiResponse.NotFound();
            }

            sysFeedbackEntity.AdminReply = model.RepliedContent;

            _feedbackToSystemRepository.Update(sysFeedbackEntity);

            await UnitOfWork.CommitAsync();

            return BaseApiResponse.OK();
        }

        #endregion
    }
}
