using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ServiceTypeEngineerController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public ServiceTypeEngineerController(PostgreDBContext context)
        {
            _context = context;
        }

        // ===========================
        // GET
        // ===========================
        [HttpGet]
        [Route("api/ServiceTypeEngineerDetails")]
        public IActionResult GetServiceTypeEngineerDetails()
        {
            var data = (from a in _context.ServiceTypeEngineerModels

                        join s in _context.ServiceTypeModels
                            on a.ServiceTypeID equals s.ServiceTypeID into st
                        from s in st.DefaultIfEmpty()

                        join e in _context.EmployeeMasters
                            on a.Custodianid.ToString() equals e.CustodianID into emp
                        from e in emp.DefaultIfEmpty()

                        where a.Status == "Active"

                        select new
                        {
                            a.AssignEnginerid,

                            a.ServiceTypeID,
                            serviceTypeName = s != null ? s.ServiceTypeName : "",

                            a.Custodianid,
                            custodianName = e != null ? e.CustodianName : "",

                            a.Status,
                            a.Createddate
                        }).ToList();

            return Ok(data);
        }

        // ===========================
        // INSERT
        // ===========================
        [HttpPost]
        [Route("api/InsertServiceTypeEngineer")]
        public IActionResult InsertServiceTypeEngineer([FromBody] ServiceTypeEngineerModel model)
        {
            try
            {
                var obj = new ServiceTypeEngineerModel
                {
                    ServiceTypeID = model.ServiceTypeID,
                    Custodianid = model.Custodianid,
                    Status = "Active",
                    Createddate = DateTime.UtcNow
                };

                _context.ServiceTypeEngineerModels.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    Message = "Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // ===========================
        // UPDATE
        // ===========================
        [HttpPut]
        [Route("api/UpdateServiceTypeEngineer")]
        public IActionResult UpdateServiceTypeEngineer([FromBody] ServiceTypeEngineerModel model)
        {
            var data = _context.ServiceTypeEngineerModels
                .FirstOrDefault(x => x.AssignEnginerid == model.AssignEnginerid);

            if (data == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Record Not Found"
                });
            }

            data.ServiceTypeID = model.ServiceTypeID;
            data.Custodianid = model.Custodianid;
            data.Status = "Active";

            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Message = "Updated Successfully"
            });
        }

        // ===========================
        // DELETE (Soft Delete)
        // ===========================
        [HttpDelete]
        [Route("api/DeleteServiceTypeEngineer/{id}")]
        public IActionResult DeleteServiceTypeEngineer(int id)
        {
            var data = _context.ServiceTypeEngineerModels
                .FirstOrDefault(x => x.AssignEnginerid == id);

            if (data == null)
            {
                return NotFound(new
                {
                    Success = false,
                    Message = "Record Not Found"
                });
            }

            data.Status = "Inactive";

            _context.SaveChanges();

            return Ok(new
            {
                Success = true,
                Message = "Deleted Successfully"
            });
        }
    }
}
