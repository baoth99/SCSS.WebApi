using SCSS.Data.EF.Repositories;
using SCSS.Data.Entities;
using System.Threading.Tasks;

namespace SCSS.Data.EF.UnitOfWork
{
    public interface IUnitOfWork
    {
        #region Repositories

        /// <summary>
        /// Gets the account repository.
        /// </summary>
        /// <value>
        /// The account repository.
        /// </value>
        IRepository<Account> AccountRepository { get; }

        /// <summary>
        /// Gets the collector coordinate.
        /// </summary>
        /// <value>
        /// The collector coordinate.
        /// </value>
        IRepository<CollectorCoordinate> CollectorCoordinateRepository { get; }

        /// <summary>
        /// Gets the dealer information repository.
        /// </summary>
        /// <value>
        /// The dealer information repository.
        /// </value>
        IRepository<DealerInformation> DealerInformationRepository { get; }

        /// <summary>
        /// Gets the collecting request repository.
        /// </summary>
        /// <value>
        /// The collecting request repository.
        /// </value>
        IRepository<CollectingRequest> CollectingRequestRepository { get; }

        /// <summary>
        /// Gets the collecting request configuration repository.
        /// </summary>
        /// <value>
        /// The collecting request configuration repository.
        /// </value>
        IRepository<CollectingRequestConfig> CollectingRequestConfigRepository { get; }

        /// <summary>
        /// Gets the collect deal transaction repository.
        /// </summary>
        /// <value>
        /// The collect deal transaction repository.
        /// </value>
        IRepository<CollectDealTransaction> CollectDealTransactionRepository { get; }

        /// <summary>
        /// Gets the collect deal transaction detail repository.
        /// </summary>
        /// <value>
        /// The collect deal transaction detail repository.
        /// </value>
        IRepository<CollectDealTransactionDetail> CollectDealTransactionDetailRepository { get; }

        /// <summary>
        /// Gets the feedback repository.
        /// </summary>
        /// <value>
        /// The feedback repository.
        /// </value>
        IRepository<Feedback> FeedbackRepository { get; }

        /// <summary>
        /// Gets the complaint repository.
        /// </summary>
        /// <value>
        /// The complaint repository.
        /// </value>
        IRepository<Complaint> ComplaintRepository { get; }

        /// <summary>
        /// Gets the seller complant repository.
        /// </summary>
        /// <value>
        /// The seller complant repository.
        /// </value>
        IRepository<SellerComplaint> SellerComplaintRepository { get; }

        /// <summary>
        /// Gets the collector complaint repository.
        /// </summary>
        /// <value>
        /// The collector complaint repository.
        /// </value>
        IRepository<CollectorComplaint> CollectorComplaintRepository { get; }

        /// <summary>
        /// Gets the dealer complaint repository.
        /// </summary>
        /// <value>
        /// The dealer complaint repository.
        /// </value>
        IRepository<DealerComplaint> DealerComplaintRepository { get; }

        /// <summary>
        /// Gets the image slider repository.
        /// </summary>
        /// <value>
        /// The image slider repository.
        /// </value>
        IRepository<ImageSlider> ImageSliderRepository { get; }

        /// <summary>
        /// Gets the location repository.
        /// </summary>
        /// <value>
        /// The location repository.
        /// </value>
        IRepository<Location> LocationRepository { get; }

        /// <summary>
        /// Gets the notification repository.
        /// </summary>
        /// <value>
        /// The notification repository.
        /// </value>
        IRepository<Notification> NotificationRepository { get; }

        /// <summary>
        /// Gets the promotion repository.
        /// </summary>
        /// <value>
        /// The promotion repository.
        /// </value>
        IRepository<Promotion> PromotionRepository { get; }

        /// <summary>
        /// Gets the role repository.
        /// </summary>
        /// <value>
        /// The role repository.
        /// </value>
        IRepository<Role> RoleRepository { get; }

        /// <summary>
        /// Gets the scrap category repository.
        /// </summary>
        /// <value>
        /// The scrap category repository.
        /// </value>
        IRepository<ScrapCategory> ScrapCategoryRepository { get; }

        /// <summary>
        /// Gets the scrap category detail repository.
        /// </summary>
        /// <value>
        /// The scrap category detail repository.
        /// </value>
        IRepository<ScrapCategoryDetail> ScrapCategoryDetailRepository { get; }

        /// <summary>
        /// Gets the sell collect transaction repository.
        /// </summary>
        /// <value>
        /// The sell collect transaction repository.
        /// </value>
        IRepository<SellCollectTransaction> SellCollectTransactionRepository { get; }

        /// <summary>
        /// Gets the sell collect transaction detail repository.
        /// </summary>
        /// <value>
        /// The sell collect transaction detail repository.
        /// </value>
        IRepository<SellCollectTransactionDetail> SellCollectTransactionDetailRepository { get; }

        /// <summary>
        /// Gets the service transaction repository.
        /// </summary>
        /// <value>
        /// The service transaction repository.
        /// </value>
        IRepository<ServiceTransaction> ServiceTransactionRepository { get; }

        /// <summary>
        /// Gets the transaction service fee percent repository.
        /// </summary>
        /// <value>
        /// The transaction service fee percent repository.
        /// </value>
        IRepository<TransactionServiceFeePercent> TransactionServiceFeePercentRepository { get;  }

        /// <summary>
        /// Gets the transaction award amount repository.
        /// </summary>
        /// <value>
        /// The transaction award amount repository.
        /// </value>
        IRepository<TransactionAwardAmount> TransactionAwardAmountRepository { get; }

        /// <summary>
        /// Gets the collecting request rejection repository.
        /// </summary>
        /// <value>
        /// The collecting request rejection repository.
        /// </value>
        IRepository<CollectingRequestRejection> CollectingRequestRejectionRepository { get; }

        /// <summary>
        /// Gets the collector cancel reason repository.
        /// </summary>
        /// <value>
        /// The collector cancel reason repository.
        /// </value>
        IRepository<CollectorCancelReason> CollectorCancelReasonRepository { get; }

        #endregion

        #region Commit Async to Database

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        /// <returns></returns>
        Task CommitAsync();

        #endregion

    }
}
