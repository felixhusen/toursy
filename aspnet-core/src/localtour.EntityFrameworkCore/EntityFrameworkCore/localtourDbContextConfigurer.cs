using System.Data.Common;
using Microsoft.EntityFrameworkCore;

namespace localtour.EntityFrameworkCore
{
    public static class localtourDbContextConfigurer
    {
        public static void Configure(DbContextOptionsBuilder<localtourDbContext> builder, string connectionString)
        {
            builder.UseSqlServer(connectionString);
        }

        public static void Configure(DbContextOptionsBuilder<localtourDbContext> builder, DbConnection connection)
        {
            builder.UseSqlServer(connection);
        }
    }
}
