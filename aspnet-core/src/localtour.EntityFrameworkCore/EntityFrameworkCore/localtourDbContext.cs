using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using localtour.Authorization.Roles;
using localtour.Authorization.Users;
using localtour.MultiTenancy;
using localtour.Tours;
using localtour.TourPictures;
using localtour.Reviews;
using localtour.Bookings;
using localtour.Requests;
using localtour.Transactions;
using localtour.Disputes;
using localtour.States;

namespace localtour.EntityFrameworkCore
{
    public class localtourDbContext : AbpZeroDbContext<Tenant, Role, User, localtourDbContext>
    {
        /* Define a DbSet for each entity of the application */
        public virtual DbSet<Tour> Tours { get; set; }
        public virtual DbSet<TourPicture> TourPictures { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Request> Requests { get; set; }
        public virtual DbSet<Transaction> Transactions { get; set; }
        public virtual DbSet<Dispute> Disputes { get; set; }
        public virtual DbSet<State> States { get; set; }

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

            modelBuilder.Entity<TourPicture>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Review>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Booking>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Request>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Transaction>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });

            modelBuilder.Entity<Dispute>(t =>
            {
                t.HasIndex(e => new { e.TenantId });
            });
        }
    }
}
