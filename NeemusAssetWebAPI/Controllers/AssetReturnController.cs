using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetReturnController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public AssetReturnController(
            PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/AssetReturnDetails")]
        public IActionResult GetAssetReturnDetails()
        {
            var data = _context.EmployeeAssetReturns
                               .OrderByDescending(
                                   x => x.AssetReturnID)
                               .ToList();

            return Ok(data);
        }

        [HttpPost]
        [Route("api/CreateAssetReturn")]
        public IActionResult CreateAssetReturn(
            [FromBody] EmployeeAssetReturn model)
        {
            try
            {
                EmployeeAssetReturn obj =
                    new EmployeeAssetReturn()
                    {
                        AssetID =
                            model.AssetID,

                        AssetClassID =
                            model.AssetClassID,

                        EmployeeID =
                            model.EmployeeID,

                        RequestBy =
                            model.RequestBy,

                        ToLocation =
                            model.ToLocation,

                        ToCustodian =
                            model.ToCustodian,

                        CustodianComments =
                            model.CustodianComments,

                        CustodianDepartment =
                            model.CustodianDepartment,

                        CustDesignation =
                            model.CustDesignation,

                        ApproverID =
                            model.ApproverID,

                        Status =
                            "Request Sent To Admin",

                        RequestType =
                            "Asset Return",

                        Date =
                            DateTime.UtcNow
                    };

                _context.EmployeeAssetReturns.Add(obj);

                _context.SaveChanges();

                return Ok(new
                {
                    Message =
                        "Asset Return Request Sent Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.InnerException?.Message ??
                    ex.Message
                );
            }
        }
        [HttpPut]
        [Route("api/AdminApproveAssetReturn")]
        public IActionResult AdminApproveAssetReturn(
    [FromBody] EmployeeAssetReturn model)
        {
            var data =
                _context.EmployeeAssetReturns
                .FirstOrDefault(x =>
                    x.AssetReturnID ==
                    model.AssetReturnID);

            if (data == null)
            {
                return NotFound();
            }

            data.AdminID =
                model.AdminID;

            data.AdminName =
                model.AdminName;

            data.AdminComments =
                model.AdminComments;

            data.Status =
                "Approved By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Asset Return Approved Successfully"
            });
        }

        [HttpPut]
        [Route("api/AdminRejectAssetReturn")]
        public IActionResult AdminRejectAssetReturn(
            [FromBody] EmployeeAssetReturn model)
        {
            var data =
                _context.EmployeeAssetReturns
                .FirstOrDefault(x =>
                    x.AssetReturnID ==
                    model.AssetReturnID);

            if (data == null)
            {
                return NotFound();
            }

            data.AdminID =
                model.AdminID;

            data.AdminName =
                model.AdminName;

            data.AdminComments =
                model.AdminComments;

            data.Status =
                "Rejected By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Asset Return Rejected Successfully"
            });
        }
    }
}