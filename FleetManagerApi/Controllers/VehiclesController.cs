using FleetManagerApi.Data;
using FleetManagerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VehiclesController : ControllerBase
    {
        private readonly FleetDbContext _db;

        public VehiclesController(FleetDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Vehicles.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) =>
            await _db.Vehicles.FindAsync(id) is { } v ? Ok(v) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create(Vehicle vehicle)
        {
            vehicle.Status = "Available";
            _db.Vehicles.Add(vehicle);
            await _db.SaveChangesAsync();
            return CreatedAtAction(nameof(GetById), new { id = vehicle.VehicleID }, vehicle);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Vehicle updatedVehicle)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle is null) return NotFound();

            vehicle.Model = updatedVehicle.Model;
            vehicle.LicensePlate = updatedVehicle.LicensePlate;
            vehicle.DailyRentalPrice = updatedVehicle.DailyRentalPrice;
            vehicle.Status = updatedVehicle.Status;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var vehicle = await _db.Vehicles.FindAsync(id);
            if (vehicle is null) return NotFound();

            vehicle.Status = "Decommissioned";
            await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}