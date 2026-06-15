using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    
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
        [Route("api/StatusDetails")]
        public IActionResult GetStatus()
        {
            var data = _context.StatusMasters.Where(x => x.Status == "Active").ToList();

            return Ok(data);
        }
        //Add
        [HttpPost]
        [Route("api/InsertStatusDetails")]
        public IActionResult InsertStatusDetails([FromBody] StatusMaster model)
        {
            try
            {
                StatusMaster obj = new StatusMaster()
                {
                    StatusName = model.StatusName,
                    StatusCode = model.StatusCode,
                    Status = "Active"

                };

                _context.StatusMasters.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Status Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // edit
        [HttpPut]
        [Route("api/UpdateStatusDetails")]
        public IActionResult UpdateStatusDetails([FromBody] StatusMaster model)
        {
            var data = _context.StatusMasters
              .FirstOrDefault(x => x.StatusID == model.StatusID);

            if (data == null)
            {
                return NotFound();
            }

            data.StatusName = model.StatusName;
            data.StatusCode = model.StatusCode;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok();
        }

        //delete
        [HttpDelete]
        [Route("api/DeleteStatusDetails/{id}")]
        public IActionResult DeleteStatusDetails(int id)
        {
            var data = _context.StatusMasters
                               .FirstOrDefault(x => x.StatusID == id);

            if (data == null)
            {
                return NotFound();
            }

            data.Status = "InActive";

            _context.SaveChanges();

            return Ok();
        }








        //public async Task<ActionResult<IEnumerable<StatusMaster>>> GetStatus()
        //{
        //    return await _context.StatusMaster.ToListAsync();
        //}

        // GET: api/Status/1
        //[HttpGet("{id}")]

        //public async Task<ActionResult<StatusMaster>> GetStatusById(int id)
        //{
        //    var status = await _context.StatusMaster.FindAsync(id);

        //    if (status == null)
        //    {
        //        return NotFound();
        //    }

        //    return status;
        //}

        // POST: api/Status
        //[HttpPost]

        //public async Task<ActionResult<StatusMaster>> InsertStatus(StatusMaster status)
        //{
        //    _context.StatusMaster.Add(status);
        //    await _context.SaveChangesAsync();

        //    return Ok(status);
        //}

        //// PUT: api/Status/1
        //[HttpPut("{id}")]
        //public async Task<IActionResult> UpdateStatus(int id, StatusMaster status)
        //{
        //    if (id != status.StatusID)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(status).State = EntityState.Modified;

        //    await _context.SaveChangesAsync();

        //    return Ok(status);
        //}

        //// DELETE: api/Status/1
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteStatus(int id)
        //{
        //    var status = await _context.StatusMaster.FindAsync(id);

        //    if (status == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.StatusMaster.Remove(status);

        //    await _context.SaveChangesAsync();

        //    return Ok("Deleted Successfully");
        //}
    }
}