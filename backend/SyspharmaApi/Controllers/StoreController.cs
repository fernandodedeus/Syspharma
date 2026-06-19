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
        public async Task<ActionResult<IEnumerable<Store>>> GetStores(int page = 0, int pagesize = 10)
        {
            return await _context.Stores
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
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
            _context.Stores.Add(store);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetStore), new { id = store.Idstore }, store);
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
            var store = await _context.Stores.FindAsync(id);
            if (store == null)
            {
                return NotFound();
            }

            _context.Stores.Remove(store);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StoreExists(int id)
        {
            return _context.Stores.Any(e => e.Idstore == id);
        }
    }
}
