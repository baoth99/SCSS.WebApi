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

        public DbSet<AccountCategory> MyProperty { get; set; }

        public DbSet<Booking> Booking { get; set; }

        public DbSet<CategoryAdmin> CategoryAdmin { get; set; }

        public DbSet<CollectDealTransaction> CollectDealTransaction { get; set; }

        public DbSet<CollectDealTransactionDetail> CollectDealTransactionDetail { get; set; }

        public DbSet<Feedback> Feedback { get; set; }

        public DbSet<ItemType> ItemType { get; set; }

        public DbSet<Location> Location { get; set; }

        public DbSet<Notification> Notification { get; set; }

        public DbSet<Promotion> Promotion { get; set; }

        public DbSet<Role> Role { get; set; }

        public DbSet<SellCollectTransaction> SellCollectTransaction { get; set; }

        public DbSet<SellCollectTransactionDetail> SellCollectTransactionDetail { get; set; }

        public DbSet<ServiceTransaction> ServiceTransaction { get; set; }

        public DbSet<Unit> Unit { get; set; }

        public DbSet<CollectDealTransactionPromotion> CollectDealTransactionPromotion { get; set; }

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

            modelBuilder.Entity<AccountCategory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CollectorAccountId).IsConcurrencyToken();
                entity.Property(e => e.Status).IsConcurrencyToken();
            });

            modelBuilder.Entity<CategoryAdmin>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<CollectDealTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.BonusAmount).HasColumnType("decimal(15,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(15,2)");
            });

            modelBuilder.Entity<CollectDealTransactionDetail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.BonusAmount).HasColumnType("decimal(15,2)");
                entity.Property(e => e.Total).HasColumnType("decimal(15,2)");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.Latitude).HasColumnType("decimal(8,6)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(9,6)");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<SellCollectTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.Total).HasColumnType("decimal(15,2)");
            });

            modelBuilder.Entity<SellCollectTransactionDetail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.Total).HasColumnType("decimal(15,2)");
            });

            modelBuilder.Entity<ServiceTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.Amount).HasColumnType("decimal(15,2)");

            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<CollectDealTransactionPromotion>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.BonusAmount).HasColumnType("decimal(15,2)");
            });

            modelBuilder.Entity<TransactionServiceFeePercent>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<TransactionAwardAmount>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<BookingRejection>(entity =>
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
                        if (entry.Entity is BaseEntity)
                        {
                            entry.CurrentValues["CreatedTime"] = DateTime.Now;
                            entry.CurrentValues["CreatedBy"] = accountId;                          
                        }
                        if (entry.Entity is Account)
                        {
                            entry.CurrentValues["CreatedTime"] = DateTime.Now;
                        }
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is IHasSoftDelete)
                        {
                            entry.CurrentValues["IsDeleted"] = BooleanConstants.TRUE;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is BaseEntity)
                        {
                            entry.CurrentValues["UpdatedTime"] = DateTime.Now;
                            entry.CurrentValues["UpdatedBy"] = accountId; // Custom
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

