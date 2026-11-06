using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FleetManagerApi.Models
{
    // Represents a rental contract between a customer and the fleet management system.
    // The contract includes references to the rented vehicle and the customer, as well as rental period and financial details.
    // Implements IValidatableObject to enforce custom validation rules, such as ensuring the end date is after the start date and that the total amount is positive.
    // The Status property indicates the current state of the contract (e.g., "Active", "Completed", "Cancelled").
    // The Vehicle and Customer navigation properties allow for easy access to related data when querying contracts.
    // The model is designed to work with Entity Framework Core, with appropriate data annotations for validation and database schema configuration.
    // The ContractID serves as the primary key, while VehicleID and CustomerID are foreign keys linking to the respective entities.
    
    public class Contract
    {
        [Key]
        public int ContractID { get; set; }

        [Required]
        public int VehicleID { get; set; }
        public Vehicle? Vehicle { get; set; }

        [Required]
        public int CustomerID { get; set; }
        public Customer? Customer { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        [Column(TypeName = "decimal(10,2)")]
        public decimal TotalAmount { get; set; }

      
        [Required]
        [StringLength(20)]
        public string Status { get; set; } = "Active";

        // Implement IValidatableObject to add custom validation logic
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndDate <= StartDate)
            {
                yield return new ValidationResult(
                    "EndDate must be after StartDate.",
                    new[] { nameof(EndDate) });
            }

            if (TotalAmount <= 0)
            {
                yield return new ValidationResult(
                    "TotalAmount must be greater than zero.",
                    new[] { nameof(TotalAmount) });
            }
        }
    }
}
