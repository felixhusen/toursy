using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using localtour.Configuration;
using localtour.Web;

namespace localtour.EntityFrameworkCore
{
    /* This class is needed to run "dotnet ef ..." commands from command line on development. Not used anywhere else */
    public class localtourDbContextFactory : IDesignTimeDbContextFactory<localtourDbContext>
    {
        public localtourDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<localtourDbContext>();
            var configuration = AppConfigurations.Get(WebContentDirectoryFinder.CalculateContentRootFolder());

            localtourDbContextConfigurer.Configure(builder, configuration.GetConnectionString(localtourConsts.ConnectionStringName));

            return new localtourDbContext(builder.Options);
        }
    }
}
