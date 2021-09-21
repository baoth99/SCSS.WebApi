using SCSS.Application.Admin.Models.TransactionServiceFeeModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.Admin.Interfaces
{
    public interface ITransactionServiceFeeService
    {
        Task<BaseApiResponseModel> CreateTransactionServiceFee(TransactionServiceFeeCreateModel model);
    }
}
