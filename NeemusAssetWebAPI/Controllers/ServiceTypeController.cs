using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ServiceTypeController : Controller
    {
        private readonly PostgreDBContext _context;
        public ServiceTypeController(PostgreDBContext context)
        {
            _context = context;
        }
        //Get
        [HttpGet]
        [Route("api/ServiceTypeDetails")]
        public IActionResult GetServiceTypeDetails()
        {
            var data = _context.ServiceTypeModels
                               .Where(x => x.Status == "Active")
                               .ToList();

            return Ok(data);
        }
        //Add
        // Add
        [HttpPost]
        [Route("api/InsertServiceTypeDetails")]
        public IActionResult InsertServiceTypeDetails([FromBody] ServiceTypeModel model)
        {
            try
            {
                var obj = new ServiceTypeModel
                {
                    ServiceTypeName = model.ServiceTypeName,
                    Description = model.Description,
                    Status = "Active",
                    CreatedDate = DateTime.UtcNow   // Use UTC
                };

                _context.ServiceTypeModels.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Service Type Inserted Successfully",
                    Data = obj
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // edit
        [HttpPut]
        [Route("api/UpdateServiceTypeDetails")]
        public IActionResult UpdateServiceTypeDetails([FromBody] ServiceTypeModel model)
        {
            var data = _context.ServiceTypeModels
                .FirstOrDefault(x => x.ServiceTypeID == model.ServiceTypeID);

            if (data == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Service Type Not Found"
                });
            }

            data.ServiceTypeName = model.ServiceTypeName;
            data.Description = model.Description;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Message = "Updated Successfully"
            });
        }


        // Soft Delete
        [HttpDelete]
        [Route("api/DeleteServiceTypeDetails/{id}")]
        public IActionResult DeleteServiceTypeDetails(int id)
        {
            var data = _context.ServiceTypeModels
                               .FirstOrDefault(x => x.ServiceTypeID == id);

            if (data == null)
            {
                return NotFound("Service Type Not Found");
            }

            data.Status = "Inactive";

            _context.SaveChanges();

            return Ok("Service Type Inactivated Successfully");
        }
    }
}
