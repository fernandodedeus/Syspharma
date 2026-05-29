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
    public class PaymentController(SyspharmaContext context) : ControllerBase
    {

        private readonly SyspharmaContext _context = context;

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _context.Payments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> GetPayment(int id)
        {
            var payment = await _context.Payments.FindAsync(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpPost]
        public async Task<ActionResult<Payment>> PostPayment(Payment payment)
        {
            try
            {
                _context.Payments.Add(payment);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetPayment), new { id = payment.Idpayment }, payment);
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"PaymentController/PostPayment", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutPayment(int id, Payment payment)
        {
            if (id != payment.Idpayment)
            {
                return BadRequest();
            }

            _context.Entry(payment).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaymentExists(id))
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
        public async Task<IActionResult> DeletePayment(int id)
        {
            try
            {
                var payment = await _context.Payments.FindAsync(id);
                if (payment == null)
                {
                    return NotFound();
                }

                _context.Payments.Remove(payment);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            catch (Exception ex)
            {
                await ErrorLogger.Log(_context, $"PaymentController/DeletePayment/id={id}", ex.Message);
                return StatusCode((int)HttpStatusCode.InternalServerError, ex.Message);
            }
        }

        private bool PaymentExists(int id)
        {
            return _context.Payments.Any(e => e.Idpayment == id);
        }
    }
}
