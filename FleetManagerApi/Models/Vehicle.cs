using System.ComponentModel.DataAnnotations;

namespace FleetManagerApi.Models

{
    // Represents a vehicle in the fleet management system.
    // The vehicle has a unique ID, license plate, model, daily rental price, and status.
    // The model is designed to work with Entity Framework Core, with appropriate data annotations for validation and database schema configuration.
    // The VehicleID serves as the primary key for the vehicle entity.
    public class Vehicle
    {
        [Key] 
        public int VehicleID { get; set; }

        [Required]
        [StringLength(20)]
        public string LicensePlate { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Model { get; set; } = string.Empty;

        [Required]
        public decimal DailyRentalPrice { get; set; }

        [Required]
        [StringLength(50)]
        public string Status { get; set; } = "Available";

    }
}
