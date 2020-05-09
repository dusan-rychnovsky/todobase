using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace todobase
{
    /*
     * This class is needed for generating identity db context when generating migrations.
     * See https://codingblast.com/entityframework-core-idesigntimedbcontextfactory/
     */
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
    {
        public AppDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables()
                .Build();

            var builder = new DbContextOptionsBuilder<AppDbContext>();

            var dbConfig = configuration.GetSection("Db");
            var connectionString = dbConfig.GetSection("ConnectionString").Value;

            builder.UseSqlServer(connectionString);

            return new AppDbContext(builder.Options);
        }
    }
}
