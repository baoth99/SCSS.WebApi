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

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        public AppDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppDbContext"/> class.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="authSession">The authentication session.</param>
        public AppDbContext(DbContextOptions<AppDbContext> options, IAuthSession authSession) : base(options)
        {
            Database.EnsureCreated();
            _authSession = authSession;
        }

        #endregion

        #region DbSet

        // Add Dbset here
        public DbSet<Account> Account { get; set; }

        public DbSet<AuditTrailLog> AuditTrailLog { get; set; }

        public DbSet<CollectorCoordinate> CollectorCoordinate { get; set; }

        public DbSet<DealerInformation> DealerInformation { get; set; }

        public DbSet<CollectingRequest> CollectingRequest { get; set; }

        public DbSet<CollectingRequestConfig> CollectingRequestConfig { get; set; }

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

        public DbSet<ServiceTransaction> ServiceTransaction { get; set; }

        public DbSet<TransactionServiceFeePercent> TransactionServiceFeePercent { get; set; }

        public DbSet<TransactionAwardAmount> TransactionAwardAmount { get; set; }

        public DbSet<CollectorCancelReason> CollectorCancelReason { get; set; }

        public DbSet<Complaint> Complaint { get; set; }

        public DbSet<SellerComplaint> SellerComplaint { get; set; }

        public DbSet<CollectorComplaint> CollectorComplaint { get; set; }

        public DbSet<DealerComplaint> DealerComplaint { get; set; }

        #endregion

        #region OnConfiguring

        /// <summary>
        /// <para>
        /// Override this method to configure the database (and other options) to be used for this context.
        /// This method is called for each instance of the context that is created.
        /// The base implementation does nothing.
        /// </para>
        /// <para>
        /// In situations where an instance of <see cref="T:Microsoft.EntityFrameworkCore.DbContextOptions" /> may or may not have been passed
        /// to the constructor, you can use <see cref="P:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.IsConfigured" /> to determine if
        /// the options have already been set, and skip some or all of the logic in
        /// <see cref="M:Microsoft.EntityFrameworkCore.DbContext.OnConfiguring(Microsoft.EntityFrameworkCore.DbContextOptionsBuilder)" />.
        /// </para>
        /// </summary>
        /// <param name="optionsBuilder">A builder used to create or modify options for this context. Databases (and other extensions)
        /// typically define extension methods on this object that allow you to configure the context.</param>
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

        /// <summary>
        /// Override this method to further configure the model that was discovered by convention from the entity types
        /// exposed in <see cref="T:Microsoft.EntityFrameworkCore.DbSet`1" /> properties on your derived context. The resulting model may be cached
        /// and re-used for subsequent instances of your derived context.
        /// </summary>
        /// <param name="modelBuilder">The builder being used to construct the model for this context. Databases (and other extensions) typically
        /// define extension methods on this object that allow you to configure aspects of the model that are specific
        /// to a given database.</param>
        /// <remarks>
        /// If a model is explicitly set on the options for this context (via <see cref="M:Microsoft.EntityFrameworkCore.DbContextOptionsBuilder.UseModel(Microsoft.EntityFrameworkCore.Metadata.IModel)" />)
        /// then this method will not be run.
        /// </remarks>
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

            modelBuilder.Entity<CollectingRequest>(entity =>
            {
                entity.Property(e => e.CollectorAccountId).IsConcurrencyToken();
                entity.Property(e => e.Status).IsConcurrencyToken();
            });         

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Latitude).HasColumnType("decimal(8,6)");
                entity.Property(e => e.Longitude).HasColumnType("decimal(9,6)");
            });

            modelBuilder.Entity<CollectorCoordinate>(entity =>
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

        /// <summary>
        /// <para>
        /// Saves all changes made in this context to the database.
        /// </para>
        /// <para>
        /// This method will automatically call <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.DetectChanges" /> to discover any
        /// changes to entity instances before saving to the underlying database. This can be disabled via
        /// <see cref="P:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AutoDetectChangesEnabled" />.
        /// </para>
        /// <para>
        /// Multiple active operations on the same context instance are not supported.  Use 'await' to ensure
        /// that any asynchronous operations have completed before calling another method on this context.
        /// </para>
        /// </summary>
        /// <param name="acceptAllChangesOnSuccess">Indicates whether <see cref="M:Microsoft.EntityFrameworkCore.ChangeTracking.ChangeTracker.AcceptAllChanges" /> is called after the changes have
        /// been sent successfully to the database.</param>
        /// <param name="cancellationToken">A <see cref="T:System.Threading.CancellationToken" /> to observe while waiting for the task to complete.</param>
        /// <returns>
        /// A task that represents the asynchronous save operation. The task result contains the
        /// number of state entries written to the database.
        /// </returns>
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            OnBeforeSaving();
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        /// <summary>
        /// Called when [before saving].
        /// </summary>
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
                            addBaseEntity.CreatedTime = DateTimeVN.DATETIME_NOW;
                            addBaseEntity.CreatedBy = accountId;                          
                        }
                        if (entry.Entity is Account addAccountEntity)
                        {
                            addAccountEntity.CreatedTime = DateTimeVN.DATETIME_NOW;
                        }
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is IHasSoftDelete deleteEntry)
                        {
                            entry.State = EntityState.Modified;
                            deleteEntry.IsDeleted = BooleanConstants.TRUE;
                        }
                        break;

                    case EntityState.Modified:
                        if (entry.Entity is BaseEntity modifyBaseEntity)
                        {
                            modifyBaseEntity.UpdatedTime = DateTimeVN.DATETIME_NOW;
                            modifyBaseEntity.UpdatedBy = accountId; 
                        }
                        if (entry.Entity is Account updateAccountEntity)
                        {
                            updateAccountEntity.UpdatedBy = accountId;
                            updateAccountEntity.UpdatedTime = DateTimeVN.DATETIME_NOW;
                        }
                        if (entry.Entity is CollectingRequest modifyCollectingRequest)
                        {
                            entry.State = EntityState.Modified;
                        }
                        break;
                }
            }
        }

        #endregion Before SaveChanges

    }

    #region HasSoftDelete Config

    public static class SoftDeleteQueryExtension
    {
        /// <summary>
        /// Adds the soft delete query filter.
        /// </summary>
        /// <param name="entityData">The entity data.</param>
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension).GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                .MakeGenericMethod(entityData.ClrType);

            var filter = methodToCall.Invoke(null, new object[] { });

            entityData.SetQueryFilter((LambdaExpression)filter);

            entityData.AddIndex(entityData.
                 FindProperty(nameof(IHasSoftDelete.IsDeleted)));
        }

        /// <summary>
        /// Gets the soft delete filter.
        /// </summary>
        /// <typeparam name="TEntity">The type of the entity.</typeparam>
        /// <returns></returns>
        private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : class, IHasSoftDelete
        {
            Expression<Func<TEntity, bool>> filter = x => !x.IsDeleted;
            return filter;
        }
    }

    #endregion

}

