using SCSS.Data.EF.Repositories;
using SCSS.Data.Entities;
using System.Threading.Tasks;

namespace SCSS.Data.EF.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories

        IRepository<Account> AccountRepository { get; }

        IRepository<DealerInformation> DealerInformationRepository { get; }

        IRepository<Booking> BookingRepository { get; }

        IRepository<CollectDealTransaction> CollectDealTransactionRepository { get; }

        IRepository<CollectDealTransactionDetail> CollectDealTransactionDetailRepository { get; }

        IRepository<Feedback> FeedbackRepository { get; }

        IRepository<ImageSlider> ImageSliderRepository { get; }

        IRepository<Location> LocationRepository { get; }

        IRepository<Notification> NotificationRepository { get; }

        IRepository<Promotion> PromotionRepository { get; }

        IRepository<Role> RoleRepository { get; }

        IRepository<SellCollectTransaction> SellCollectTransactionRepository { get; }

        IRepository<SellCollectTransactionDetail> SellCollectTransactionDetailRepository { get; }

        IRepository<ServiceTransaction> ServiceTransactionRepository { get; }

        IRepository<TransactionServiceFeePercent> TransactionServiceFeePercentRepository { get;  }

        IRepository<TransactionAwardAmount> TransactionAwardAmountRepository { get; }

        IRepository<BookingRejection> BookingRejectionRepository { get; }

        #endregion

        Task CommitAsync();
    }
}
