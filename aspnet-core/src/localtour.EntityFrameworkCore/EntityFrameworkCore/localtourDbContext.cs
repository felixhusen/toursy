using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using localtour.Authorization.Roles;
using localtour.Authorization.Users;
using localtour.MultiTenancy;
using localtour.Tours;

namespace localtour.EntityFrameworkCore
{
    public class localtourDbContext : AbpZeroDbContext<Tenant, Role, User, localtourDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Tour> Tours { get; set; }

        public localtourDbContext(DbContextOptions<localtourDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Tour>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
        }
    }
}
