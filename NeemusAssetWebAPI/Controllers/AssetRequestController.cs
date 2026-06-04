using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetRequestController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public AssetRequestController(PostgreDBContext context)
        {
            _context = context;
        }

        // GET ALL REQUESTS

        [HttpGet]
        [Route("api/AssetRequestDetails")]
        public IActionResult GetAssetRequests()
        {
            var data = _context.EmployeeAssetRequests
                               .OrderByDescending(x => x.AssetRequestID)
                               .ToList();

            return Ok(data);
        }

        // GET REQUESTS BY EMPLOYEE

        [HttpGet]
        [Route("api/AssetRequestDetails/{employeeId}")]
        public IActionResult GetAssetRequestsByEmployee(string employeeId)
        {
            var data = _context.EmployeeAssetRequests
                               .Where(x => x.EmployeeID == employeeId)
                               .OrderByDescending(x => x.AssetRequestID)
                               .ToList();

            return Ok(data);
        }

        // INSERT REQUEST

        [HttpPost]
        [Route("api/CreateAssetRequest")]
        public IActionResult CreateAssetRequest(
            [FromBody] EmployeeAssetRequest model)
        {
            try
            {
                EmployeeAssetRequest obj =
                    new EmployeeAssetRequest()
                    {
                        AssetTypeID = model.AssetTypeID,
                        AssetClassID = model.AssetClassID,
                        RequestBy = model.RequestBy,
                        EmployeeID = model.EmployeeID,
                        Location = model.Location,
                        Quantity = model.Quantity,
                        LocationID = model.LocationID,
                        CustodianDepartment =
                            model.CustodianDepartment,

                        CustDesignation =
                            model.CustDesignation,

                        RequestType = "Asset Request",

                        Status = "Request Sent To Approver",

                        ApproverID = model.ApproverID,

                        Date = DateTime.UtcNow
                    };

                _context.EmployeeAssetRequests.Add(obj);

                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Request Sent Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // UPDATE REQUEST

        [HttpPut]
        [Route("api/UpdateAssetRequest")]
        public IActionResult UpdateAssetRequest(
            [FromBody] EmployeeAssetRequest model)
        {
            var data = _context.EmployeeAssetRequests
                               .FirstOrDefault(
                                   x => x.AssetRequestID ==
                                        model.AssetRequestID);

            if (data == null)
            {
                return NotFound();
            }

            data.AssetClassID = model.AssetClassID;
            data.AssetTypeID = model.AssetTypeID;
            data.Location = model.Location;
            data.Quantity = model.Quantity;

            _context.SaveChanges();

            return Ok("Updated Successfully");
        }

        // APPROVE REQUEST

        [HttpPut]
        [Route("api/ApproveAssetRequest")]
        public IActionResult ApproveAssetRequest(
            [FromBody] EmployeeAssetRequest model)
        {
            var data = _context.EmployeeAssetRequests
                               .FirstOrDefault(
                                   x => x.AssetRequestID ==
                                        model.AssetRequestID);

            if (data == null)
            {
                return NotFound("Request Not Found");
            }

            data.ApproverID =
                model.ApproverID;

            data.ApproverName =
                model.ApproverName;

            data.ApproverComments =
                model.ApproverComments;

            data.Status =
                "Request Sent To Admin";

            _context.SaveChanges();

            return Ok(new
            {
                message = "Approved Successfully"
            });
        }


        // REJECT REQUEST

        [HttpPut]
        [Route("api/RejectAssetRequest")]
        public IActionResult RejectAssetRequest(
            [FromBody] EmployeeAssetRequest model)
        {
            var data = _context.EmployeeAssetRequests
                               .FirstOrDefault(
                                   x => x.AssetRequestID ==
                                        model.AssetRequestID);

            if (data == null)
            {
                return NotFound("Request Not Found");
            }

            data.ApproverID =
                model.ApproverID;

            data.ApproverName =
                model.ApproverName;

            data.ApproverComments =
                model.ApproverComments;

            data.Status =
                "Rejected By Approver";

            _context.SaveChanges();

            return Ok(new
            {
                message = "Rejected Successfully"
            });
        }
        // DELETE REQUEST

        [HttpDelete]
        [Route("api/DeleteAssetRequest/{id}")]
        public IActionResult DeleteAssetRequest(int id)
        {
            var data = _context.EmployeeAssetRequests
                               .FirstOrDefault(
                                   x => x.AssetRequestID == id);

            if (data == null)
            {
                return NotFound();
            }

            _context.EmployeeAssetRequests.Remove(data);

            _context.SaveChanges();

            return Ok("Deleted Successfully");
        }
    }
}