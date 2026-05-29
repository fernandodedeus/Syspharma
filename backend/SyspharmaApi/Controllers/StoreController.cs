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
    public class StoreController(SyspharmaContext context) : ControllerBase
    {
        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Store>>> GetStores()
        {
            return await _context.Stores.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Store>> GetStore(int id)
        {
            var store = await _context.Stores.FindAsync(id);

            if (store == null)
            {
                return NotFound();
            }

            return store;
        }

        [HttpPost]
        public async Task<ActionResult<Store>> PostStore(Store store)
        {
            try
            {
                _context.Stores.Add(store);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetStore), new { id = store.Idstore }, store);
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"StoreController/PostStore", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutStore(int id, Store store)
        {
            if (id != store.Idstore)
            {
                return BadRequest();
            }

            _context.Entry(store).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StoreExists(id))
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
        public async Task<IActionResult> DeleteStore(int id)
        {
            try
            {
                var store = await _context.Stores.FindAsync(id);
                if (store == null)
                {
                    return NotFound();
                }

                _context.Stores.Remove(store);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"StoreController/DeleteStore/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.Idstore == id);
        }
    }
}
