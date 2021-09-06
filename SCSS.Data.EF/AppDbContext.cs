using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using SCSS.Utilities.Configurations;
using SCSS.Utilities.Constants;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;

namespace SCSS.Data.EF
{
    public class AppDbContext : DbContext
    {
        #region Fields

        /// <summary>
        /// The authentication session
        /// </summary>
        private readonly IAuthSession _authSession;

        #endregion Fields

        #region Constructor

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options, IAuthSession authSession) : base(options)
        {
            Database.EnsureCreated();
            _authSession = authSession;
        }

        #endregion

        #region DbSet

        // Add Dbset here
        public DbSet<Account> Account { get; set; }

        public DbSet<DealerInformation> DealerInformation { get; set; }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<CollectDealTransaction> CollectDealTransaction { get; set; }

        public DbSet<CollectDealTransactionDetail> CollectDealTransactionDetail { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<ImageSlider> ImageSlider { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<Notification> Notification { get; set; }

        public DbSet<Promotion> Promotion { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<ScrapCategory> ScrapCategory { get; set; }

        public DbSet<ScrapCategoryDetail> ScrapCategoryDetail { get; set; }

        public DbSet<SellCollectTransaction> SellCollectTransaction { get; set; }

        public DbSet<SellCollectTransactionDetail> SellCollectTransactionDetail { get; set; }

        public DbSet<ServicePack> ServicePack { get; set; }

        public DbSet<Subscription> Subscription { get; set; }

        public DbSet<ServiceTransaction> ServiceTransaction { get; set; }

        public DbSet<TransactionServiceFeePercent> TransactionServiceFeePercent { get; set; }

        public DbSet<TransactionAwardAmount> TransactionAwardAmount { get; set; }

        public DbSet<BookingRejection> BookingRejection { get; set; }

        #endregion

        #region OnConfiguring

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(AppSettingValues.SqlConnectionString, builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }   
            base.OnConfiguring(optionsBuilder);
        }

        #endregion

        #region OnModelCreating

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Entities Config

            modelBuilder.Entity<Account>(entity =>
            {
                entity.HasIndex(e => e.Phone).IsUnique();
            });

            modelBuilder.Entity<DealerInformation>(entity =>
            {
                entity.HasIndex(e => e.DealerPhone).IsUnique();
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.CollectorAccountId).IsConcurrencyToken();
                entity.Property(e => e.Status).IsConcurrencyToken();
            });         

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Latitude).HasColumnType("decimal(8,6)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(9,6)");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            #endregion


            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(IHasSoftDelete).IsAssignableFrom(entityType.ClrType))
                {
                    entityType.AddSoftDeleteQueryFilter();

                }
            }
            base.OnModelCreating(modelBuilder);
        }

        #endregion

        #region Before SaveChanges

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        private void OnBeforeSaving()
        {
            ChangeTracker.DetectChanges();

            var accountId = _authSession.UserSession != null ? _authSession.UserSession.Id : Guid.Empty;

            var entitiesTrackingChanged = ChangeTracker.Entries().Where(e => e.State == EntityState.Added ||
                                                                             e.State == EntityState.Modified ||
                                                                             e.State == EntityState.Deleted);

            foreach (var entry in entitiesTrackingChanged)
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        if (entry.Entity is BaseEntity addBaseEntity)
                        {
                            addBaseEntity.CreatedTime = DateTime.Now;
                            addBaseEntity.CreatedBy = accountId;                          
                        }
                        if (entry.Entity is Account addAccountEntity)
                        {
                            addAccountEntity.CreatedTime = DateTime.Now;
                        }
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is IHasSoftDelete deleteEntry)
                        {
                            entry.State = EntityState.Modified;
                            deleteEntry.IsDeleted = BooleanConstants.TRUE;
                        }
                        if (entry.Entity is ScrapCategory scrapCategoryEntry)
                        {
                            entry.State = EntityState.Modified;
                            scrapCategoryEntry.IsDeleted = BooleanConstants.TRUE;
                        }
                        if (entry.Entity is ScrapCategoryDetail scrapCategoryDetailEntry)
                        {
                            entry.State = EntityState.Modified;
                            scrapCategoryDetailEntry.IsDeleted = BooleanConstants.TRUE;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is BaseEntity modifyBaseEntity)
                        {
                            modifyBaseEntity.UpdatedTime = DateTime.Now;
                            modifyBaseEntity.UpdatedBy = accountId; 
                        }                         
                        break;
                }
            }
        }

        #endregion

    }

    #region HasSoftDelete Config

    public static class SoftDeleteQueryExtension
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension).GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);

            var filter = methodToCall.Invoke(null, new object[] { });

            entityData.SetQueryFilter((LambdaExpression)filter);

            entityData.AddIndex(entityData.
                 FindProperty(nameof(IHasSoftDelete.IsDeleted)));
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, IHasSoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }

    #endregion

}

