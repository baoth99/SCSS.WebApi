using SCSS.Application.ScrapDealer.Models.StatisticModels;
using SCSS.Utilities.ResponseModel;
using System.Threading.Tasks;

namespace SCSS.Application.ScrapDealer.Interfaces
{
    public interface IStatisticService
    {
        /// <summary>
        /// Gets the statistic in range time.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Task<BaseApiResponseModel> GetStatisticInRangeTime(StatisticDateFilterModel model);
    }
}
