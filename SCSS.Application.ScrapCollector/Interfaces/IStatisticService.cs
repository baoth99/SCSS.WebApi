using SCSS.Application.ScrapCollector.Models.StatisticModels;
using SCSS.Utilities.ResponseModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapCollector.Interfaces
{
    public interface IStatisticService
    {
        /// <summary>
        /// Gets the statistic in time range.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetStatisticInTimeRange(StatisticDateFilterModel model);

    }
}
