using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SyspharmaApi.Context;
using SyspharmaApi.Contracts.Product;
using SyspharmaApi.Helpers;
using SyspharmaApi.Models;
using System.Net;

namespace SyspharmaApi.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProductBatchController(SyspharmaContext context) : ControllerBase
    {
        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductBatch>>> GetProductBatches(int page = 0, int pagesize = 10)
        {
            return await _context.ProductBatches
                .Skip(page * pagesize)
                .Take(pagesize)
                .ToListAsync();
        }

        [HttpGet("expiringbatches")]
        public async Task<ActionResult<IEnumerable<ExpiringBatches>>> GetExpiringBatches()
        {
            var expiresmaxConfig = await _context.Configs.FindAsync((int)ConfigType.ExpirationDaysAlert);
            int expiresmax = expiresmaxConfig?.Value ?? 30;
            var today = DateOnly.FromDateTime(DateTime.Today);

            return await _context.ProductBatches
                .Where(pb => pb.Expirationdate <= DateOnly.FromDateTime(DateTime.Today.AddDays(expiresmax)))
                .Join(
                    _context.Products,
                    pb => pb.Idproduct,
                    pr => pr.Idproduct,
                    (pb, pr) => new ExpiringBatches
                    (
                        pr.Idproduct,
                        pb.Idbatch,
                        pr.Internalcode,
                        pr.Description,
                        pb.Batchcode,
                        pb.Expirationdate.DayNumber - today.DayNumber
                    )
                )
                .ToListAsync();
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
            _context.ProductBatches.Add(productbatch);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetProductBatch), new { id = productbatch.Idbatch }, productbatch);
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
            var productbatch = await _context.ProductBatches.FindAsync(id);
            if (productbatch == null)
            {
                return NotFound();
            }

            _context.ProductBatches.Remove(productbatch);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductBatchExists(int id)
        {
            return _context.ProductBatches.Any(e => e.Idbatch == id);
        }
    }
}
