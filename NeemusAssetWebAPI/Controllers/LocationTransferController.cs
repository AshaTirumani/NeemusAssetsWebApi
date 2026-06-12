using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
using static NeemusAssetWebAPI.Models.CustodianChangeRequestModel;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class LocationTransferController : ControllerBase
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetContext;
        public LocationTransferController(
       PostgreDBContext context,
       AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
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

        //[HttpGet]
        //[Route("api/GetAssetTypesByClass/{assetClassID}")]
        //public IActionResult GetAssetTypesByClass(int assetClassID)
        //{
        //    var data = _assetContext.AssetTypeModels
        //        .Where(x => x.AssetClassID == assetClassID)
        //        .ToList();

        //    return Ok(data);
        //}
        // [HttpGet]
        // [Route("api/LocationTransferDetails")]
        // public IActionResult GetLocationTransfers()
        // {
        //     var data = _context.EmployeeLocationChanges
        //.OrderByDescending(x => x.LocationChangeID)
        //.ToList();

        //     return Ok(data);
        // }
        // //[HttpGet]
        // //[Route("api/GetAssetTypesByClass/{assetClassID}")]
        // //public IActionResult GetAssetTypesByClass(int assetClassID)
        // //{
        // //    try
        // //    {
        // //        var data = _assetContext.AssetTypeModels
        // //            .Where(x => x.AssetClassID == assetClassID)
        // //            .ToList();

        // //        return Ok(data);
        // //    }
        // //    catch (Exception ex)
        // //    {
        // //        return BadRequest(ex.ToString());
        // //    }
        // //}
        [HttpPost]
        [Route("api/CreateLocationTransfer")]
        public IActionResult CreateLocationTransfer(
     [FromBody] EmployeeLocationChange model)
        {
            try
            {
                EmployeeLocationChange obj =
                    new EmployeeLocationChange()
                    {
                        AssetID = model.AssetID,

                        AssetClassID = model.AssetClassID,

                        LocationID = model.LocationID,

                        ToLocation = model.ToLocation,

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

                        Date =
                            DateTime.UtcNow
                    };

                _context.EmployeeLocationChanges.Add(obj);

                _context.SaveChanges();

                return Ok(
                    "Location Transfer Request Sent Successfully"
                );
            }
            catch (Exception ex)
            {
                return BadRequest(
                    ex.InnerException?.Message ?? ex.Message
                );
            }
        }
        //[HttpGet]
        //[Route("api/LocationTransferDetails")]
        //public IActionResult GetLocationTransfers()
        //{
        //    var transfers = _context.EmployeeLocationChanges
        //        .OrderByDescending(x => x.LocationChangeID)
        //        .ToList();

        //    var assets = _assetContext.AssetModels.ToList();

        //    // ADD THIS HERE
        //    foreach (var a in assets)
        //    {
        //        Console.WriteLine(
        //            $"AssetID={a.AssetID} MainAsset={a.MainAssetNumber}"
        //        );
        //    }

        //    var data = transfers.Select(x =>
        //    {
        //        var asset = assets.FirstOrDefault(
        //            a => a.AssetID == x.AssetID
        //        );

        //        return new
        //        {
        //            x.LocationChangeID,
        //            x.AssetID,

        //            MainAssetNumber =
        //                asset?.MainAssetNumber,

        //            AssetSubNumber =
        //                asset?.AssetSubNumber,

        //            AssetTypeName =
        //                asset?.AssetType,

        //            AssetClassName =
        //                asset?.AssetClass
        //        };
        //    }).ToList();

        //    return Ok(data);
        //}
        //[HttpGet]
        //[Route("api/GetAssetsByClass/{assetClass}")]
        //public IActionResult GetAssetsByClass(string assetClass)
        //{
        //    var data = _assetContext.AssetModels
        //        .Where(x => x.AssetClass == assetClass)
        //        .ToList();

        //    return Ok(data);
        //}
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
    
    [HttpPut]
        [Route("api/AdminApproveLocationTransfer")]
        public IActionResult AdminApproveLocationTransfer(
    [FromBody] EmployeeLocationChange model)
        {
            var data =
                _context.EmployeeLocationChanges
                .FirstOrDefault(x =>
                    x.LocationChangeID ==
                    model.LocationChangeID);

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

            data.AdminDate =
                DateTime.UtcNow;

            data.Status =
                "Approved By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Location Transfer Approved By Admin"
            });
        }
        [HttpPut]
        [Route("api/AdminRejectLocationTransfer")]
        public IActionResult AdminRejectLocationTransfer(
    [FromBody] EmployeeLocationChange model)
        {
            var data =
                _context.EmployeeLocationChanges
                .FirstOrDefault(x =>
                    x.LocationChangeID ==
                    model.LocationChangeID);

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

            data.AdminDate =
                DateTime.UtcNow;

            data.Status =
                "Rejected By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Location Transfer Rejected By Admin"
            });
        }
        [HttpPut]
        [Route("api/AdminApproveCustodianTransfer")]
        public IActionResult AdminApproveCustodianTransfer(
    [FromBody] CustodianChangeRequest model)
        {
            var data =
                _context.CustodianChangeRequests
                .FirstOrDefault(x =>
                    x.CustodianChangeID ==
                    model.CustodianChangeID);

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

            data.AdminDate =
                DateTime.UtcNow;

            data.Status =
                "Approved By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Custodian Transfer Approved Successfully"
            });
        }

        [HttpPut]
        [Route("api/AdminRejectCustodianTransfer")]
        public IActionResult AdminRejectCustodianTransfer(
            [FromBody] CustodianChangeRequest model)
        {
            var data =
                _context.CustodianChangeRequests
                .FirstOrDefault(x =>
                    x.CustodianChangeID ==
                    model.CustodianChangeID);

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

            data.AdminDate =
                DateTime.UtcNow;

            data.Status =
                "Rejected By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Custodian Transfer Rejected Successfully"
            });
        }
        [HttpPut]
        [Route("api/AdminApproveAssetRequest")]
        public IActionResult AdminApproveAssetRequest(
    [FromBody] EmployeeAssetRequest model)
        {
            var data =
                _context.EmployeeAssetRequests
                .FirstOrDefault(x =>
                    x.AssetRequestID ==
                    model.AssetRequestID);

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

            data.AdminDate =
                DateTime.UtcNow;

            data.Status =
                "Approved By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Asset Request Approved Successfully"
            });
        }

        [HttpPut]
        [Route("api/AdminRejectAssetRequest")]
        public IActionResult AdminRejectAssetRequest(
            [FromBody] EmployeeAssetRequest model)
        {

            var data =
                _context.EmployeeAssetRequests
                .FirstOrDefault(x =>
                    x.AssetRequestID ==
                    model.AssetRequestID);

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

            data.AdminDate =
                DateTime.UtcNow;

            data.Status =
                "Rejected By Admin";

            _context.SaveChanges();

            return Ok(new
            {
                Message =
                    "Asset Request Rejected Successfully"
            });
        }
    }
    }
