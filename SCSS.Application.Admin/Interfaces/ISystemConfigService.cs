using SCSS.Application.Admin.Models.SystemConfigModels;
using SCSS.Application.Admin.Models.TransactionAwardAmountModels;
using SCSS.Application.Admin.Models.TransactionServiceFeeModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ISystemConfigService
    {
        /// <summary>
        /// Settings the system configuration.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ModifySystemConfig(SystemConfigModifyModel model);

        /// <summary>
        /// Gets the system configuration information.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetSystemConfigInfo();

        /// <summary>
        /// Creates the transaction award amount.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ModifyTransactionAwardAmount(TransactionAwardAmountModifyModel model);

        /// <summary>
        /// Gets the transaction award amount.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionAwardAmount();

        /// <summary>
        /// Creates the transaction service fee.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> ModifyTransactionServiceFee(TransactionServiceFeeModifyModel model);

        /// <summary>
        /// Gets the transaction service fee.
        /// </summary>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetTransactionServiceFee();
    }
}
