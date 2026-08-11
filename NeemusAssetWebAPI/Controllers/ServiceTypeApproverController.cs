using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ServiceTypeApproverController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public ServiceTypeApproverController(PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/ServiceTypeApproverDetails")]
        public IActionResult GetServiceTypeApproverDetails()
        {
            var data = (from a in _context.ServiceTypeApproverModels

                        join s in _context.ServiceTypeModels
                            on a.Servicetypeid equals s.ServiceTypeID into st
                        from s in st.DefaultIfEmpty()

                        join e in _context.EmployeeMasters
                            on a.Custodianid equals e.CustodianID into emp
                        from e in emp.DefaultIfEmpty()

                        where a.Status == "Active"

                        select new
                        {
                            assginserviceid = a.Assginserviceid,
                            servicetypeid = a.Servicetypeid,
                            serviceTypeName = s != null ? s.ServiceTypeName : "",

                            custodianid = a.Custodianid,
                            custodianName = e != null ? e.CustodianName : "",

                            status = a.Status,
                            createddate = a.Createddate
                        }).ToList();

            return Ok(data);
        }
        // INSERT
        [HttpPost]
        [Route("api/InsertServiceTypeApprover")]
        public IActionResult InsertServiceTypeApprover([FromBody] ServiceTypeApproverModel model)
        {
            try
            {
                var obj = new ServiceTypeApproverModel
                {
                    Servicetypeid = model.Servicetypeid,
                    Custodianid = model.Custodianid,
                    Status = "Active",
                    Createddate = DateTime.UtcNow
                };

                _context.ServiceTypeApproverModels.Add(obj);
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
        [HttpPut]
        [Route("api/UpdateServiceTypeApprover")]
        public IActionResult UpdateServiceTypeApprover([FromBody] ServiceTypeApproverModel model)
        {
            try
            {
                var data = _context.ServiceTypeApproverModels
                    .FirstOrDefault(x => x.Assginserviceid == model.Assginserviceid);

                if (data == null)
                {
                    return NotFound(new
                    {
                        Success = false,
                        Message = "Record Not Found"
                    });
                }

                data.Servicetypeid = model.Servicetypeid;
                data.Custodianid = model.Custodianid;
                data.Status = "Active";

                // Keep the original Createddate if it already exists
                if (data.Createddate == null)
                    data.Createddate = DateTime.UtcNow;

                _context.SaveChanges();

                return Ok(new
                {
                    Success = true,
                    Message = "Updated Successfully",
                    Data = data
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Success = false,
                    Message = ex.InnerException?.Message ?? ex.Message
                });
            }
        }

        // SOFT DELETE
        [HttpDelete]
        [Route("api/DeleteServiceTypeApprover/{id}")]
        public IActionResult DeleteServiceTypeApprover(int id)
        {
            var data = _context.ServiceTypeApproverModels
                               .FirstOrDefault(x => x.Assginserviceid == id);

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