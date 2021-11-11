using System.Threading.Tasks;

namespace SCSS.Aplication.BackgroundService.Interfaces
{
    public interface IPromotionService
    {
        /// <summary>
        /// Scans the expired promotion.
        /// </summary>
        /// <returns></returns>
        Task ScanExpiredPromotion();

        /// <summary>
        /// Scans the future promotion.
        /// </summary>
        /// <returns></returns>
        Task ScanFuturePromotion();
    }
}
