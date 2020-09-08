using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace localtour.EntityFrameworkCore
{
    public static class localtourDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<localtourDbContext> builder, string connectionString)
        {
            builder.UseNpgsql(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<localtourDbContext> builder, DbConnection connection)
        {
            builder.UseNpgsql(connection);
        }
    }
}
