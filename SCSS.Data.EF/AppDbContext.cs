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
        public DbSet<Role> Role { get; set; }

        #endregion

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Data Source=scss-database.cehfzxl85v4h.ap-southeast-1.rds.amazonaws.com;Initial Catalog=SCSS.Demo;User ID=admin;Password=scsspassword123", builder =>
                {
                    builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                });
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {



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
                        entry.CurrentValues["CreateTime"] = DateTime.Now;
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
