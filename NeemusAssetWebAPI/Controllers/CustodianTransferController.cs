using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
using static NeemusAssetWebAPI.Models.CustodianChangeRequestModel;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class CustodianTransferController : Controller
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetContext;

        public CustodianTransferController(
            PostgreDBContext context,
            AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
        }

        // =========================================
        // GET ALL CUSTODIAN TRANSFER REQUESTS
        // =========================================

        [HttpGet]
        [Route("api/GetCustodianTransferRequests")]
        public IActionResult GetCustodianTransferRequests()
        {
            try
            {
                var data = _context.CustodianChangeRequests
                                   .OrderByDescending(x => x.CustodianChangeID)
                                   .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================================
        // GET ASSET CLASSES
        // =========================================

        [HttpGet]
        [Route("api/GetAssetClasses")]
        public IActionResult GetAssetClasses()
        {
            try
            {
                var data = _assetContext.AssetClasss
                                        .Where(x => x.Status == "Active")
                                        .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================================
        // GET ASSETS BY ASSET CLASS
        // =========================================


        [HttpGet]
        [Route("api/GetAssetTypesByClass/{assetClassID}")]
        public IActionResult GetAssetTypesByClass(int assetClassID)
        {
            try
            {
                var data = _assetContext.AssetTypeModels
                                        .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // =========================================
        // INSERT CUSTODIAN TRANSFER REQUEST
        // =========================================

        [HttpPost]
        [Route("api/InsertCustodianTransfer")]
        public IActionResult InsertCustodianTransfer([FromBody] CustodianChangeRequest model)
        {
            try
            {
                var obj = new CustodianChangeRequest()
                {
                    AssetID = model.AssetID,
                    EmployeeID = model.EmployeeID,
                    RequestBy = model.RequestBy,
                    CustodianComments = model.CustodianComments,
                    AssetClassID = model.AssetClassID,

                    Date = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Utc),

                    CustodianDepartment = model.CustodianDepartment,
                    CustDesignation = model.CustDesignation,
                    RequestType = "Custodian Transfer",
                    RequestedChangeCustodian = model.RequestedChangeCustodian
                };
                // APPROVER LOGIC

                if (string.IsNullOrEmpty(model.ApproverID))
                {
                    obj.ApproverID = "ADMIN";
                    obj.Status = "Request Sent To Admin";
                }
                else
                {
                    obj.ApproverID = model.ApproverID;
                    obj.Status = "Request Sent To Approver";
                }

                _context.CustodianChangeRequests.Add(obj);

                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Custodian Transfer Request Sent Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        // =========================================
        // DELETE CUSTODIAN TRANSFER REQUEST
        // =========================================

        [HttpDelete]
        [Route("api/DeleteCustodianTransfer/{id}")]
        public IActionResult DeleteCustodianTransfer(int id)
        {
            try
            {
                var data = _context.CustodianChangeRequests
                                   .FirstOrDefault(x => x.CustodianChangeID == id);

                if (data == null)
                {
                    return NotFound("Record Not Found");
                }

                _context.CustodianChangeRequests.Remove(data);

                _context.SaveChanges();

                return Ok("Deleted Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/ApproveCustodianTransfer")]
        public IActionResult ApproveCustodianTransfer(
        [FromBody] CustodianChangeRequest model)
        {
            var data =
                _context.CustodianChangeRequests
                .FirstOrDefault(
                    x => x.CustodianChangeID ==
                         model.CustodianChangeID);

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
                Message =
                    "Approved Successfully"
            });
        }
        [HttpPut]
        [Route("api/RejectCustodianTransfer")]
        public IActionResult RejectCustodianTransfer(
    [FromBody] CustodianChangeRequest model)
        {
            var data =
                _context.CustodianChangeRequests
                .FirstOrDefault(
                    x => x.CustodianChangeID ==
                         model.CustodianChangeID);

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
                Message =
                    "Rejected Successfully"
            });
        }
    }
    }