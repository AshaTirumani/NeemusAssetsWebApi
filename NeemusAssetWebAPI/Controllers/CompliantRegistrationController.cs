using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComplaintRegistrationController : ControllerBase
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetContext;

        public ComplaintRegistrationController(
            PostgreDBContext context,
            AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
        }

        // =========================================================
        // GET ALL COMPLAINTS
        // GET: /api/ComplaintRegistration
        // =========================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintRegistration>>>
            GetComplaintRegistration()
        {
            var data = await _context.ComplaintRegistrations
                .OrderByDescending(x => x.ComplaintID)
                .ToListAsync();

            return Ok(data);
        }


        // =========================================================
        // GET COMPLAINT BY ID
        // GET: /api/ComplaintRegistration/5
        // =========================================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintRegistration>>
            GetComplaintRegistration(int id)
        {
            var complaint = await _context.ComplaintRegistrations
                .FindAsync(id);

            if (complaint == null)
            {
                return NotFound(new
                {
                    message = "Complaint not found"
                });
            }

            return Ok(complaint);
        }
        ///  insert 
        [HttpPost]
        public async Task<ActionResult<ComplaintRegistration>> PostComplaintRegistration(ComplaintRegistration complaint)
        {
            try
            {
                complaint.CreatedDate = DateTime.UtcNow;
                complaint.Status = "Pending";

                _context.ComplaintRegistrations.Add(complaint);
                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetComplaintRegistration),
                    new { id = complaint.ComplaintID },
                    complaint);
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
        [HttpPost("InsertTicketWithFile")]
        public async Task<IActionResult> InsertTicketWithFile(
     [FromForm] ComplaintRegistration complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                // =====================================================
                // 1. Set Complaint Registration values
                // =====================================================

                complaint.CreatedDate = DateTime.UtcNow;
                complaint.Status = "Pending";

                // =====================================================
                // 2. Save uploaded file
                // =====================================================

                if (complaint.File != null && complaint.File.Length > 0)
                {
                    var folderPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "complaintimages");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var fileName = Guid.NewGuid().ToString()
                                    + Path.GetExtension(complaint.File.FileName);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(
                        filePath,
                        FileMode.Create))
                    {
                        await complaint.File.CopyToAsync(stream);
                    }

                    complaint.FilePath = "~/complaintimages/" + fileName;
                }
                else
                {
                    complaint.FilePath = "";
                }

                // =====================================================
                // 3. Insert ComplaintRegistration
                // =====================================================

                _context.ComplaintRegistrations.Add(complaint);

                await _context.SaveChangesAsync();

                int complaintId = complaint.ComplaintID;

                // =====================================================
                // GET NEXT SEQUENCE FOR THIS USER
                // =====================================================

                int nextSequence = 1;

                if (!string.IsNullOrEmpty(complaint.EmployeeID))
                {
                    var lastSequence = await _context.ComplaintTransactions
                        .Where(x => x.USR_ID == complaint.EmployeeID)
                        .Select(x => x.Sequence)
                        .ToListAsync();

                    var sequenceNumbers = lastSequence
                        .Select(x =>
                        {
                            if (int.TryParse(x, out int number))
                                return number;

                            return 0;
                        })
                        .ToList();

                    if (sequenceNumbers.Any())
                    {
                        nextSequence = sequenceNumbers.Max() + 1;
                    }
                }

                // =====================================================
                // INSERT COMPLAINT TRANSACTION
                // =====================================================

                var complaintTransaction = new ComplaintTransaction
                {
                    ComplaintID = complaintId,

                    Status = "Pending",

                    USR_ID = complaint.EmployeeID,

                    Remarks = complaint.Complaint_Description,

                    CreatedDate = complaint.CreatedDate,

                    Comments = complaint.Comments,

                    FileDocument = complaint.FileDocument,

                    ComplaintType = complaint.ComplaintType,

                    // IMPORTANT
                    Sequence = nextSequence.ToString(),

                    ApproverComments = complaint.ApproverComments
                };



                _context.ComplaintTransactions.Add(complaintTransaction);

                await _context.SaveChangesAsync();

                // =====================================================
                // 5. Commit both inserts
                // =====================================================

                await transaction.CommitAsync();

                return Ok(new
                {
                    message = "Ticket Raised Successfully",

                    complaintID = complaint.ComplaintID,

                    complaintTransactionID =
                        complaintTransaction.ComplaintTransactionID
                });
            }
            catch (DbUpdateException ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message = ex.InnerException?.Message ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();

                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }

        // =========================================================
        // TICKET STATUS
        // GET:
        // /api/ComplaintRegistration/TicketStatusDetails/151
        // =========================================================

        [HttpGet("TicketStatusDetails/{custodianId}")]
        public async Task<IActionResult> GetTicketStatusDetails(string custodianId)
        {
            try
            {
                Console.WriteLine($"CustodianID: {custodianId}");

                var result = await (
                    from cr in _context.ComplaintRegistrations

                    join ct in _context.ComplaintTransactions
                        on cr.ComplaintID equals ct.ComplaintID

                    join st in _context.ServiceTypeModels
                        on cr.ServiceTypeID equals st.ServiceTypeID
                        into serviceJoin

                    from st in serviceJoin.DefaultIfEmpty()

                    where cr.EmployeeID == custodianId
                          && cr.AssetID != null

                    orderby ct.CreatedDate descending

                    select new
                    {
                        userName = cr.EmployeeName,

                        ticketSequence =
                            ct.USR_ID + "__C0" + ct.Sequence,

                        serviceType =
                            st != null
                                ? st.ServiceTypeName
                                : "",

                        ticketDescription =
                            cr.Complaint_Description,

                        engineerRemarks =
                            ct.Comments,

                        status =
                            ct.Status,

                        ticketPriority =
                            cr.ComplainPriority,

                        ticketDate =
                            ct.CreatedDate,

                        solvedDate =
                            ct.ProgressDate
                    }
                ).ToListAsync();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }

        // =========================================================
        // GENERAL TICKETS
        // GET:
        // /api/ComplaintRegistration/GeneralTicketDetails/151
        // =========================================================

        [HttpGet("GeneralTicketDetails/{custodianId}")]
        public async Task<IActionResult>
            GetGeneralTicketDetails(string custodianId)
        {
            try
            {
                Console.WriteLine(
                    $"GeneralTicketDetails CustodianID = {custodianId}"
                );

                // -------------------------------------------------
                // Get complaints
                // EmployeeID = CustodianID
                // -------------------------------------------------

                var complaints =
                    await _context.ComplaintRegistrations
                        .Where(cr =>
                            cr.EmployeeID == custodianId &&
                            cr.AssetID == null)
                        .ToListAsync();

                if (!complaints.Any())
                {
                    return Ok(new List<object>());
                }

                var complaintIds = complaints
                    .Select(x => x.ComplaintID)
                    .ToList();

                // -------------------------------------------------
                // Get transactions
                // -------------------------------------------------

                var transactions =
                    await _context.ComplaintTransactions
                        .Where(ct =>
                            ct.ComplaintID != null &&
                            complaintIds.Contains(ct.ComplaintID.Value))
                        .ToListAsync();

                // -------------------------------------------------
                // Get service types
                // -------------------------------------------------

                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID != null)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();

                var serviceTypes =
                    await _context.ServiceTypeModels
                        .Where(st =>
                            serviceTypeIds.Contains(st.ServiceTypeID))
                        .ToListAsync();

                // -------------------------------------------------
                // Build result
                // (ordering moved to the final projection — ordering
                // the "transactions" list before the join had no
                // effect, since LINQ joins follow the outer sequence's
                // order, not the inner one)
                // -------------------------------------------------

                var result = (
                    from cr in complaints

                    join ct in transactions
                        on cr.ComplaintID equals ct.ComplaintID

                    join st in serviceTypes
                        on cr.ServiceTypeID equals st.ServiceTypeID
                        into serviceJoin

                    from st in serviceJoin.DefaultIfEmpty()

                    select new
                    {
                        userName =
                            cr.EmployeeName,

                        ticketSequence =
                            ct.USR_ID + "__C0" + ct.Sequence,

                        serviceType =
                            st != null
                                ? st.ServiceTypeName
                                : "",

                        ticketDescription =
                            cr.Complaint_Description,

                        engineerRemarks =
                            ct.Comments,

                        ticketPriority =
                            cr.ComplainPriority,

                        status =
                            ct.Status,

                        ticketDate =
                            ct.CreatedDate,

                        solvedDate =
                            ct.ProgressDate
                    }
                )
                .OrderByDescending(x => x.ticketDate)
                .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = "Error fetching general tickets",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        [HttpGet("ViewAllDepartmentTickets/{custodianId}")]
        public async Task<IActionResult> ViewAllDepartmentTickets(string custodianId)
        {
            try
            {
                // =====================================================
                // 1. FIND LOGGED-IN EMPLOYEE
                // =====================================================

                var user = await _context.EmployeeMasters
                    .FirstOrDefaultAsync(x =>
                        x.CustodianID == custodianId &&
                        x.CustodianStatus == "Active");

                if (user == null)
                {
                    return NotFound(new
                    {
                        message = "Employee not found"
                    });
                }


                // =====================================================
                // 2. CHECK APPROVER ROLE
                // =====================================================

                var isApprover = await _context.RoleMasterModels
                    .AnyAsync(x =>
                        x.CustodianID == custodianId &&
                        x.ROLE_NAME == "Approver" &&
                        x.ROLE_STATUS == "Active");

                if (!isApprover)
                {
                    return Unauthorized(new
                    {
                        message = "User is not an Approver"
                    });
                }


                // =====================================================
                // 3. GET DEPARTMENT
                // =====================================================

                var department = user.CustodianDepartmentCode;

                if (string.IsNullOrWhiteSpace(department))
                {
                    return BadRequest(new
                    {
                        message = "Department not found"
                    });
                }


                // =====================================================
                // 4. GET DEPARTMENT COMPLAINTS
                // =====================================================

                var complaints = await _context.ComplaintRegistrations
                    .Where(x =>
                        x.EmployeeDepartment == department)
                    .OrderByDescending(x => x.ComplaintID)
                    .ToListAsync();


                // =====================================================
                // 5. GET COMPLAINT TRANSACTIONS
                // =====================================================

                var complaintIds = complaints
                    .Select(x => x.ComplaintID)
                    .ToList();

                var transactions = await _context.ComplaintTransactions
                    .Where(x =>
                        x.ComplaintID != null &&
                        complaintIds.Contains(x.ComplaintID.Value))
                    .ToListAsync();


                // =====================================================
                // 6. GET ASSETS FROM ASSET MASTER
                // =====================================================

                var assetIds = complaints
                    .Where(x => x.AssetID != null)
                    .Select(x => x.AssetID)
                    .Distinct()
                    .ToList();

                var assets = await _assetContext.AssetModels
                    .Where(x => assetIds.Contains(x.AssetID))
                    .Select(x => new
                    {
                        x.AssetID,
                        x.MainAssetNumber,
                        x.AssetDesc
                    })
                    .ToListAsync();
                // =====================================================
                // GET SERVICE TYPES
                // =====================================================

                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID != null)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();

                var serviceTypes = await _context.ServiceTypeModels
                    .Where(x => serviceTypeIds.Contains(x.ServiceTypeID))
                    .Select(x => new
                    {
                        x.ServiceTypeID,
                        x.ServiceTypeName
                    })
                    .ToListAsync();


                // =====================================================
                // 7. BUILD FINAL RESULT
                // =====================================================

                var tickets =
                    (from cr in complaints

                     join ct in transactions
                         on cr.ComplaintID equals ct.ComplaintID

                     join asset in assets
                         on cr.AssetID equals asset.AssetID
                         into assetJoin

                     from asset in assetJoin.DefaultIfEmpty()

                     join st in serviceTypes
                         on cr.ServiceTypeID equals st.ServiceTypeID
                         into serviceTypeJoin

                     from st in serviceTypeJoin.DefaultIfEmpty()

                     select new
                     {
                         complaintTransactionID =
                      ct.ComplaintTransactionID,
                         // ==========================================
                         // TICKET ID
                         // ==========================================

                         TicketID =
                             ct.USR_ID + "_CO" + ct.Sequence,


                         // ==========================================
                         // ASSET DETAILS
                         // ==========================================

                         assetNumber =
                             asset != null
                                 ? asset.MainAssetNumber
                                 : "",

                         assetname =
                             asset != null
                                 ? asset.AssetDesc
                                 : "",


                         // ==========================================
                         // EMPLOYEE DETAILS
                         // ==========================================

                         EmployeeName =
                             cr.EmployeeName,

                         EmployeeID =
                             cr.EmployeeID,


                         // ==========================================
                         // STATUS
                         // ==========================================

                         status =
                             ct.Status,


                         // ==========================================
                         // DATES
                         // ==========================================

                         createdDate =
                             ct.CreatedDate,

                         AssignedDate =
                             ct.AssignedDate,

                         ResolvedOrWorkinProgressDate =
                             ct.ProgressDate,


                         // ==========================================
                         // SERVICE TYPE
                         // FROM COMPLAINT REGISTRATION
                         // ==========================================

                         ServiceType =
    st != null
        ? st.ServiceTypeName
        : "",


                         // ==========================================
                         // TICKET DESCRIPTION
                         // ==========================================

                         TicketDescription =
                             cr.Complaint_Description,


                         // ==========================================
                         // TIME TAKEN
                         // ==========================================

                         timeTaken = ""
                     })
                    .ToList();


                // =====================================================
                // 8. RETURN RESPONSE
                // =====================================================

                return Ok(new
                {
                    department = department,
                    tickets = tickets
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        // ============================================================
        // GET CLOSED TICKETS FOR LOGGED-IN USER
        //
        // GET:
        // api/ComplaintTransaction/UserClosedTickets/100
        //
        // 100 = EmployeeID / CustodianID
        // ============================================================

        [HttpGet("UserClosedTickets/{userId}")]
        public async Task<IActionResult> GetUserClosedTickets(string userId)
        {
            try
            {
               

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(new
                    {
                        message = "User ID is required."
                    });
                }


                

                var transactions = await _context.ComplaintTransactions
                    .Where(x =>
                        x.USR_ID == userId &&
                        x.Status == "Closed"
                    )
                    .OrderByDescending(x => x.ProgressDate)
                    .ToListAsync();


                if (!transactions.Any())
                {
                    return Ok(new List<object>());
                }


             

                var complaintIds = transactions
                    .Where(x => x.ComplaintID.HasValue)
                    .Select(x => x.ComplaintID!.Value)
                    .Distinct()
                    .ToList();


                
                var complaints = await _context.ComplaintRegistrations
                    .Where(x =>
                        complaintIds.Contains(x.ComplaintID))
                    .ToListAsync();


              
                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID.HasValue)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();


                

                var serviceTypes = await _context.ServiceTypeModels
                    .Where(x =>
                        serviceTypeIds.Contains(x.ServiceTypeID))
                    .ToListAsync();


              

                var assetIds = complaints
                    .Where(x => x.AssetID.HasValue)
                    .Select(x => x.AssetID!.Value)
                    .Distinct()
                    .ToList();


             

                var assets = await _assetContext.AssetModels
                    .Where(x => assetIds.Contains(x.AssetID))
                    .Select(x => new
                    {
                        x.AssetID,
                        x.MainAssetNumber,
                        x.AssetDesc
                    })
                    .ToListAsync();


              

                var result =
                    (from ct in transactions

                     join cr in complaints
                         on ct.ComplaintID equals cr.ComplaintID

                     join st in serviceTypes
                         on cr.ServiceTypeID equals st.ServiceTypeID
                         into serviceJoin

                     from st in serviceJoin.DefaultIfEmpty()

                     join asset in assets
                         on cr.AssetID equals asset.AssetID
                         into assetJoin

                     from asset in assetJoin.DefaultIfEmpty()

                     select new
                     {
                        

                         complaintTransactionID =
                             ct.ComplaintTransactionID,


                         

                         employeeName =
                             cr.EmployeeName,

                         employeeID =
                             cr.EmployeeID,


                         

                         ticketSequence =
                             ct.USR_ID + "_CO" + ct.Sequence,


                        

                         serviceType =
                             st != null
                                 ? st.ServiceTypeName
                                 : "",


                         
                         assetNumber =
                             asset != null
                                 ? asset.MainAssetNumber
                                 : "",

                         assetDescription =
                             asset != null
                                 ? asset.AssetDesc
                                 : "",


                        

                         ticketDescription =
                             cr.Complaint_Description,


                        

                         engineerRemarks =
                             ct.Comments,


                         

                         ticketStatus =
                             ct.Status,


                        
                         ticketDate =
                             ct.CreatedDate,

                         solvedDate =
                             ct.ProgressDate,

                         ticketClosedDate =
                             ct.ProgressDate,


                       

                         assignedEngineer =
                             ct.AssignedTo,


                       

                         timeTaken = ""
                     })
                    .OrderByDescending(x => x.ticketClosedDate)
                    .ToList();


              

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = "Error fetching user closed tickets.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
    }
    }
    
    
    


    
