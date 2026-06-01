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
    public class InventoryMovementMovementController(SyspharmaContext context) : ControllerBase
    {

        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryMovement>>> GetInventoryMovements()
        {
            return await _context.InventoryMovements.ToListAsync();
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
            try
            {
                _context.InventoryMovements.Add(inventorymovement);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetInventoryMovement), new { id = inventorymovement.IdinventoryMovement }, inventorymovement);
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"InventoryMovementController/PostInventoryMovement", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
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
            try
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
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"InventoryMovementController/DeleteInventoryMovement/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool InventoryMovementExists(int id)
        {
            return _context.InventoryMovements.Any(e => e.IdinventoryMovement == id);
        }
    }
}
