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
        private readonly AssetSAPDBContext _assetContext;
        public ServiceTypeController(PostgreDBContext context,
             AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
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
    



      [HttpGet]
        [Route("api/ServiceTypeTickets")]
        public async Task<IActionResult> GetServiceTypeTickets(
            [FromQuery] int? serviceTypeId)
        {
            try
            {
                // ====================================================
                // 1. GET COMPLAINT TRANSACTIONS
                // ====================================================

                var transactions = await _context.ComplaintTransactions
                    .OrderByDescending(x => x.CreatedDate)
                    .ToListAsync();


                if (!transactions.Any())
                {
                    return Ok(new List<object>());
                }


                // ====================================================
                // 2. GET COMPLAINT IDS
                // ====================================================

                var complaintIds = transactions
                    .Where(x => x.ComplaintID.HasValue)
                    .Select(x => x.ComplaintID!.Value)
                    .Distinct()
                    .ToList();


                if (!complaintIds.Any())
                {
                    return Ok(new List<object>());
                }


                // ====================================================
                // 3. GET COMPLAINT REGISTRATIONS
                // ====================================================

                var complaints = await _context.ComplaintRegistrations
                    .Where(x =>
                        complaintIds.Contains(x.ComplaintID))
                    .ToListAsync();


                // ====================================================
                // 4. FILTER BY SERVICE TYPE
                // ====================================================

                if (serviceTypeId.HasValue)
                {
                    complaints = complaints
                        .Where(x =>
                            x.ServiceTypeID.HasValue &&
                            x.ServiceTypeID.Value ==
                            serviceTypeId.Value)
                        .ToList();
                }


                if (!complaints.Any())
                {
                    return Ok(new List<object>());
                }


                // ====================================================
                // 5. GET VALID COMPLAINT IDS
                // ====================================================

                var filteredComplaintIds = complaints
                    .Select(x => x.ComplaintID)
                    .Distinct()
                    .ToList();


                // ====================================================
                // 6. FILTER TRANSACTIONS
                // ====================================================

                transactions = transactions
                    .Where(x =>
                        x.ComplaintID.HasValue &&
                        filteredComplaintIds.Contains(
                            x.ComplaintID.Value))
                    .ToList();


                if (!transactions.Any())
                {
                    return Ok(new List<object>());
                }


                // ====================================================
                // 7. GET SERVICE TYPES
                // ====================================================

                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID.HasValue)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();


                var serviceTypes =
                    await _context.ServiceTypeModels
                        .Where(x =>
                            serviceTypeIds.Contains(
                                x.ServiceTypeID))
                        .ToListAsync();


                // ====================================================
                // 8. GET ASSET IDS
                // ====================================================

                var assetIds = complaints
                    .Where(x => x.AssetID.HasValue)
                    .Select(x => x.AssetID!.Value)
                    .Distinct()
                    .ToList();


                var assets =
                    await _assetContext.AssetModels
                        .Where(x =>
                            assetIds.Contains(x.AssetID))
                        .Select(x => new
                        {
                            x.AssetID,
                            x.MainAssetNumber,
                            x.AssetDesc
                        })
                        .ToListAsync();


                // ====================================================
                // 9. BUILD RESULT
                // ====================================================

                var result =
                    (from ct in transactions

                     join cr in complaints
                         on ct.ComplaintID
                         equals cr.ComplaintID

                     join st in serviceTypes
                         on cr.ServiceTypeID
                         equals st.ServiceTypeID
                         into serviceJoin

                     from st in serviceJoin.DefaultIfEmpty()

                     join asset in assets
                         on cr.AssetID
                         equals asset.AssetID
                         into assetJoin

                     from asset in assetJoin.DefaultIfEmpty()

                     select new
                     {
                         // =========================================
                         // TICKET ID
                         // =========================================

                         ticketId =
                             ct.USR_ID +
                             "_CO" +
                             ct.Sequence,


                         complaintTransactionID =
                             ct.ComplaintTransactionID,


                         // =========================================
                         // ASSET
                         // =========================================

                         assetNumber =
                             asset != null
                                 ? asset.MainAssetNumber
                                 : "",

                         assetName =
                             asset != null
                                 ? asset.AssetDesc
                                 : "",


                         // =========================================
                         // EMPLOYEE
                         // =========================================

                         employeeId =
                             cr.EmployeeID,

                         employeeName =
                             cr.EmployeeName,


                         // =========================================
                         // SERVICE TYPE
                         // =========================================

                         serviceTypeId =
                             cr.ServiceTypeID,

                         serviceType =
                             st != null
                                 ? st.ServiceTypeName
                                 : "",


                         // =========================================
                         // APPROVER
                         // =========================================

                         //approveId =
                         //    ct.ApproverID,

                         //approveName =
                         //    "",


                         // =========================================
                         // ENGINEER
                         // =========================================

                         assignedEngineerId =
                             ct.AssignedTo,

                         assignedEngineerName =
                             "",


                         // =========================================
                         // STATUS
                         // =========================================

                         status =
                             ct.Status,


                         // =========================================
                         // DESCRIPTION
                         // =========================================

                         ticketDescription =
                             cr.Complaint_Description,


                         engineerRemarks =
                             ct.Comments,


                         // =========================================
                         // DATES
                         // =========================================

                         createdDate =
                             ct.CreatedDate,

                         resolvedDate =
                             ct.ProgressDate
                     })
                    .OrderByDescending(x => x.createdDate)
                    .ToList();


                return Ok(result);
            }
            catch (Exception ex)
            {
                return StatusCode(
                    500,
                    new
                    {
                        message =
                            "Error fetching service type tickets.",

                        error =
                            ex.Message,

                        innerException =
                            ex.InnerException?.Message
                    });
            }
        }
    }
}

