using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Models;
using NeemusAssetWebAPI.Data;
namespace NeemusAssetWebAPI.Controllers
{

    [ApiController]

    public class LocationController : Controller
    {

        private readonly PostgreDBContext _context;
        public LocationController(PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/LocationDetails")]
        public IActionResult GetLocations()
        {
            var data = _context.LocationMasters.ToList();

            return Ok(data);
        }

        [HttpPost]
        [Route("api/InsertLocation")]
        public IActionResult InsertLocation([FromBody] LocationMaster model)
        {
            try
            {
                //if (model == null)
                //{
                //    return BadRequest("Invalid Data");
                //}

                LocationMaster obj = new LocationMaster()
                {
                    Location = model.Location,
                    LocationCode = model.LocationCode,
                    Status = "Active",
                    Block = model.Block,
                    Date = DateTime.Now
                };

                _context.LocationMasters.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Location Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/UpdateLocation")]
        public IActionResult UpdateLocation([FromBody] LocationMaster model)
        {
            var data = _context.LocationMasters
                               .FirstOrDefault(x => x.LocationID == model.LocationID);

            if (data == null)
            {
                return NotFound();
            }

            data.Location = model.Location;
            data.LocationCode = model.LocationCode;
            data.Block = model.Block;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("api/DeleteLocation/{id}")]
        public IActionResult DeleteLocation(int id)
        {
            var data = _context.LocationMasters
                               .FirstOrDefault(x => x.LocationID == id );

            if (data == null)
            {
                return NotFound();
            }

            data.Status = "InActive";

            _context.SaveChanges();

            return Ok("Deleted Successfully");
        }










        //    [HttpPost]
        //    [Route("InsertLocation")]
        //    public IActionResult InsertLocation([FromBody] LocationMaster model)
        //    {
        //        try
        //        {
        //            model.Status = "Active";

        //            _context.LocationMasters.Add(model);

        //            _context.SaveChanges();

        //            return Ok(new
        //            {
        //                message = "Location Inserted Successfully"
        //            });
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.Message);
        //        }
        //    }
        //}

    }
}
