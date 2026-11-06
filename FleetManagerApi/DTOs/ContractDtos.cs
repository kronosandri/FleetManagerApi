using System.ComponentModel.DataAnnotations;

namespace FleetManagerApi.DTOs
{
    // Deliberately narrow: only the fields that make sense to set at creation time.
    // VehicleID and CustomerID are required, but Status is not — it will be set to "Available" by default.
    // TotalAmount is required, but it will be validated to be greater than zero in the model.
    public class CreateContractDto
    {
        [Required]
        public int VehicleID { get; set; }

        [Required]
        public int CustomerID { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal TotalAmount { get; set; }
    }

    // Deliberately narrow: only the fields that make sense to edit after creation.
    // VehicleID/CustomerID are excluded — reassigning a contract to a different
    // vehicle/customer should go through cancel + create-new, so the availability
    // checks run again. Status is excluded — use the dedicated /cancel endpoint,
    // which also handles releasing the vehicle back to "Available".
    public class UpdateContractDto
    {
        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Range(0.01, 1000000)]
        public decimal TotalAmount { get; set; }
    }
}
