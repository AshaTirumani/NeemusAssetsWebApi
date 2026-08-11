using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintTransactionController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public ComplaintTransactionController(PostgreDBContext context)
        {
            _context = context;
        }

        // GET: api/ComplaintTransaction
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintTransaction>>> GetComplaintTransactions()
        {
            return await _context.ComplaintTransactions
                .OrderByDescending(x => x.ComplaintTransactionID)
                .ToListAsync();
        }

        // GET: api/ComplaintTransaction/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintTransaction>> GetComplaintTransaction(int id)
        {
            var transaction = await _context.ComplaintTransactions
                .FirstOrDefaultAsync(x =>
                    x.ComplaintTransactionID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            return transaction;
        }

        // GET transactions for a ComplaintID
        // GET: api/ComplaintTransaction/Complaint/10
        [HttpGet("Complaint/{complaintId}")]
        public async Task<ActionResult<IEnumerable<ComplaintTransaction>>> GetByComplaintId(int complaintId)
        {
            var transactions = await _context.ComplaintTransactions
                .Where(x => x.ComplaintID == complaintId)
                .OrderBy(x => x.CreatedDate)
                .ToListAsync();

            return Ok(transactions);
        }

        // POST: api/ComplaintTransaction
        [HttpPost]
        public async Task<ActionResult<ComplaintTransaction>> PostComplaintTransaction(
            ComplaintTransaction transaction)
        {
            try
            {
                transaction.CreatedDate ??= DateTime.UtcNow;

                _context.ComplaintTransactions.Add(transaction);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetComplaintTransaction),
                    new
                    {
                        id = transaction.ComplaintTransactionID
                    },
                    transaction);
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // PUT: api/ComplaintTransaction/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutComplaintTransaction(
            int id,
            ComplaintTransaction transaction)
        {
            if (id != transaction.ComplaintTransactionID)
            {
                return BadRequest();
            }

            _context.Entry(transaction).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.ComplaintTransactions
                    .AnyAsync(x =>
                        x.ComplaintTransactionID == id))
                {
                    return NotFound();
                }

                throw;
            }

            return NoContent();
        }

        // DELETE: api/ComplaintTransaction/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComplaintTransaction(int id)
        {
            var transaction = await _context.ComplaintTransactions
                .FirstOrDefaultAsync(x =>
                    x.ComplaintTransactionID == id);

            if (transaction == null)
            {
                return NotFound();
            }

            _context.ComplaintTransactions.Remove(transaction);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}