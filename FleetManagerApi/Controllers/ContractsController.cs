using FleetManagerApi.Data;
using FleetManagerApi.DTOs;
using FleetManagerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ContractsController : ControllerBase
    {
        private readonly FleetDbContext _db;

        public ContractsController(FleetDbContext db)
        {
            _db = db;
        }

        // GET: api/contracts
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Contracts
                .Include(c => c.Vehicle)
                .Include(c => c.Customer)
                .ToListAsync());

        // GET: api/contracts/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) =>
            await _db.Contracts
                .Include(c => c.Vehicle)
                .Include(c => c.Customer)
                .FirstOrDefaultAsync(c => c.ContractID == id) is { } c ? Ok(c) : NotFound();

        // POST: api/contracts
        [HttpPost]
        public async Task<IActionResult> Create(CreateContractDto dto)
        {
            var vehicle = await _db.Vehicles.FindAsync(dto.VehicleID);
            if (vehicle is null) return NotFound(new { message = "Vehicle not found." });

            if (vehicle.Status != "Available")
                return BadRequest(new { message = "Selected vehicle is not available for lease." });

            var customer = await _db.Customers.FindAsync(dto.CustomerID);
            if (customer is null) return NotFound(new { message = "Customer not found." });

            var contract = new Contract
            {
                VehicleID = dto.VehicleID,
                CustomerID = dto.CustomerID,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                TotalAmount = dto.TotalAmount,
                Status = "Active"
            };

            // Automatically transition vehicle status to Rented
            vehicle.Status = "Rented";

            _db.Contracts.Add(contract);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = contract.ContractID }, contract);
        }

        // POST: api/contracts/5/cancel
        [HttpPost("{id}/cancel")]
        public async Task<IActionResult> Cancel(int id)
        {
            var contract = await _db.Contracts.Include(c => c.Vehicle).FirstOrDefaultAsync(c => c.ContractID == id);
            if (contract is null) return NotFound();

            if (contract.Status == "Cancelled" || contract.Status == "Completed")
                return BadRequest(new { message = "Contract is already closed." });

            contract.Status = "Cancelled";

            // Release vehicle back to available
            if (contract.Vehicle != null)
            {
                contract.Vehicle.Status = "Available";
            }

            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}