using Microsoft.AspNetCore.Mvc;
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
            var data = _context.ServiceTypeModels.ToList();

            return Ok(data);
        }
        //Add
        [HttpPost]
        [Route("api/InsertServiceTypeDetails")]
        public IActionResult InsertServiceTypeDetails([FromBody] ServiceTypeModel model)
        {
            try
            {
                ServiceTypeModel obj = new ServiceTypeModel()
                {
                    ServiceTypeName = model.ServiceTypeName,
                    Description=model.Description,
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                };

                _context.ServiceTypeModels.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Service Type Inserted Successfully",
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
        [Route("api/UpdateServiceTypeDetails")]
        public IActionResult UpdateServiceTypeDetails([FromBody] ServiceTypeModel model)
        {
            var data = _context.ServiceTypeModels
              .FirstOrDefault(x => x.ServiceTypeID == model.ServiceTypeID);

            if (data == null)
            {
                return NotFound();
            }

            data.ServiceTypeName = model.ServiceTypeName;
            data.Description = model.Description;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok("Updated Successfully");
        }

    }
}
