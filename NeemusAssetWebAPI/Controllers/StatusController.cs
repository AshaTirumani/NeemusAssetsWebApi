using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StatusController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public StatusController(PostgreDBContext context)
        {
            _context = context;
        }

        // GET: api/Status
        [HttpGet]
        public async Task<ActionResult<IEnumerable<StatusMaster>>> GetStatus()
        {
            return await _context.StatusMaster.ToListAsync();
        }

        // GET: api/Status/1
        [HttpGet("{id}")]
        public async Task<ActionResult<StatusMaster>> GetStatusById(int id)
        {
            var status = await _context.StatusMaster.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            return status;
        }

        // POST: api/Status
        [HttpPost]
        public async Task<ActionResult<StatusMaster>> InsertStatus(StatusMaster status)
        {
            _context.StatusMaster.Add(status);
            await _context.SaveChangesAsync();

            return Ok(status);
        }

        // PUT: api/Status/1
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateStatus(int id, StatusMaster status)
        {
            if (id != status.StatusID)
            {
                return BadRequest();
            }

            _context.Entry(status).State = EntityState.Modified;

            await _context.SaveChangesAsync();

            return Ok(status);
        }

        // DELETE: api/Status/1
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStatus(int id)
        {
            var status = await _context.StatusMaster.FindAsync(id);

            if (status == null)
            {
                return NotFound();
            }

            _context.StatusMaster.Remove(status);

            await _context.SaveChangesAsync();

            return Ok("Deleted Successfully");
        }
    }
}