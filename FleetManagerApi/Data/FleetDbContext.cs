namespace FleetManagerApi.Data;
using Microsoft.EntityFrameworkCore;
  

public class FleetDbContext : DbContext
    {
    // Constructor that takes DbContextOptions and passes them to the base DbContext class
    // This allows the DbContext to be configured with options such as the database provider and connection string
    // The options are typically configured in the Program.cs file or in a design-time factory for migrations
    // The DbContextOptions are used to configure the DbContext, including the database provider (e.g., SQLite, SQL Server) and other options such as logging and caching
    // The DbContextOptions are also used to configure the DbContext for dependency injection, allowing it to be injected into controllers and services
    public FleetDbContext(DbContextOptions<FleetDbContext> options) : base(options) { }
        // Define DbSets for each entity
        public DbSet<Models.Customer> Customers { get; set; }
        public DbSet<Models.Vehicle> Vehicles { get; set; }
        public DbSet<Models.Contract> Contracts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configure relationships
        // A Contract has one Customer and one Vehicle, but a Customer or Vehicle can have many Contracts.
        // We also specify that deleting a Customer or Vehicle should not cascade delete Contracts.
        // This is important to prevent accidental data loss
        modelBuilder.Entity<Models.Contract>()
            .HasOne(c => c.Customer)
            .WithMany()
            .HasForeignKey(c => c.CustomerID)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Models.Contract>()
            .HasOne(c => c.Vehicle)
            .WithMany()
            .HasForeignKey(c => c.VehicleID)
            .OnDelete(DeleteBehavior.Restrict);

        // Configure unique constraints
        // LicensePlate must be unique across all Vehicles
        // Email must be unique across all Customers
        // DriverLicenseNo must be unique across all Customers

        modelBuilder.Entity<Models.Vehicle>()
            .HasIndex(v => v.LicensePlate)
            .IsUnique();

        modelBuilder.Entity<Models.Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        modelBuilder.Entity<Models.Customer>()
            .HasIndex(c => c.DriverLicenseNo)
            .IsUnique();

        // Configure precision for decimal properties
        // DailyRentalPrice and TotalAmount should have a precision of 10 and scale of 2
        // This means they can store values up to 99999999.99
        // This is important for financial calculations to avoid rounding errors
        modelBuilder.Entity<Models.Vehicle>()
            .Property(v => v.DailyRentalPrice)
            .HasPrecision(10, 2);

        modelBuilder.Entity<Models.Contract>() 
            .Property(c => c.TotalAmount)
            .HasPrecision(10, 2);


    }
}
