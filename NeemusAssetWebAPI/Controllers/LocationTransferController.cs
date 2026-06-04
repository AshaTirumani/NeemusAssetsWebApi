using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class LocationTransferController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public LocationTransferController(
            PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/LocationTransferDetails")]
        public IActionResult GetLocationTransfers()
        {
            var data = _context.EmployeeLocationChanges
                               .OrderByDescending(x => x.LocationChangeID)
                               .ToList();

            return Ok(data);
        }

        [HttpPost]
        [Route("api/CreateLocationTransfer")]
        public IActionResult CreateLocationTransfer(
            [FromBody] LocationTransferRequestModel model)
        {
            try
            {
                EmployeeLocationChange obj =
                    new EmployeeLocationChange()
                    {
                        AssetID = model.AssetID,

                        AssetClassID =
                            model.AssetClassID,

                        LocationID =
                            model.LocationID,

                        ToLocation =
                            model.ToLocation,

                        CustodianComments =
                            model.CustodianComments,

                        EmployeeID =
                            model.EmployeeID,

                        RequestBy =
                            model.RequestBy,

                        CustodianDepartment =
                            model.CustodianDepartment,

                        CustDesignation =
                            model.CustDesignation,

                        ApproverID =
                            model.ApproverID,

                        RequestType =
                            "Location Transfer",

                        Status =
                            "Request Sent To Approver",

                        Date = DateTime.UtcNow
                    };

                _context.EmployeeLocationChanges.Add(obj);

                _context.SaveChanges();

                return Ok(new
                {
                    Message =
                        "Location Transfer Request Sent Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.ToString());
            }
        }
    
    [HttpPut]
        [Route("api/ApproveLocationTransfer")]
        public IActionResult ApproveLocationTransfer(
    [FromBody] EmployeeLocationChange model)
        {
            var data = _context.EmployeeLocationChanges
                               .FirstOrDefault(
                                   x => x.LocationChangeID ==
                                        model.LocationChangeID);

            if (data == null)
            {
                return NotFound();
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
                Message = "Approved Successfully"
            });
        }
        [HttpPut]
        [Route("api/RejectLocationTransfer")]
        public IActionResult RejectLocationTransfer(
    [FromBody] EmployeeLocationChange model)
        {
            var data = _context.EmployeeLocationChanges
                               .FirstOrDefault(
                                   x => x.LocationChangeID ==
                                        model.LocationChangeID);

            if (data == null)
            {
                return NotFound();
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
                Message = "Rejected Successfully"
            });
        }
    }

    }
