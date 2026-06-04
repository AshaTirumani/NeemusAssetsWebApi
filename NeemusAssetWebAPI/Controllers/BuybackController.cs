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
                            "Request Sent To Approver",

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
    }
}