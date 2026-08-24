using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintTransactionController : ControllerBase
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetContext;

        public ComplaintTransactionController(
     PostgreDBContext context,
     AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
        }


        // =====================================================
        // GET ALL COMPLAINT TRANSACTIONS
        // GET: api/ComplaintTransaction
        // =====================================================

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ComplaintTransaction>>>
            GetComplaintTransactions()
        {
            return await _context.ComplaintTransactions
                .OrderByDescending(x => x.ComplaintTransactionID)
                .ToListAsync();
        }


        // =====================================================
        // GET COMPLAINT TRANSACTION BY ID
        // GET: api/ComplaintTransaction/5
        // =====================================================

        [HttpGet("{id}")]
        public async Task<ActionResult<ComplaintTransaction>>
            GetComplaintTransaction(int id)
        {
            var transaction =
                await _context.ComplaintTransactions
                    .FirstOrDefaultAsync(x =>
                        x.ComplaintTransactionID == id);

            if (transaction == null)
            {
                return NotFound(new
                {
                    message = "Complaint transaction not found."
                });
            }

            return Ok(transaction);
        }


        // =====================================================
        // GET TRANSACTIONS BY COMPLAINT ID
        // GET: api/ComplaintTransaction/Complaint/10
        // =====================================================

        [HttpGet("Complaint/{complaintId}")]
        public async Task<ActionResult<IEnumerable<ComplaintTransaction>>>
            GetByComplaintId(int complaintId)
        {
            var transactions =
                await _context.ComplaintTransactions
                    .Where(x =>
                        x.ComplaintID == complaintId)
                    .OrderBy(x => x.CreatedDate)
                    .ToListAsync();

            return Ok(transactions);
        }


        // =====================================================
        // POST COMPLAINT TRANSACTION
        // POST: api/ComplaintTransaction
        // =====================================================

        [HttpPost]
        public async Task<ActionResult<ComplaintTransaction>>
            PostComplaintTransaction(
                ComplaintTransaction transaction)
        {
            try
            {
                // Set created date if not supplied
                transaction.CreatedDate ??=
                    DateTime.UtcNow;

                _context.ComplaintTransactions.Add(
                    transaction);

                await _context.SaveChangesAsync();

                return CreatedAtAction(
                    nameof(GetComplaintTransaction),
                    new
                    {
                        id =
                            transaction.ComplaintTransactionID
                    },
                    transaction
                );
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message =
                        ex.InnerException?.Message
                        ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message = ex.Message
                });
            }
        }


        // =====================================================
        // UPDATE COMPLAINT TRANSACTION
        // PUT: api/ComplaintTransaction/5
        // =====================================================

        [HttpPut("{id}")]
        public async Task<IActionResult>
            PutComplaintTransaction(
                int id,
                ComplaintTransaction transaction)
        {
            if (id != transaction.ComplaintTransactionID)
            {
                return BadRequest(new
                {
                    message =
                        "ComplaintTransactionID does not match the URL."
                });
            }

            _context.Entry(transaction).State =
                EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                var exists =
                    await _context.ComplaintTransactions
                        .AnyAsync(x =>
                            x.ComplaintTransactionID == id);

                if (!exists)
                {
                    return NotFound(new
                    {
                        message =
                            "Complaint transaction not found."
                    });
                }

                throw;
            }

            return NoContent();
        }


        // =====================================================
        // DELETE COMPLAINT TRANSACTION
        // DELETE: api/ComplaintTransaction/5
        // =====================================================

        [HttpDelete("{id}")]
        public async Task<IActionResult>
            DeleteComplaintTransaction(int id)
        {
            var transaction =
                await _context.ComplaintTransactions
                    .FirstOrDefaultAsync(x =>
                        x.ComplaintTransactionID == id);

            if (transaction == null)
            {
                return NotFound(new
                {
                    message =
                        "Complaint transaction not found."
                });
            }

            _context.ComplaintTransactions.Remove(
                transaction);

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // =====================================================
        // ASSIGN TICKET TO ENGINEER
        //
        // PUT:
        // api/ComplaintTransaction/AssignTicket/5
        // =====================================================

        [HttpPut("AssignTicket/{id}")]
        public async Task<IActionResult>
            AssignTicket(
                int id,
                [FromBody] AssignTicketRequest request)
        {
            try
            {
              

                if (request == null)
                {
                    return BadRequest(new
                    {
                        message =
                            "Request data is required."
                    });
                }


               

                var transaction =
                    await _context.ComplaintTransactions
                        .FirstOrDefaultAsync(x =>
                            x.ComplaintTransactionID == id);

                if (transaction == null)
                {
                    return NotFound(new
                    {
                        message =
                            "Ticket transaction not found."
                    });
                }


              

                if (request.AssignedTo <= 0)
                {
                    return BadRequest(new
                    {
                        message =
                            "Please select a valid engineer."
                    });
                }



                transaction.AssignedTo =
                    request.AssignedTo;


               

                transaction.ApproverComments =
                    request.ApproverComments;


               
                transaction.AssignedDate =
                    DateTime.UtcNow;


               

                transaction.Status =
                    "Assign";


               

                await _context.SaveChangesAsync();


               
                return Ok(new
                {
                    message =
                        "Ticket assigned successfully.",

                    complaintTransactionID =
                        transaction.ComplaintTransactionID,

                    assignedTo =
                        transaction.AssignedTo,

                    approverComments =
                        transaction.ApproverComments,

                    assignedDate =
                        transaction.AssignedDate,

                    status =
                        transaction.Status
                });
            }
            catch (DbUpdateException ex)
            {
                return BadRequest(new
                {
                    message =
                        ex.InnerException?.Message
                        ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    message =
                        ex.InnerException?.Message
                        ?? ex.Message
                });
            }
        }
        // =====================================================
        // GET ASSIGNED TICKETS FOR ENGINEER
        //
        // GET:
        // api/ComplaintTransaction/EngineerTickets/155
        //
        // 155 = Engineer User ID / Custodian ID
        // =====================================================

        [HttpGet("EngineerTickets/{userId}")]
        public async Task<IActionResult> GetEngineerTickets(
     string userId,
     [FromQuery] string? status = null)
        {
            try
            {
               

                if (string.IsNullOrWhiteSpace(userId)) 
                {
                    return BadRequest(new
                    {
                        message = "Engineer User ID is required."
                    });
                }

                // AssignedTo in ComplaintTransaction is int?
                if (!int.TryParse(userId.Trim(), out int engineerId))
                {
                    return BadRequest(new
                    {
                        message = "Invalid Engineer User ID."
                    });
                }


               

                var query = _context.ComplaintTransactions
      .Where(x =>
          x.AssignedTo.HasValue &&
          x.AssignedTo.Value == engineerId
      );


             

                if (!string.IsNullOrWhiteSpace(status))
                {
                    query = query.Where(x =>
                        x.Status == status
                    );
                }


                var transactions = await query
                    .OrderByDescending(x => x.AssignedDate)
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
                    .Where(x => complaintIds.Contains(x.ComplaintID))
                    .ToListAsync();


             

                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID.HasValue)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();



                var serviceTypes = await _context.ServiceTypeModels
                    .Where(x => serviceTypeIds.Contains(x.ServiceTypeID))
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


                      

                         ticketID =
                             ct.USR_ID + "_CO" + ct.Sequence,


                      

                         assetNumber =
                             asset != null
                                 ? asset.MainAssetNumber
                                 : "",

                         assetName =
                             asset != null
                                 ? asset.AssetDesc
                                 : "",


                      

                         employeeID =
                             cr.EmployeeID,

                         employeeName =
                             cr.EmployeeName,


                     

                         serviceType =
                             st != null
                                 ? st.ServiceTypeName
                                 : "",



                         ticketDescription =
                             cr.Complaint_Description,


                    

                         status =
                             ct.Status,


                      

                         assignedTo =
                             ct.AssignedTo,



                         approverComments =
                             ct.ApproverComments,


                       

                         createdDate =
                             ct.CreatedDate,

                         assignedDate =
                             ct.AssignedDate,

                         resolvedOrWorkinProgressDate =
                             ct.ProgressDate,


                      

                         engineerComments =
                             ct.Comments,



                         timeTaken =
                             ""
                     })
                    .OrderByDescending(x => x.assignedDate)
                    .ToList();


               

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = "Error fetching engineer assigned tickets.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }


        // ============================================================
        // UPDATE ENGINEER TICKET
        // ============================================================
        // PUT:
        // api/ComplaintTransaction/SolveTicket/4
        //
        // 4 = ComplaintTransactionID
        // ============================================================

      

        [HttpPut("SolveTicket/{id}")]
        public async Task<IActionResult> SolveTicket(
            int id,
            [FromBody] SolveTicketRequest request)
        {
            using var dbTransaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
           

                if (request == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request is required."
                    });
                }


                if (string.IsNullOrWhiteSpace(request.Status))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Status is required."
                    });
                }


               

                var transaction =
                    await _context.ComplaintTransactions
                        .FirstOrDefaultAsync(x =>
                            x.ComplaintTransactionID == id);


                if (transaction == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message =
                            $"Complaint transaction {id} not found."
                    });
                }


              

                if (!transaction.ComplaintID.HasValue)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message =
                            "Complaint ID is not available for this transaction."
                    });
                }


                int complaintId =
                    transaction.ComplaintID.Value;



                var complaint =
                    await _context.ComplaintRegistrations
                        .FirstOrDefaultAsync(x =>
                            x.ComplaintID == complaintId);


                if (complaint == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message =
                            $"Complaint registration {complaintId} not found."
                    });
                }


              

                transaction.Status =
                    request.Status;

                transaction.Comments =
                    request.Comments?.Trim();

                transaction.ProgressDate =
                    DateTime.UtcNow;



                complaint.Status =
                    request.Status;


           

                await _context.SaveChangesAsync();


              

                await dbTransaction.CommitAsync();



                return Ok(new
                {
                    success = true,

                    message =
                        "Ticket updated successfully.",

                    complaintTransactionID =
                        transaction.ComplaintTransactionID,

                    complaintID =
                        transaction.ComplaintID,

                    transactionStatus =
                        transaction.Status,

                    complaintStatus =
                        complaint.Status,

                    comments =
                        transaction.Comments,

                    progressDate =
                        transaction.ProgressDate
                });
            }
            catch (DbUpdateException ex)
            {
                await dbTransaction.RollbackAsync();

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while updating ticket.",
                    error =
                        ex.InnerException?.Message
                        ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while updating ticket.",
                    error = ex.Message,
                    innerException =
                        ex.InnerException?.Message
                });
            }
        }
        // =====================================================
        // GET RESOLVED TICKETS FOR USER
        //
        // GET:
        // api/ComplaintTransaction/UserResolvedTickets/213594
        //
        // 213594 = User / Employee ID
        // =====================================================

        [HttpGet("UserResolvedTickets/{userId}")]
        public async Task<IActionResult> GetUserResolvedTickets(string userId)
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
                        x.Status == "Resolved"
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

                         userName =
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


                         

                         status =
                             ct.Status,


                        

                         ticketDate =
                             ct.CreatedDate,


                        

                         solvedDate =
                             ct.ProgressDate,


                        

                         assignedEngineer =
                             ct.AssignedTo
                     })
                    .OrderByDescending(x => x.solvedDate)
                    .ToList();


                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = "Error fetching user resolved tickets.",
                    error = ex.Message,
                    innerException = ex.InnerException?.Message
                });
            }
        }
        // ============================================================
        // CLOSE / RE-OPEN USER TICKET
        //
        // PUT:
        // api/ComplaintTransaction/UserTicketAction/{transactionId}
        //
        // Example:
        // api/ComplaintTransaction/UserTicketAction/13
        // ============================================================

      

        [HttpPut("UserTicketAction/{transactionId}")]
        public async Task<IActionResult> UserTicketAction(
            int transactionId,
            [FromBody] UserTicketActionRequest request)
        {
            using var dbTransaction =
                await _context.Database.BeginTransactionAsync();

            try
            {
               

                if (request == null)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Request is required."
                    });
                }


                if (string.IsNullOrWhiteSpace(request.Status))
                {
                    return BadRequest(new
                    {
                        success = false,
                        message = "Status is required."
                    });
                }



                if (
                    request.Status != "Re_open" &&
                    request.Status != "Closed"
                )
                {
                    return BadRequest(new
                    {
                        success = false,
                        message =
                            "Invalid status. Only Re_open or Closed is allowed."
                    });
                }



                var transaction =
                    await _context.ComplaintTransactions
                        .FirstOrDefaultAsync(x =>
                            x.ComplaintTransactionID == transactionId);


                if (transaction == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message =
                            $"Complaint transaction {transactionId} not found."
                    });
                }


            

                if (!transaction.ComplaintID.HasValue)
                {
                    return BadRequest(new
                    {
                        success = false,
                        message =
                            "Complaint ID is not available for this transaction."
                    });
                }


                int complaintId =
                    transaction.ComplaintID.Value;



                var complaint =
                    await _context.ComplaintRegistrations
                        .FirstOrDefaultAsync(x =>
                            x.ComplaintID == complaintId);


                if (complaint == null)
                {
                    return NotFound(new
                    {
                        success = false,
                        message =
                            $"Complaint registration {complaintId} not found."
                    });
                }


            

                transaction.Status =
                    request.Status;

                transaction.Comments =
                    request.Comments?.Trim();


           

                complaint.Status =
                    request.Status;



                await _context.SaveChangesAsync();



                await dbTransaction.CommitAsync();


              
                return Ok(new
                {
                    success = true,

                    message =
                        request.Status == "Re_open"
                            ? "Ticket re-opened successfully."
                            : "Ticket closed successfully.",

                    complaintTransactionID =
                        transaction.ComplaintTransactionID,

                    complaintID =
                        transaction.ComplaintID,

                    transactionStatus =
                        transaction.Status,

                    complaintStatus =
                        complaint.Status,

                    comments =
                        transaction.Comments
                });
            }
            catch (DbUpdateException ex)
            {
                await dbTransaction.RollbackAsync();

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while updating ticket.",
                    error =
                        ex.InnerException?.Message
                        ?? ex.Message
                });
            }
            catch (Exception ex)
            {
                await dbTransaction.RollbackAsync();

                return StatusCode(500, new
                {
                    success = false,
                    message = "Error while updating ticket.",
                    error = ex.Message,
                    innerException =
                        ex.InnerException?.Message
                });
            }
        }
        // ============================================================
        // GET CLOSED TICKETS FOR USER
        //
        // GET:
        // api/ComplaintTransaction/UserClosedTickets/213594
        //
        // 213594 = User / Employee ID
        // ============================================================

        [HttpGet("UserClosedTickets/{userId}")]
        public async Task<IActionResult> GetUserClosedTickets(string userId)
        {
            try
            {
                // ========================================================
                // 1. VALIDATE USER ID
                // ========================================================

                if (string.IsNullOrWhiteSpace(userId))
                {
                    return BadRequest(new
                    {
                        message = "User ID is required."
                    });
                }


                // ========================================================
                // 2. GET CLOSED TRANSACTIONS FOR THIS USER
                // ========================================================

                var transactions = await _context.ComplaintTransactions
                    .Where(x =>
                        x.USR_ID == userId &&
                        x.Status == "Closed"
                    )
                    .OrderByDescending(x => x.ProgressDate)
                    .ToListAsync();


                // ========================================================
                // 3. NO CLOSED TICKETS
                // ========================================================

                if (!transactions.Any())
                {
                    return Ok(new List<object>());
                }


                // ========================================================
                // 4. GET COMPLAINT IDS
                // ========================================================

                var complaintIds = transactions
                    .Where(x => x.ComplaintID.HasValue)
                    .Select(x => x.ComplaintID!.Value)
                    .Distinct()
                    .ToList();


                // ========================================================
                // 5. GET COMPLAINT REGISTRATIONS
                // ========================================================

                var complaints = await _context.ComplaintRegistrations
                    .Where(x => complaintIds.Contains(x.ComplaintID))
                    .ToListAsync();


                // ========================================================
                // 6. GET SERVICE TYPE IDS
                // ========================================================

                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID.HasValue)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();


                // ========================================================
                // 7. GET SERVICE TYPES
                // ========================================================

                var serviceTypes = await _context.ServiceTypeModels
                    .Where(x => serviceTypeIds.Contains(x.ServiceTypeID))
                    .ToListAsync();


                // ========================================================
                // 8. GET ASSET IDS
                // ========================================================

                var assetIds = complaints
                    .Where(x => x.AssetID.HasValue)
                    .Select(x => x.AssetID!.Value)
                    .Distinct()
                    .ToList();


                // ========================================================
                // 9. GET ASSETS
                // ========================================================

                var assets = await _assetContext.AssetModels
                    .Where(x => assetIds.Contains(x.AssetID))
                    .Select(x => new
                    {
                        x.AssetID,
                        x.MainAssetNumber,
                        x.AssetDesc
                    })
                    .ToListAsync();


                // ========================================================
                // 10. BUILD RESULT
                // ========================================================

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

                         assetClass =
                             "",

                         assetDescription =
                             asset != null
                                 ? asset.AssetDesc
                                 : "",

                         engineerRemarks =
                             ct.Comments,

                         ticketDescription =
                             cr.Complaint_Description,

                         ticketDate =
                             ct.CreatedDate,

                         solvedDate =
                             ct.ProgressDate,

                         ticketClosedDate =
                             ct.ProgressDate,

                         timeTaken =
                             "",

                         ticketStatus =
                             ct.Status,

                         assignedEngineer =
                             ct.AssignedTo
                     })
                    .OrderByDescending(x => x.ticketClosedDate)
                    .ToList();


                // ========================================================
                // 11. RETURN RESULT
                // ========================================================

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



        // ============================================================
        // GET ALL RESOLVED + CLOSED TICKETS FOR ADMIN
        //


        // ============================================================
        // GET COMPLAINT TRANSACTIONS BY STATUS
        //
        // Examples:
        // GET: api/ComplaintTransaction/ByStatus?status=Closed
        // GET: api/ComplaintTransaction/ByStatus?status=Resolved
        // ============================================================

        [HttpGet("ByStatus")]
        public async Task<IActionResult> GetTicketsByStatus(
            [FromQuery] string status)
        {
            try
            {
                // Validate status
                if (string.IsNullOrWhiteSpace(status))
                {
                    return BadRequest(new
                    {
                        message = "Status is required."
                    });
                }

                // Get transactions based on status
                var transactions = await _context.ComplaintTransactions
                    .Where(x => x.Status == status)
                    .OrderByDescending(x => x.ProgressDate)
                    .ToListAsync();

                // No records
                if (!transactions.Any())
                {
                    return Ok(new List<object>());
                }

                // Get Complaint IDs
                var complaintIds = transactions
                    .Where(x => x.ComplaintID.HasValue)
                    .Select(x => x.ComplaintID!.Value)
                    .Distinct()
                    .ToList();

                // Get complaints
                var complaints = await _context.ComplaintRegistrations
                    .Where(x => complaintIds.Contains(x.ComplaintID))
                    .ToListAsync();

                // Get Service Type IDs
                var serviceTypeIds = complaints
                    .Where(x => x.ServiceTypeID.HasValue)
                    .Select(x => x.ServiceTypeID!.Value)
                    .Distinct()
                    .ToList();

                // Get Service Types
                var serviceTypes = await _context.ServiceTypeModels
                    .Where(x => serviceTypeIds.Contains(x.ServiceTypeID))
                    .ToListAsync();

                // Get Asset IDs
                var assetIds = complaints
                    .Where(x => x.AssetID.HasValue)
                    .Select(x => x.AssetID!.Value)
                    .Distinct()
                    .ToList();

                // Get Assets from SAP DB
                var assets = await _assetContext.AssetModels
                    .Where(x => assetIds.Contains(x.AssetID))
                    .Select(x => new
                    {
                        x.AssetID,
                        x.MainAssetNumber,
                        x.AssetDesc
                    })
                    .ToListAsync();

                // Build result
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

                         ticketId =
                             ct.USR_ID + "_C" + ct.Sequence,

                         ticketSequence =
                             ct.USR_ID + "_CO" + ct.Sequence,

                         employeeName =
                             cr.EmployeeName,

                         employeeID =
                             cr.EmployeeID,

                         serviceTypeName =
                             st != null
                                 ? st.ServiceTypeName
                                 : "",

                         assetNumber =
                             asset != null
                                 ? asset.MainAssetNumber
                                 : "",

                         assetName =
                             asset != null
                                 ? asset.AssetDesc
                                 : "",

                         ticketDescription =
                             cr.Complaint_Description,

                         engineerRemarks =
                             ct.Comments,

                         assignedEngineer =
                             ct.AssignedTo,

                         status =
                             ct.Status,

                         createdDate =
                             ct.CreatedDate,

                         resolvedDate =
                             ct.ProgressDate,

                         ticketClosedDate =
                             ct.Status == "Closed"
                                 ? ct.ProgressDate
                                 : null,

                         typeOfTicket =
                             "",

                         timeTaken =
                             ""
                     })
                    .OrderByDescending(x => x.createdDate)
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                return StatusCode(500, new
                {
                    message = "Error fetching tickets.",
                    error = ex.Message,
                    innerException =
                        ex.InnerException?.Message
                });
            }
        }
    }
    }
    
    
   
        
    
