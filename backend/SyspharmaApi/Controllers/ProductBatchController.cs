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
    public class ProductBatchBatchController(SyspharmaContext context) : ControllerBase
    {
        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBatch>>> GetProductBatches()
        {
            return await _context.ProductBatches.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductBatch>> GetProductBatch(int id)
        {
            var productbatch = await _context.ProductBatches.FindAsync(id);

            if (productbatch == null)
            {
                return NotFound();
            }

            return productbatch;
        }

        [HttpPost]
        public async Task<ActionResult<ProductBatch>> PostProductBatch(ProductBatch productbatch)
        {
            try
            {
                _context.ProductBatches.Add(productbatch);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetProductBatch), new { id = productbatch.Idbatch }, productbatch);
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"ProductBatchController/PostProductBatch", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutProductBatch(int id, ProductBatch productbatch)
        {
            if (id != productbatch.Idbatch)
            {
                return BadRequest();
            }

            _context.Entry(productbatch).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductBatchExists(id))
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
        public async Task<IActionResult> DeleteProductBatch(int id)
        {
            try
            {
                var productbatch = await _context.ProductBatches.FindAsync(id);
                if (productbatch == null)
                {
                    return NotFound();
                }

                _context.ProductBatches.Remove(productbatch);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"ProductBatchController/DeleteProductBatch/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool ProductBatchExists(int id)
        {
            return _context.ProductBatches.Any(e => e.Idbatch == id);
        }
    }
}
