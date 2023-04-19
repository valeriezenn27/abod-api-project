using abod_api_project.Interface;
using abod_api_project.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace abod_api_project.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }

        public Task<int> SaveChangesAsync()
        {
            var now = DateTime.UtcNow;

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is IAuditable auditable)
                {
                    switch (entry.State)
                    {
                        case EntityState.Added:
                            auditable.CreatedAt = now;
                            auditable.UpdatedAt = now;
                            break;

                        case EntityState.Modified:
                            auditable.UpdatedAt = now;
                            break;
                    }
                }

                if (entry.Entity is ISoftDeletable deletable && entry.State == EntityState.Deleted)
                {
                    entry.State = EntityState.Modified;
                    deletable.Deleted = true;
                    deletable.DeletedAt = now;
                }
            }

            return base.SaveChangesAsync();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasQueryFilter(x => !x.Deleted);
        }
    }
}
