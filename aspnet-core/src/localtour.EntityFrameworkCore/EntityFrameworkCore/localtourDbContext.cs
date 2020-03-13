using Microsoft.EntityFrameworkCore;
using Abp.Zero.EntityFrameworkCore;
using localtour.Authorization.Roles;
using localtour.Authorization.Users;
using localtour.MultiTenancy;

namespace localtour.EntityFrameworkCore
{
    public class localtourDbContext : AbpZeroDbContext<Tenant, Role, User, localtourDbContext>
    {
        /* Define a DbSet for each entity of the application */
        
        public localtourDbContext(DbContextOptions<localtourDbContext> options)
            : base(options)
        {
        }
    }
}
