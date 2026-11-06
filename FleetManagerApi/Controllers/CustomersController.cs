using FleetManagerApi.Data;
using FleetManagerApi.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FleetManagerApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly FleetDbContext _db;

        public CustomersController(FleetDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await _db.Customers.ToListAsync());

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) =>
            await _db.Customers.FindAsync(id) is { } c ? Ok(c) : NotFound();

        [HttpPost]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (await _db.Customers.AnyAsync(c => c.Email == customer.Email))
                return BadRequest(new { message = "Email is already registered." });

            if (await _db.Customers.AnyAsync(c => c.DriverLicenseNo == customer.DriverLicenseNo))
                return BadRequest(new { message = "Driver's license is already registered." });

            customer.CreatedAt = DateTime.UtcNow;
            _db.Customers.Add(customer);
            await _db.SaveChangesAsync();

            return CreatedAtAction(nameof(GetById), new { id = customer.CustomerID }, customer);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, Customer updated)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer is null) return NotFound();

            customer.FullName = updated.FullName;
            customer.Email = updated.Email;
            customer.PhoneNumber = updated.PhoneNumber;
            customer.DriverLicenseNo = updated.DriverLicenseNo;

            await _db.SaveChangesAsync();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _db.Customers.FindAsync(id);
            if (customer is null) return NotFound();

            bool hasContracts = await _db.Contracts.AnyAsync(c => c.CustomerID == id);
            if (hasContracts)
                return BadRequest(new { message = "Cannot remove a customer with contract history." });

            _db.Customers.Remove(customer);
            await _db.SaveChangesAsync();

            return NoContent();
        }
    }
}