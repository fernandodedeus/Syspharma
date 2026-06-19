using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyspharmaApi.Context;
using SyspharmaApi.Helpers;
using SyspharmaApi.Models;
using System.Net;

namespace SyspharmaApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class InventoryMovementController(SyspharmaContext context) : ControllerBase
    {

        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMovement>>> GetInventoryMovements(int page = 0, int pagesize = 10)
        {
            return await _context.InventoryMovements
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryMovement>> GetInventoryMovement(int id)
        {
            var inventorymovement = await _context.InventoryMovements.FindAsync(id);

            if (inventorymovement == null)
            {
                return NotFound();
            }

            return inventorymovement;
        }

        [HttpPost]
        public async Task<ActionResult<InventoryMovement>> PostInventoryMovement(InventoryMovement inventorymovement)
        {
            _context.InventoryMovements.Add(inventorymovement);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryMovement), new { id = inventorymovement.IdinventoryMovement }, inventorymovement);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryMovement(int id, InventoryMovement inventorymovement)
        {
            if (id != inventorymovement.IdinventoryMovement)
            {
                return BadRequest();
            }

            _context.Entry(inventorymovement).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryMovementExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryMovement(int id)
        {
            var inventorymovement = await _context.InventoryMovements.FindAsync(id);
            if (inventorymovement == null)
            {
                return NotFound();
            }

            _context.InventoryMovements.Remove(inventorymovement);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryMovementExists(int id)
        {
            return _context.InventoryMovements.Any(e => e.IdinventoryMovement == id);
        }
    }
}
