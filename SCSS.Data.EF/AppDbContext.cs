using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using SCSS.Data.Entities;
using SCSS.Utilities.AuthSessionConfig;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SCSS.Data.EF
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            Database.EnsureCreated();
        }


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

        public DbSet<ScheduleType> ScheduleType { get; set; }

        public DbSet<SellCollectTransaction> SellCollectTransaction { get; set; }

        public DbSet<SellCollectTransactionDetail> SellCollectTransactionDetail { get; set; }

        public DbSet<ServiceTransaction> ServiceTransaction { get; set; }

        public DbSet<TimePeriod> TimePeriod { get; set; }

        public DbSet<Unit> Unit { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("ConnectionString", builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            #region Entities Config

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<AccountCategory>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<Booking>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<CategoryAdmin>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<CollectDealTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<CollectDealTransactionDetail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Feedback>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Location>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<Promotion>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<ScheduleType>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<SellCollectTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<SellCollectTransactionDetail>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<ServiceTransaction>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
            });

            modelBuilder.Entity<TimePeriod>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
            });

            modelBuilder.Entity<Unit>(entity =>
            {
                entity.Property(e => e.Id).HasDefaultValueSql("newsequentialid()");
                entity.Property(e => e.CreateTime).HasDefaultValue(DateTime.Now);
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

        #region Before SaveChanges

        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            OnBeforeSaving();
            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        private void OnBeforeSaving()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.CurrentValues["IsDeleted"] = false;
                        //entry.CurrentValues["CreateTime"] = DateTime.Now;
                        break;

                    case EntityState.Deleted:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["IsDeleted"] = true;
                        entry.CurrentValues["DeleteTime"] = DateTime.Now;
                        break;

                    case EntityState.Modified:
                        entry.State = EntityState.Modified;
                        entry.CurrentValues["ModifyTime"] = DateTime.Now;
                        entry.CurrentValues["ModifyBy"] = AuthSessionGlobalVariable.UserSession.Id; // Custom
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

