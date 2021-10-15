using Microsoft.EntityFrameworkCore;
using SCSS.Data.EF.Repositories;
using SCSS.Data.Entities;
using SCSS.Utilities.Configurations;
using System;
using System.Threading.Tasks;

namespace SCSS.Data.EF.UnitOfWork
{
    public class UnitOfWork : IDisposable, IUnitOfWork
    {
        #region AppDbContext

        /// <summary>
        /// The application database context
        /// </summary>
        private readonly AppDbContext AppDbContext;

        #endregion

        #region Disposed

        /// <summary>
        /// The disposed
        /// </summary>
        private bool Disposed = false;

        #endregion

        #region Private variable Repositories

        private IRepository<Account> _accountRepository;

        private IRepository<DealerInformation> _dealerInformationRepository;

        private IRepository<CollectingRequest> _collectingRequestRepository;

        private IRepository<CollectingRequestConfig> _collectingRequestConfigRepository;

        private IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        private IRepository<CollectDealTransactionDetail> _collectDealTransactionDetailRepository;

        private IRepository<Feedback> _feedbackRepository;

        private IRepository<Complaint> _complaintRepository;

        private IRepository<SellerComplaint> _sellerComplaintRepository;

        private IRepository<CollectorComplaint> _collectorComplaintRepository;

        private IRepository<DealerComplaint> _dealerComplaintRepository;

        private IRepository<ImageSlider> _imageSliderRepository;

        private IRepository<Location> _locationRepository;

        private IRepository<Notification> _notificationRepository;

        private IRepository<Promotion> _promotionRepository;

        private IRepository<Role> _roleRepository;

        private IRepository<ScrapCategory> _scrapCategoryRepository;

        private IRepository<ScrapCategoryDetail> _scrapCategoryDetailRepository;

        private IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        private IRepository<SellCollectTransactionDetail> _sellCollectTransactionDetailRepository;

        private IRepository<ServiceTransaction> _serviceTransactionRepository;

        private IRepository<TransactionServiceFeePercent> _transactionServiceFeePercentRepository;

        private IRepository<TransactionAwardAmount> _transactionAwardAmountRepository;

        private IRepository<CollectingRequestRejection> _collectingRequestRejectionRepository;

        private IRepository<CollectorCancelReason> _collectorCancelReasonRepository;

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class.
        /// </summary>
        /// <param name="appDbContext">The application database context.</param>
        public UnitOfWork(AppDbContext appDbContext)
        {
            //Initialize AppDbContext
            AppDbContext = appDbContext;
            //Set Command Timeout
            AppDbContext.Database.SetCommandTimeout(AppSettingValues.CommandTimeout);
        }

        #endregion

        #region Publish Access Repositories

        public IRepository<Account> AccountRepository => _accountRepository ??= (_accountRepository = new Repository<Account>(AppDbContext));

        public IRepository<DealerInformation> DealerInformationRepository => _dealerInformationRepository ??= (_dealerInformationRepository = new Repository<DealerInformation>(AppDbContext));

        public IRepository<CollectingRequest> CollectingRequestRepository => _collectingRequestRepository ??= (_collectingRequestRepository = new Repository<CollectingRequest>(AppDbContext));

        public IRepository<CollectingRequestConfig> CollectingRequestConfigRepository => _collectingRequestConfigRepository ??= (_collectingRequestConfigRepository = new Repository<CollectingRequestConfig>(AppDbContext));

        public IRepository<CollectDealTransaction> CollectDealTransactionRepository => _collectDealTransactionRepository ??= (_collectDealTransactionRepository = new Repository<CollectDealTransaction>(AppDbContext));

        public IRepository<CollectDealTransactionDetail> CollectDealTransactionDetailRepository => _collectDealTransactionDetailRepository ??= (_collectDealTransactionDetailRepository = new Repository<CollectDealTransactionDetail>(AppDbContext));

        public IRepository<Feedback> FeedbackRepository => _feedbackRepository ??= (_feedbackRepository = new Repository<Feedback>(AppDbContext));

        public IRepository<Complaint> ComplaintRepository => _complaintRepository ??= (_complaintRepository = new Repository<Complaint>(AppDbContext));

        public IRepository<SellerComplaint> SellerComplantRepository => _sellerComplaintRepository ??= (_sellerComplaintRepository = new Repository<SellerComplaint>(AppDbContext));

        public IRepository<CollectorComplaint> CollectorComplaintRepository => _collectorComplaintRepository ??= (_collectorComplaintRepository = new Repository<CollectorComplaint>(AppDbContext));

        public IRepository<DealerComplaint> DealerComplaintRepository => _dealerComplaintRepository ??= (_dealerComplaintRepository = new Repository<DealerComplaint>(AppDbContext));

        public IRepository<ImageSlider> ImageSliderRepository => _imageSliderRepository ??= (_imageSliderRepository = new Repository<ImageSlider>(AppDbContext));

        public IRepository<Location> LocationRepository => _locationRepository ??= (_locationRepository = new Repository<Location>(AppDbContext));

        public IRepository<Notification> NotificationRepository => _notificationRepository ??= (_notificationRepository = new Repository<Notification>(AppDbContext));

        public IRepository<Promotion> PromotionRepository => _promotionRepository ??= (_promotionRepository = new Repository<Promotion>(AppDbContext));

        public IRepository<Role> RoleRepository => _roleRepository ??= (_roleRepository = new Repository<Role>(AppDbContext));

        public IRepository<ScrapCategory> ScrapCategoryRepository => _scrapCategoryRepository ??= (_scrapCategoryRepository = new Repository<ScrapCategory>(AppDbContext));

        public IRepository<ScrapCategoryDetail> ScrapCategoryDetailRepository => _scrapCategoryDetailRepository ??= (_scrapCategoryDetailRepository = new Repository<ScrapCategoryDetail>(AppDbContext));

        public IRepository<SellCollectTransaction> SellCollectTransactionRepository => _sellCollectTransactionRepository ??= (_sellCollectTransactionRepository = new Repository<SellCollectTransaction>(AppDbContext));

        public IRepository<SellCollectTransactionDetail> SellCollectTransactionDetailRepository => _sellCollectTransactionDetailRepository ??= (_sellCollectTransactionDetailRepository = new Repository<SellCollectTransactionDetail>(AppDbContext));

        public IRepository<ServiceTransaction> ServiceTransactionRepository => _serviceTransactionRepository ??= (_serviceTransactionRepository = new Repository<ServiceTransaction>(AppDbContext));

        public IRepository<TransactionServiceFeePercent> TransactionServiceFeePercentRepository => _transactionServiceFeePercentRepository ??= (_transactionServiceFeePercentRepository = new Repository<TransactionServiceFeePercent>(AppDbContext));

        public IRepository<TransactionAwardAmount> TransactionAwardAmountRepository => _transactionAwardAmountRepository ??= (_transactionAwardAmountRepository = new Repository<TransactionAwardAmount>(AppDbContext));

        public IRepository<CollectingRequestRejection> CollectingRequestRejectionRepository => _collectingRequestRejectionRepository ??= (_collectingRequestRejectionRepository = new Repository<CollectingRequestRejection>(AppDbContext));

        public IRepository<CollectorCancelReason> CollectorCancelReasonRepository => _collectorCancelReasonRepository ??= (_collectorCancelReasonRepository = new Repository<CollectorCancelReason>(AppDbContext));
        #endregion

        #region Commit Async

        /// <summary>
        /// Commits the asynchronous.
        /// </summary>
        public async Task CommitAsync()
        {
            try
            {
                await AppDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #endregion

        #region IDisposable

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!this.Disposed)
            {
                if (disposing)
                {
                }
            }
            this.Disposed = true;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
