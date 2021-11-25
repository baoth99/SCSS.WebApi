using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface IServiceTransactionService
    {
        /// <summary>
        /// Summaries the sevice fee.
        /// </summary>
        /// <returns></returns>
        Task SummarySeviceFee();
    }
}
