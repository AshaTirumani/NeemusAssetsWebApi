using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AuditController : Controller
    {
        private readonly PostgreDBContext _context;

        public AuditController(PostgreDBContext context)
        {
            _context = context;
        }

        // GET API
        [HttpGet]
        [Route("api/AuditDetails")]
        public IActionResult GetAudits()
        {
            var data = _context.AuditMasters.ToList();

            return Ok(data);
        }

        // INSERT API
        [HttpPost]
        [Route("api/InsertAudit")]
        public IActionResult InsertAudit([FromBody] AuditMaster model)
        {
            try
            {
                AuditMaster obj = new AuditMaster()
                {
                    AuditDate = model.AuditDate,
                    AuditName = model.AuditName,
                    AuditDescription = model.AuditDescription,
                    UnitNo = model.UnitNo,
                    AuditBy = model.AuditBy,
                    Status = "Active",
                    AuditStatus = model.AuditStatus,
                    LocationID = model.LocationID,
                    TotalStock = model.TotalStock,
                    CustodianDepartment = model.CustodianDepartment,
                    CustDepartmentCode = model.CustDepartmentCode,
                    CustDesignation = model.CustDesignation,
                    CustodianName = model.CustodianName,
                    CompletionDate = model.CompletionDate,
                    AdminRemarks = model.AdminRemarks
                };

                _context.AuditMasters.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Audit Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // UPDATE API
        [HttpPut]
        [Route("api/UpdateAudit")]
        public IActionResult UpdateAudit([FromBody] AuditMaster model)
        {
            try
            {
                var data = _context.AuditMasters
                                   .FirstOrDefault(x => x.AuditID == model.AuditID);

                if (data == null)
                {
                    return BadRequest("Audit ID not found");
                }

                data.AuditDate = model.AuditDate;
                data.AuditName = model.AuditName;
                data.AuditDescription = model.AuditDescription;
                data.UnitNo = model.UnitNo;
                data.AuditBy = model.AuditBy;
                data.Status = model.Status;
                data.AuditStatus = model.AuditStatus;
                data.LocationID = model.LocationID;
                data.TotalStock = model.TotalStock;
                data.CustodianDepartment = model.CustodianDepartment;
                data.CustDepartmentCode = model.CustDepartmentCode;
                data.CustDesignation = model.CustDesignation;
                data.CustodianName = model.CustodianName;
                data.CompletionDate = model.CompletionDate;
                data.AdminRemarks = model.AdminRemarks;

                _context.SaveChanges();

                return Ok("Updated Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }

        [HttpDelete]
        [Route("api/DeleteAudit/{id}")]
        public IActionResult DeleteAudit(int id)
        {
            try
            {
                var data = _context.AuditMasters
                                   .FirstOrDefault(x => x.AuditID == id);

                if (data == null)
                {
                    return NotFound("Audit Not Found");
                }

                data.Status = "InActive";

                _context.SaveChanges();

                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }
    }
    }

        
    