//using Microsoft.AspNetCore.Mvc;
//using NeemusAssetWebAPI.Data;
//using NeemusAssetWebAPI.Models;

//namespace NeemusAssetWebAPI.Controllers
//{
//    [ApiController]
//    public class AuditController : Controller
//    {
//        private readonly PostgreDBContext _context;

//        public AuditController(PostgreDBContext context)
//        {
//            _context = context;
//        }

//        // GET API
//        [HttpGet]
//        [Route("api/AuditDetails")]
//        public IActionResult GetAudits()
//        {
//            var data = _context.AuditMasters.ToList();

//            return Ok(data);
//        }

//        // INSERT API
//        [HttpPost]
//        [Route("api/InsertAudit")]
//        public IActionResult InsertAudit([FromBody] AuditMaster model)
//        {
//            try
//            {
//                AuditMaster obj = new AuditMaster()
//                {
//                    AuditDate = model.AuditDate,
//                    AuditName = model.AuditName,
//                    AuditDescription = model.AuditDescription,
//                    UnitNo = model.UnitNo,
//                    AuditBy = model.AuditBy,
//                    Status = "Active",
//                    AuditStatus = model.AuditStatus,
//                    LocationID = model.LocationID,
//                    TotalStock = model.TotalStock,
//                    CustodianDepartment = model.CustodianDepartment,
//                    CustDepartmentCode = model.CustDepartmentCode,
//                    CustDesignation = model.CustDesignation,
//                    CustodianName = model.CustodianName,
//                    CompletionDate = model.CompletionDate,
//                    AdminRemarks = model.AdminRemarks
//                };

//                _context.AuditMasters.Add(obj);
//                _context.SaveChanges();

//                return Ok(new
//                {
//                    Message = "Audit Inserted Successfully",
//                    Data = obj
//                });
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        // UPDATE API
//        [HttpPut]
//        [Route("api/UpdateAudit")]
//        public IActionResult UpdateAudit([FromBody] AuditMaster model)
//        {
//            try
//            {
//                var data = _context.AuditMasters
//                                   .FirstOrDefault(x => x.AuditID == model.AuditID);

//                if (data == null)
//                {
//                    return BadRequest("Audit ID not found");
//                }

//                data.AuditDate = model.AuditDate;
//                data.AuditName = model.AuditName;
//                data.AuditDescription = model.AuditDescription;
//                data.UnitNo = model.UnitNo;
//                data.AuditBy = model.AuditBy;
//                data.Status = model.Status;
//                data.AuditStatus = model.AuditStatus;
//                data.LocationID = model.LocationID;
//                data.TotalStock = model.TotalStock;
//                data.CustodianDepartment = model.CustodianDepartment;
//                data.CustDepartmentCode = model.CustDepartmentCode;
//                data.CustDesignation = model.CustDesignation;
//                data.CustodianName = model.CustodianName;
//                data.CompletionDate = model.CompletionDate;
//                data.AdminRemarks = model.AdminRemarks;

//                _context.SaveChanges();

//                return Ok("Updated Successfully");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new
//                {
//                    Message = ex.Message,
//                    InnerException = ex.InnerException?.Message
//                });
//            }
//        }

//        [HttpDelete]
//        [Route("api/DeleteAudit/{id}")]
//        public IActionResult DeleteAudit(int id)
//        {
//            try
//            {
//                var data = _context.AuditMasters
//                                   .FirstOrDefault(x => x.AuditID == id);

//                if (data == null)
//                {
//                    return NotFound("Audit Not Found");
//                }

//                data.Status = "InActive";

//                _context.SaveChanges();

//                return Ok("Deleted Successfully");
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(new
//                {
//                    Message = ex.Message,
//                    InnerException = ex.InnerException?.Message
//                });
//            }
//        }
//    }
//}



//////////////changed code
///

using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
using Microsoft.EntityFrameworkCore;
namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AuditController : Controller
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetSAPDBContext;

        public AuditController(
            PostgreDBContext context,
            AssetSAPDBContext assetSAPDBContext)
        {
            _context = context;
            _assetSAPDBContext = assetSAPDBContext;
        }

        // GET API
        [HttpGet]
        [Route("api/AuditDetails")]
        public IActionResult GetAudits()
        {
            var data = _context.AuditMasters.ToList();
            return Ok(data);
        }

       
        [HttpPost]
        [Route("api/InsertAudit")]
        public IActionResult InsertAudit([FromBody] AuditMaster model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Invalid audit data.");

                if (string.IsNullOrWhiteSpace(model.AuditName))
                    return BadRequest("Audit Name is required.");

                if (model.LocationID <= 0)
                    return BadRequest("Location is required.");

                AuditMaster obj = new AuditMaster()
                {
                    AuditDate = model.AuditDate,
                    AuditName = model.AuditName.Trim(),
                    AuditDescription = model.AuditDescription?.Trim(),
                    UnitNo = model.UnitNo,
                    AuditBy = model.AuditBy,
                    // Lifecycle on create
                    Status = "Started",
                    AuditStatus = "Active",
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

        // UPDATE API — completion: Status = Completed, AuditStatus = Inactive (from client)
        [HttpPut]
        [Route("api/UpdateAudit")]
        public IActionResult UpdateAudit([FromBody] AuditMaster model)
        {
            try
            {
                if (model == null || model.AuditID <= 0)
                    return BadRequest("Invalid audit data.");

                var data = _context.AuditMasters
                    .FirstOrDefault(x => x.AuditID == model.AuditID);

                if (data == null)
                    return BadRequest("Audit ID not found");

                data.AuditDate = model.AuditDate;
                data.AuditName = model.AuditName;
                data.AuditDescription = model.AuditDescription;
                data.UnitNo = model.UnitNo;
                data.AuditBy = model.AuditBy;
                data.LocationID = model.LocationID;
                data.TotalStock = model.TotalStock;
                data.CustodianDepartment = model.CustodianDepartment;
                data.CustDepartmentCode = model.CustDepartmentCode;
                data.CustDesignation = model.CustDesignation;
                data.CustodianName = model.CustodianName;
                data.CompletionDate = model.CompletionDate;
                data.AdminRemarks = model.AdminRemarks;

                // Use values from request (edit / complete)
                if (!string.IsNullOrWhiteSpace(model.Status))
                    data.Status = model.Status;

                if (!string.IsNullOrWhiteSpace(model.AuditStatus))
                    data.AuditStatus = model.AuditStatus;

                // When marking complete, enforce both fields if only one was sent
                if (string.Equals(data.Status, "Completed", StringComparison.OrdinalIgnoreCase))
                {
                    data.Status = "Completed";
                    data.AuditStatus = "Inactive";
                }

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

        // DELETE API — soft delete (record flag, not physical delete)
        [HttpDelete]
        [Route("api/DeleteAudit/{id}")]
        public IActionResult DeleteAudit(int id)
        {
            try
            {
                var data = _context.AuditMasters
                    .FirstOrDefault(x => x.AuditID == id);

                if (data == null)
                    return NotFound("Audit Not Found");

                data.Status = "InActive";
                data.AuditStatus = "Inactive";

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


        //[HttpGet]
        //[Route("api/ViewAudits/{auditId}")]
        //public async Task<IActionResult> ViewAudit(int auditId)
        //{
        //    try
        //    {
        //        // Get selected audit
        //        var audit = await _context.AuditMasters
        //            .FirstOrDefaultAsync(x => x.AuditID == auditId);

        //        if (audit == null)
        //        {
        //            return Ok(new List<object>());
        //        }

        //        // Get location name
        //        var location = await _context.LocationMasters
        //            .FirstOrDefaultAsync(x => x.LocationID == audit.LocationID);

        //        // Get assets
        //        var assets = await _assetSAPDBContext.AssetModels.ToListAsync();

        //        // Filter assets by audit location
        //        var result = assets
        //            .Where(x => x.Location == audit.LocationID.ToString())
        //            .Select(x => new
        //            {
        //                assetId = x.AssetID,
        //                mainAssetNumber = x.MainAssetNumber,
        //                assetDesc = x.AssetDesc,
        //                assetClass = x.AssetClass,
        //                assetType = x.AssetType,
        //                assetLocation = x.Location,
        //                locationName = location != null ? location.Location : "",
        //                auditName = audit.AuditName,
        //                auditBy = audit.AuditBy,
        //                auditedDate = audit.AuditDate,
        //                assetStatus = x.Status
        //            })
        //            .ToList();

        //        return Ok(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex.Message);
        //    }
        //}

        [HttpGet]
        [Route("api/ViewAudit/{auditId}")]
        public async Task<IActionResult> ViewAudit()
        {
            try
            {
                var locations = await _context.LocationMasters.ToListAsync();

                var assets = await _assetSAPDBContext.AssetModels.ToListAsync();

                var result = (
                    from asset in assets
                    join loc in locations
                    on asset.Location equals loc.LocationID.ToString()
                    select new
                    {
                        asset.AssetID,
                        asset.MainAssetNumber,
                        asset.AssetDesc,
                        asset.AssetClass,
                        asset.AssetType,
                        asset.Location,
                        //LocationName = loc.LocationName
                    }
                ).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



    }
}



