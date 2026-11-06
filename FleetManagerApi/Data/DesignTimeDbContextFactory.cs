
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace FleetManagerApi.Data
{
    // This class is used to create a design-time DbContext for EF Core migrations.
    // It implements the IDesignTimeDbContextFactory interface, which allows EF Core tools to create a DbContext instance at design time.
    // This is necessary because the DbContext is usually configured in the Program.cs file, which is not available at design time.
    // The CreateDbContext method reads the connection string from the appsettings.json file and uses it to configure the DbContextOptions for the FleetDbContext.
    // The FleetDbContext is then returned, allowing EF Core tools to perform migrations and other design-time operations.
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<FleetDbContext>
    {
        public FleetDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<FleetDbContext>();
            var connectionString = configuration.GetConnectionString("DefaultConnection");

            builder.UseSqlite(connectionString);
            return new FleetDbContext(builder.Options);
        }


    }
}