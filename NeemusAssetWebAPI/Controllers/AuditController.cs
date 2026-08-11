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
    public class AuditController : ControllerBase
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

        //// GET API
        //[HttpGet]
        //[Route("api/AuditDetails")]
        //public IActionResult GetAudits()
        //{
        //    var data = _context.AuditMasters.ToList();
        //    return Ok(data);
        //}
        // GET API
        [HttpGet]
        [Route("api/AuditDetails")]
        public IActionResult GetAudits()
        {
            try
            {
                var audits = _context.AuditMasters.ToList();
                var auditDetails = _context.AuditDetailsModels.ToList();
                var locations = _context.LocationMasters.ToList();
                var assets = _assetSAPDBContext.AssetModels.ToList();

                var result =
                    from audit in audits
                    join detail in auditDetails
                        on audit.AuditID equals detail.AuditID
                    join asset in assets
                        on detail.AssetID equals asset.AssetID
                    join location in locations
                        on audit.LocationID equals location.LocationID
                        into loc
                    from location in loc.DefaultIfEmpty()
                    select new
                    {
                        auditId = audit.AuditID,
                        auditName = audit.AuditName,
                        auditBy = detail.AuditBy ?? audit.AuditBy,
                        mainAssetNumber = asset.MainAssetNumber,
                        assetClass = asset.AssetClass,
                        assetDescription = asset.AssetDesc,
                        location = detail.Location,
                        custodianID = detail.CustodianID,
                        auditStatus = detail.AuditStatus,
                        auditorComments = detail.Comments,
                        auditedDate = detail.Date,
                        status = detail.Status
                    };

                return Ok(result.OrderByDescending(x => x.auditedDate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
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

                return Ok(new
                {
                    message = "Updated Successfully"
                });
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

        // GET API - list of audits for the "Select Audit" dropdown
        [HttpGet]
        [Route("api/ViewAudits")]
        public IActionResult GetAuditsList()
        {
            try
            {
                var result = _context.AuditMasters
                    .Where(x => x.Status != "InActive")
                    .OrderByDescending(x => x.AuditDate)
                    .Select(x => new
                    {
                        auditId = x.AuditID,
                        auditName = x.AuditName,
                        auditDate = x.AuditDate
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
        [Route("api/AuditDetailsForEdit")]
        public IActionResult GetAuditsForEdit()
        {
            try
            {
                var audits = _context.AuditMasters.ToList();
                var auditDetails = _context.AuditDetailsModels.ToList();
                var locations = _context.LocationMasters.ToList();
                var assets = _assetSAPDBContext.AssetModels.ToList();

                var result =
                    from audit in audits
                    join detail in auditDetails
                        on audit.AuditID equals detail.AuditID
                    join asset in assets
                        on detail.AssetID equals asset.AssetID
                    join location in locations
                        on audit.LocationID equals location.LocationID
                        into loc
                    from location in loc.DefaultIfEmpty()
                    select new
                    {
                        auditDetailsID = detail.AuditDetailsID,
                        auditId = audit.AuditID,
                        auditName = audit.AuditName,
                        auditBy = detail.AuditBy ?? audit.AuditBy,
                        mainAssetNumber = asset.MainAssetNumber,
                        assetClass = asset.AssetClass,
                        assetDescription = asset.AssetDesc,
                        location = detail.Location,
                        custodianID = detail.CustodianID,
                        auditStatus = detail.AuditStatus,
                        auditorComments = detail.Comments,
                        auditedDate = detail.Date,
                        status = detail.Status
                    };

                return Ok(result.OrderByDescending(x => x.auditedDate));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpPut]
        [Route("api/UpdateAuditDetails")]
        public IActionResult UpdateAuditDetails([FromBody] AuditDetailsModel model)
        {
            try
            {
                if (model == null || model.AuditDetailsID <= 0)
                    return BadRequest("Invalid Audit Details");

                //var data = _context.AuditDetailsModels
                //    .FirstOrDefault(x => x.AuditDetailsID == model.AuditDetailsID);
                var data = _context.AuditDetailsModels
    .FirstOrDefault(x => x.AuditDetailsID == model.AuditDetailsID);

                if (data == null)
                    return BadRequest("Audit Details Not Found");

                data.AuditStatus = model.AuditStatus;
                data.Comments = model.Comments;
                data.Date = DateTime.Now;
                data.Status = model.Status;
                data.Location = model.Location;
                data.CustodianID = model.CustodianID;
                data.AuditBy = model.AuditBy;

                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Updated Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message
                });
            }
        }



        [HttpGet]
        [Route("api/ViewAudit/{auditId}")]
        public async Task<IActionResult> ViewAudit(int auditId)
        {
            try
            {
                if (auditId <= 0)
                    return BadRequest("Invalid Audit Id");

                var audit = await _context.AuditMasters
                    .FirstOrDefaultAsync(x => x.AuditID == auditId);

                if (audit == null)
                    return Ok(new List<object>());

                var auditDetails = await _context.AuditDetailsModels
                    .Where(x => x.AuditID == auditId)
                    .ToListAsync();

                var assets = await _assetSAPDBContext.AssetModels.ToListAsync();

                var result =
                    from detail in auditDetails
                    join asset in assets
                        on detail.AssetID equals asset.AssetID
                    select new
                    {
                        auditId = audit.AuditID,
                        auditName = audit.AuditName,
                        assetId = asset.AssetID,
                        mainAssetNumber = asset.MainAssetNumber,
                        assetDesc = asset.AssetDesc,
                        assetClass = asset.AssetClass,
                        location = detail.Location,
                        auditBy = detail.AuditBy ?? audit.AuditBy,
                        auditedDate = detail.Date,
                        auditStatus = detail.AuditStatus,
                        status = detail.Status
                    };

                return Ok(result.ToList());
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




        [HttpPost]
        [Route("api/InsertAuditDetails")]
        public async Task<IActionResult> InsertAuditDetails([FromBody] AuditDetailInsertRequest model)
        {
            try
            {
                if (model == null)
                    return BadRequest("Invalid audit detail data.");

                var existing = await _context.AuditDetailsModels
                    .FirstOrDefaultAsync(x =>
                        x.AssetID == model.AssetId &&
                        x.AuditID == model.AuditId);

                if (existing != null)
                {
                    existing.Date = DateTime.SpecifyKind(model.AuditDate, DateTimeKind.Unspecified);
                    existing.Location = model.UpdatedLocation;
                    existing.CustodianID = model.ChangedCustodian;
                    existing.Status = model.UpdatedStatus;
                    existing.Comments = model.Comments;
                    existing.AuditStatus = "Audited";
                    existing.AuditBy = "Auditor";

                    await _context.SaveChangesAsync();

                    return Ok(new
                    {
                        Message = "Audit detail updated successfully",
                        Data = existing
                    });
                }

                int nextId = await _context.AuditDetailsModels.AnyAsync()
                    ? await _context.AuditDetailsModels.MaxAsync(x => x.AuditDetailsID) + 1
                    : 1;

                var detail = new AuditDetailsModel
                {
                    AuditDetailsID = nextId,
                    AssetID = model.AssetId,
                    AuditID = model.AuditId,
                    MainAssetNumber = model.MainAssetNumber,
                    Location = model.UpdatedLocation,
                    CustodianID = model.ChangedCustodian,
                    Status = model.UpdatedStatus,
                    Comments = model.Comments,
                    Date = DateTime.SpecifyKind(model.AuditDate, DateTimeKind.Unspecified),
                    AuditStatus = "Audited",
                    AuditBy = "Auditor"
                };

                _context.AuditDetailsModels.Add(detail);

                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Audit detail inserted successfully",
                    Data = detail
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.InnerException?.Message ?? ex.Message,
                    Detail = ex.ToString()
                });
            }
        }


    

    [HttpPost]
        [Route("api/ApproveAuditAssets")]
        public async Task<IActionResult> ApproveAuditAssets([FromBody] AuditApprovalRequest request)
        {
            try
            {
                if (request == null || request.AuditId <= 0 || request.Assets == null || !request.Assets.Any())
                {
                    return BadRequest("Invalid approval request data.");
                }

                var assetIds = request.Assets.Select(a => a.AssetId).ToList();

                // 1. Update AuditMaster Remarks
                var audit = await _context.AuditMasters.FirstOrDefaultAsync(x => x.AuditID == request.AuditId);
                if (audit != null)
                {
                    audit.AdminRemarks = request.GlobalRemarks;
                    if (request.Action == "Approve")
                    {
                        audit.Status = "Completed";
                        audit.AuditStatus = "Inactive";
                    }
                }

                // 2. Fetch and Update AuditDetailsModels
                var auditDetailsList = await _context.AuditDetailsModels
                    .Where(x => x.AuditID == request.AuditId && x.AssetID.HasValue && assetIds.Contains(x.AssetID.Value))
                    .ToListAsync();

                foreach (var detail in auditDetailsList)
                {
                    var reqAsset = request.Assets.FirstOrDefault(a => a.AssetId == detail.AssetID);
                    detail.AuditStatus = request.Action == "Approve" ? "Approved" : "Rejected";
                    detail.AdminRemarks = reqAsset != null && !string.IsNullOrWhiteSpace(reqAsset.Remarks)
                                          ? reqAsset.Remarks
                                          : request.GlobalRemarks;
                }


                // 3. Apply changes to AssetModel if Approved
                if (request.Action == "Approve")
                {
                    var assetsToUpdate = await _assetSAPDBContext.AssetModels
                        .Where(a => assetIds.Contains(a.AssetID))
                        .ToListAsync();

                    int nextHistoryId = (_context.AssetAuditHistories.Any()
                        ? _context.AssetAuditHistories.Max(x => x.AuditHistoryID)
                        : 0) + 1;

                    foreach (var asset in assetsToUpdate)
                    {
                        var auditDetail = auditDetailsList.FirstOrDefault(d => d.AssetID == asset.AssetID);

                        if (auditDetail != null)
                        {
                            // Save old values for history
                            string oldLocation = asset.Location ?? string.Empty;
                            string oldStatus = asset.Status ?? string.Empty;
                            string oldCustodian = asset.CustodianID ?? string.Empty;

                            // Update Asset Master
                            if (!string.IsNullOrEmpty(auditDetail.Location))
                                asset.Location = auditDetail.Location;

                            if (!string.IsNullOrEmpty(auditDetail.Status))
                                asset.Status = auditDetail.Status;

                            if (!string.IsNullOrEmpty(auditDetail.CustodianID))
                                asset.CustodianID = auditDetail.CustodianID;

                            // Insert into AssetAuditHistory
                            var history = new AssetAuditHistory
                            {
                                AuditID = auditDetail.AuditID,
                                AssetID = auditDetail.AssetID,
                                MainAssetNumber = auditDetail.MainAssetNumber,

                                AssetLocation = oldLocation,
                                AssetCustodian = oldCustodian,
                                AssetStatus = oldStatus,

                                LocationChangedTo = auditDetail.Location,
                                CustodianChangedTo = auditDetail.CustodianID,
                                StatusChangedTo = auditDetail.Status,

                                AuditBy = auditDetail.AuditBy,
                                AuditorRemarks = auditDetail.Comments,
                                AuditedDate = auditDetail.Date,

                                ApprovedBy = "Admin", // Replace with logged-in user if available
                                ApproverRemarks = request.GlobalRemarks,
                                ApprovedDate = DateTime.Now,

                                AuditDetailsID = auditDetail.AuditDetailsID,
                                Status = "Approved",
                                AdminDate = DateTime.Now
                            };
                            // Generate AuditHistoryID manually using the pre-calculated and incremented counter
                            history.AuditHistoryID = nextHistoryId++;

                            _context.AssetAuditHistories.Add(history);
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await _assetSAPDBContext.SaveChangesAsync();

                return Ok(new { Message = $"Assets successfully {request.Action.ToLower()}ed." });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

    }
}

//public class AssetApprovalData
//{
//    public int AssetId { get; set; }
//    public string Remarks { get; set; }
//}

//public class AuditApprovalRequest
//{
//    public int AuditId { get; set; }
//    public List<AssetApprovalData> Assets { get; set; }
//    public string GlobalRemarks { get; set; }
//    public string Action { get; set; } // "Approve" or "Reject"
//}

//public class AuditDetailInsertRequest
//{
//    public int AssetId { get; set; }
//    public int AuditId { get; set; }
//    public string? AuditName { get; set; }
//    public string? Location { get; set; }
//    public string? AssetClass { get; set; }
//    public string? MainAssetNumber { get; set; }
//    public string? UpdatedLocation { get; set; }
//    public string? ChangedCustodian { get; set; }
//    public string? UpdatedStatus { get; set; }
//    public string? Comments { get; set; }
//    public DateTime AuditDate { get; set; }
//}





