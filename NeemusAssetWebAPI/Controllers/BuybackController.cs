using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class BuybackController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public BuybackController(
            PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/BuybackDetails")]
        public IActionResult GetBuybackDetails()
        {
            var data = _context.EmployeeAssetBuybacks
                               .OrderByDescending(
                                   x => x.BuybackRequestID)
                               .ToList();

            return Ok(data);
        }

        [HttpPost]
        [Route("api/CreateBuyback")]
        public IActionResult CreateBuyback(
            [FromBody] EmployeeAssetBuyback model)
        {
            try
            {
                EmployeeAssetBuyback obj =
                    new EmployeeAssetBuyback()
                    {
                        AssetID =
                            model.AssetID,

                        AssetClassID =
                            model.AssetClassID,

                        EmployeeID =
                            model.EmployeeID,

                        RequestBy =
                            model.RequestBy,

                        Comments =
                            model.Comments,

                        CustodianDepartment =
                            model.CustodianDepartment,

                        CustDesignation =
                            model.CustDesignation,

                        ApproverID =
                            model.ApproverID,

                        Status =
                            "Request Sent To Admin",

                        RequestType =
                            "Asset Buyback",

                        Date =
                            DateTime.UtcNow
                    };

                _context.EmployeeAssetBuybacks.Add(obj);

                _context.SaveChanges();

                return Ok(new
                {
                    Message =
                        "Asset Buyback Request Sent Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpPut]
        [Route("api/AdminApproveBuyback")]
        public IActionResult AdminApproveBuyback(
    [FromBody] EmployeeAssetBuyback model)
        {
            var data =
                _context.EmployeeAssetBuybacks
                .FirstOrDefault(x =>
                    x.BuybackRequestID ==
                    model.BuybackRequestID);

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
                    "Asset Buyback Approved Successfully"
            });
        }

        [HttpPut]
        [Route("api/AdminRejectBuyback")]
        public IActionResult AdminRejectBuyback(
            [FromBody] EmployeeAssetBuyback model)
        {
            var data =
                _context.EmployeeAssetBuybacks
                .FirstOrDefault(x =>
                    x.BuybackRequestID ==
                    model.BuybackRequestID);

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
                    "Asset Buyback Rejected Successfully"
            });
        }
    }
}