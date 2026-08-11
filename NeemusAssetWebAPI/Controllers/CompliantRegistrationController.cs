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
        public async Task<IActionResult> InsertTicketWithFile([FromForm] ComplaintRegistration complaint)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                complaint.CreatedDate = DateTime.UtcNow;
                complaint.Status = "Pending";

                if (complaint.File != null && complaint.File.Length > 0)
                {
                    var folderPath = Path.Combine(
                        Directory.GetCurrentDirectory(),
                        "complaintimages");

                    if (!Directory.Exists(folderPath))
                    {
                        Directory.CreateDirectory(folderPath);
                    }

                    var fileName = Guid.NewGuid().ToString() +
                                   Path.GetExtension(complaint.File.FileName);

                    var filePath = Path.Combine(folderPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await complaint.File.CopyToAsync(stream);
                    }

                    complaint.FilePath = "~/complaintimages/" + fileName;
                }
                else
                {
                    complaint.FilePath = "";
                }

                _context.ComplaintRegistrations.Add(complaint);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Ticket Raised Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
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
    }
}