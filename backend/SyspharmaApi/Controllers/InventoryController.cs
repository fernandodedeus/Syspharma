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
    public class InventoryController(SyspharmaContext context) : ControllerBase
    {

        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Inventory>>> GetInventories()
        {
            return await _context.Inventories.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Inventory>> GetInventory(int id)
        {
            var inventory = await _context.Inventories.FindAsync(id);

            if (inventory == null)
            {
                return NotFound();
            }

            return inventory;
        }

        [HttpPost]
        public async Task<ActionResult<Inventory>> PostInventory(Inventory inventory)
        {
            try
            {
                _context.Inventories.Add(inventory);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInventory), new { id = inventory.Idinventory }, inventory);
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"InventoryController/PostInventory", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventory(int id, Inventory inventory)
        {
            if (id != inventory.Idinventory)
            {
                return BadRequest();
            }

            _context.Entry(inventory).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InventoryExists(id))
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
        public async Task<IActionResult> DeleteInventory(int id)
        {
            try
            {
                var inventory = await _context.Inventories.FindAsync(id);
                if (inventory == null)
                {
                    return NotFound();
                }

                _context.Inventories.Remove(inventory);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"InventoryController/DeleteInventory/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool InventoryExists(int id)
        {
            return _context.Inventories.Any(e => e.Idinventory == id);
        }
    }
}
