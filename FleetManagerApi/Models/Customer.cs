using System.ComponentModel.DataAnnotations;

namespace FleetManagerApi.Models
{
    // Represents a customer in the fleet management system.
    // The customer has a unique ID, full name, email, phone number, and driver's license number.
    // The CreatedAt property is set to the current UTC time when the customer is created and should not be modified by clients.
    // The model is designed to work with Entity Framework Core, with appropriate data annotations for validation and database schema configuration.
    // The CustomerID serves as the primary key for the customer entity.
  
    public class Customer
    {

        [Key]
        public int CustomerID { get; set; }

        [Required]
        [StringLength(100)]
        public string FullName { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string DriverLicenseNo { get; set; } = string.Empty;

        // Server-controlled — clients should never set this directly (see CustomerDtos.cs).
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
