using Microsoft.EntityFrameworkCore;
using SCSS.Data.EF.Repositories;
using SCSS.Data.Entities;
using SCSS.Utilities.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

        private IRepository<AccountCategory> _accountCategoryRepository;

        private IRepository<Booking> _bookingRepository;

        private IRepository<CategoryAdmin> _categoryAdminRepository;

        private IRepository<CollectDealTransaction> _collectDealTransactionRepository;

        private IRepository<CollectDealTransactionDetail> _collectDealTransactionDetailRepository;

        private IRepository<Feedback> _feedbackRepository;

        private IRepository<ItemType> _itemTypeRepository;

        private IRepository<Location> _locationRepository;

        private IRepository<Notification> _notificationRepository;

        private IRepository<Promotion> _promotionRepository;

        private IRepository<Role> _roleRepository;

        private IRepository<SellCollectTransaction> _sellCollectTransactionRepository;

        private IRepository<SellCollectTransactionDetail> _sellCollectTransactionDetailRepository;

        private IRepository<ServiceTransaction> _serviceTransactionRepository;

        private IRepository<Unit> _unitRepository;

        private IRepository<CollectDealTransactionPromotion> _collectDealTransactionPromotionRepository;

        private IRepository<TransactionServiceFeePercent> _transactionServiceFeePercentRepository;

        private IRepository<TransactionAwardAmount> _transactionAwardAmountRepository;

        private IRepository<BookingRejection> _bookingRejectionRepository;

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

        public IRepository<AccountCategory> AccountCategoryRepository => _accountCategoryRepository ??= (_accountCategoryRepository = new Repository<AccountCategory>(AppDbContext));

        public IRepository<Booking> BookingRepository => _bookingRepository ??= (_bookingRepository = new Repository<Booking>(AppDbContext));

        public IRepository<CategoryAdmin> CategoryAdminRepository => _categoryAdminRepository ??= (_categoryAdminRepository = new Repository<CategoryAdmin>(AppDbContext));

        public IRepository<CollectDealTransaction> CollectDealTransactionRepository => _collectDealTransactionRepository ??= (_collectDealTransactionRepository = new Repository<CollectDealTransaction>(AppDbContext));

        public IRepository<CollectDealTransactionDetail> CollectDealTransactionDetailRepository => _collectDealTransactionDetailRepository ??= (_collectDealTransactionDetailRepository = new Repository<CollectDealTransactionDetail>(AppDbContext));

        public IRepository<Feedback> FeedbackRepository => _feedbackRepository ??= (_feedbackRepository = new Repository<Feedback>(AppDbContext));

        public IRepository<ItemType> ItemTypeRepository => _itemTypeRepository ??= (_itemTypeRepository = new Repository<ItemType>(AppDbContext));

        public IRepository<Location> LocationRepository => _locationRepository ??= (_locationRepository = new Repository<Location>(AppDbContext));

        public IRepository<Notification> NotificationRepository => _notificationRepository ??= (_notificationRepository = new Repository<Notification>(AppDbContext));

        public IRepository<Promotion> PromotionRepository => _promotionRepository ??= (_promotionRepository = new Repository<Promotion>(AppDbContext));

        public IRepository<Role> RoleRepository => _roleRepository ??= (_roleRepository = new Repository<Role>(AppDbContext));

        public IRepository<SellCollectTransaction> SellCollectTransactionRepository => _sellCollectTransactionRepository ??= (_sellCollectTransactionRepository = new Repository<SellCollectTransaction>(AppDbContext));

        public IRepository<SellCollectTransactionDetail> SellCollectTransactionDetailRepository => _sellCollectTransactionDetailRepository ??= (_sellCollectTransactionDetailRepository = new Repository<SellCollectTransactionDetail>(AppDbContext));

        public IRepository<ServiceTransaction> ServiceTransactionRepository => _serviceTransactionRepository ??= (_serviceTransactionRepository = new Repository<ServiceTransaction>(AppDbContext));

        public IRepository<Unit> UnitRepository => _unitRepository ??= (_unitRepository = new Repository<Unit>(AppDbContext));

        public IRepository<CollectDealTransactionPromotion> CollectDealTransactionPromotionRepository => _collectDealTransactionPromotionRepository ??= (_collectDealTransactionPromotionRepository = new Repository<CollectDealTransactionPromotion>(AppDbContext));

        public IRepository<TransactionServiceFeePercent> TransactionServiceFeePercentRepository => _transactionServiceFeePercentRepository ??= (_transactionServiceFeePercentRepository = new Repository<TransactionServiceFeePercent>(AppDbContext));

        public IRepository<TransactionAwardAmount> TransactionAwardAmountRepository => _transactionAwardAmountRepository ??= (_transactionAwardAmountRepository = new Repository<TransactionAwardAmount>(AppDbContext));

        public IRepository<BookingRejection> BookingRejectionRepository => _bookingRejectionRepository ??= (_bookingRejectionRepository = new Repository<BookingRejection>(AppDbContext));

        #endregion

        #region Commit

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
