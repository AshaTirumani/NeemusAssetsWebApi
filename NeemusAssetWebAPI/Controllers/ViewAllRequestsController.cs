using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ViewAllRequestsController : ControllerBase
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetContext;

        public ViewAllRequestsController(
     PostgreDBContext context,
     AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
        }

        [HttpGet]
        [Route("api/ViewAllRequests")]
        public IActionResult ViewAllRequests()
        {
            var result = new List<object>();

            // Asset Requests
            result.AddRange(
                _context.EmployeeAssetRequests
                .Select(x => new
                {
                    RequestID = x.AssetRequestID,
                    RequestType = "Asset Request",
                    EmployeeID = x.EmployeeID,
                    RequestBy = x.RequestBy,
                    Department = x.CustodianDepartment,
                    Designation = x.CustDesignation,
                    Status = x.Status,
                    Date = x.Date
                })
            );

            // Asset Buyback
            result.AddRange(
                _context.EmployeeAssetBuybacks
                .Select(x => new
                {
                    RequestID = x.BuybackRequestID,
                    RequestType = "Asset Buyback",
                    EmployeeID = x.EmployeeID,
                    RequestBy = x.RequestBy,
                    Department = x.CustodianDepartment,
                    Designation = x.CustDesignation,
                    Status = x.Status,
                    Date = x.Date
                })
            );

            // Asset Return
            result.AddRange(
                _context.EmployeeAssetReturns
                .Select(x => new
                {
                    RequestID = x.AssetReturnID,
                    RequestType = "Asset Return",
                    EmployeeID = x.EmployeeID,
                    RequestBy = x.RequestBy,
                    Department = x.CustodianDepartment,
                    Designation = x.CustDesignation,
                    Status = x.Status,
                    Date = x.Date
                })
            );

            // Location Transfer
            result.AddRange(
                _context.EmployeeLocationChanges
                .Select(x => new
                {
                    RequestID = x.LocationChangeID,
                    RequestType = "Location Transfer",
                    EmployeeID = x.EmployeeID,
                    RequestBy = x.RequestBy,
                    Department = x.CustodianDepartment,
                    Designation = x.CustDesignation,
                    Status = x.Status,
                    Date = x.Date
                })
            );

            // Custodian Transfer
            result.AddRange(
                _context.CustodianChangeRequests
                .Select(x => new
                {
                    RequestID = x.CustodianChangeID,
                    RequestType = "Custodian Transfer",
                    EmployeeID = x.EmployeeID,
                    RequestBy = x.RequestBy,
                    Department = x.CustodianDepartment,
                    Designation = x.CustDesignation,
                    Status = x.Status,
                    Date = x.Date
                })
            );

            var finalResult =
                result.OrderByDescending(
                    x => ((dynamic)x).Date
                );

            return Ok(finalResult);
        }
        [HttpGet]
        [Route("api/ViewRequestDetails/{requestType}/{requestID}")]
        public IActionResult ViewRequestDetails(
         string requestType,
         int requestID)
        {
            switch (requestType)
            {
                // ==========================
                // ASSET REQUEST
                // ==========================

                case "Asset Request":

                    var assetRequest =
                        _context.EmployeeAssetRequests
                        .FirstOrDefault(
                            x => x.AssetRequestID ==
                                 requestID);

                    if (assetRequest == null)
                        return NotFound();

                  

                    return Ok(new
                    {
                        RequestType = "Asset Request",

                        EmployeeID =
                            assetRequest.EmployeeID,

                        RequestBy =
                            assetRequest.RequestBy,

                        Department =
                            assetRequest.CustodianDepartment,

                        Designation =
                            assetRequest.CustDesignation,

                        Quantity =
                            assetRequest.Quantity,

                        RequestDate =
                            assetRequest.Date,

                        ApproverID =
                            assetRequest.ApproverID,

                        ApproverName =
                            assetRequest.ApproverName,

                        ApproverDepartment =
                            assetRequest.ApproverDepartment,

                        ApproverDesignation =
                            assetRequest.ApproverDesignation,

                        ApproverComments =
                            assetRequest.ApproverComments,

                        AdminComments =
                            assetRequest.AdminComments,

                        Status =
                            assetRequest.Status,
                        AssetName = "",
                        MainAssetNumber = "",
                        AssetSubNumber = "",
                        AssetClass = "",
                        AssetStatus = "",
                        CurrentLocation = "",
                        Component = "",
                        FirstAcquisitionDate = (DateTime?)null,
                        CapitalizationDate = (DateTime?)null
                    });

                // ==========================
                // ASSET BUYBACK
                // ==========================

                case "Asset Buyback":

                    var buyback =
                        _context.EmployeeAssetBuybacks
                        .FirstOrDefault(
                            x => x.BuybackRequestID ==
                                 requestID);

                    if (buyback == null)
                        return NotFound();

                    var buybackAsset =
                        _assetContext.AssetModels
                        .FirstOrDefault(
                            x => x.AssetID ==
                                 buyback.AssetID);

                    return Ok(new
                    {
                        RequestType = "Asset Buyback",

                        EmployeeID =
                            buyback.EmployeeID,

                        RequestBy =
                            buyback.RequestBy,

                        Department =
                            buyback.CustodianDepartment,

                        Designation =
                            buyback.CustDesignation,

                        Comments =
                            buyback.Comments,

                        RequestDate =
                            buyback.Date,

                        ApproverID =
                            buyback.ApproverID,

                        ApproverName =
                            buyback.ApproverName,

                        ApproverDepartment =
                            buyback.ApproverDepartment,

                        ApproverDesignation =
                            buyback.ApproverDesignation,

                        ApproverComments =
                            buyback.ApproverComments,

                        AdminComments =
                            buyback.AdminComments,

                        Status =
                            buyback.Status,

                        AssetName =
                            buybackAsset?.AssetDesc,

                        MainAssetNumber =
                            buybackAsset?.MainAssetNumber,

                        AssetSubNumber =
                            buybackAsset?.AssetSubNumber,

                        AssetClass =
                            buybackAsset?.AssetClass,

                        AssetStatus =
                            buybackAsset?.Status,

                        CurrentLocation =
                            buybackAsset?.Location,

                        Component =
                            buybackAsset?.Component,

                        FirstAcquisitionDate =
                            buybackAsset?.FirstAcquisitionDate,

                        CapitalizationDate =
                            buybackAsset?.AssetCapitalizationDate
                    });

                // ==========================
                // ASSET RETURN
                // ==========================

                case "Asset Return":

                    var assetReturn =
                        _context.EmployeeAssetReturns
                        .FirstOrDefault(
                            x => x.AssetReturnID ==
                                 requestID);

                    if (assetReturn == null)
                        return NotFound();

                    var returnAsset =
                        _assetContext.AssetModels
                        .FirstOrDefault(
                            x => x.AssetID ==
                                 assetReturn.AssetID);

                    return Ok(new
                    {
                        RequestType = "Asset Return",

                        EmployeeID =
                            assetReturn.EmployeeID,

                        RequestBy =
                            assetReturn.RequestBy,

                        Department =
                            assetReturn.CustodianDepartment,

                        Designation =
                            assetReturn.CustDesignation,

                        CustodianComments =
                            assetReturn.CustodianComments,

                        RequestDate =
                            assetReturn.Date,

                        ApproverID =
                            assetReturn.ApproverID,

                        ApproverName =
                            assetReturn.ApproverName,

                        ApproverDepartment =
                            assetReturn.ApproverDepartment,

                        ApproverDesignation =
                            assetReturn.ApproverDesignation,

                        ApproverComments =
                            assetReturn.ApproverComments,

                        AdminComments =
                            assetReturn.AdminComments,

                        Status =
                            assetReturn.Status,

                        AssetName =
                            returnAsset?.AssetDesc,

                        MainAssetNumber =
                            returnAsset?.MainAssetNumber,

                        AssetSubNumber =
                            returnAsset?.AssetSubNumber,

                        AssetClass =
                            returnAsset?.AssetClass,

                        AssetStatus =
                            returnAsset?.Status,

                        CurrentLocation =
                            returnAsset?.Location,

                        Component =
                            returnAsset?.Component,

                        FirstAcquisitionDate =
                            returnAsset?.FirstAcquisitionDate,

                        CapitalizationDate =
                            returnAsset?.AssetCapitalizationDate
                    });

                // ==========================
                // LOCATION TRANSFER
                // ==========================

                case "Location Transfer":

                    var location =
                        _context.EmployeeLocationChanges
                        .FirstOrDefault(
                            x => x.LocationChangeID ==
                                 requestID);

                    if (location == null)
                        return NotFound();

                    var locationAsset =
                        _assetContext.AssetModels
                        .FirstOrDefault(
                            x => x.AssetID ==
                                 location.AssetID);

                    return Ok(new
                    {
                        RequestType = "Location Transfer",

                        EmployeeID =
                            location.EmployeeID,

                        RequestBy =
                            location.RequestBy,

                        Department =
                            location.CustodianDepartment,

                        Designation =
                            location.CustDesignation,

                        ToLocation =
                            location.ToLocation,

                        CustodianComments =
                            location.CustodianComments,

                        RequestDate =
                            location.Date,

                        ApproverID =
                            location.ApproverID,

                        ApproverName =
                            location.ApproverName,

                        ApproverDepartment =
                            location.ApproverDepartment,

                        ApproverDesignation =
                            location.ApproverDesignation,

                        ApproverComments =
                            location.ApproverComments,

                        AdminComments =
                            location.AdminComments,

                        Status =
                            location.Status,

                        AssetName =
                            locationAsset?.AssetDesc,

                        MainAssetNumber =
                            locationAsset?.MainAssetNumber,

                        AssetSubNumber =
                            locationAsset?.AssetSubNumber,

                        AssetClass =
                            locationAsset?.AssetClass,

                        AssetStatus =
                            locationAsset?.Status,

                        CurrentLocation =
                            locationAsset?.Location,

                        Component =
                            locationAsset?.Component,

                        FirstAcquisitionDate =
                            locationAsset?.FirstAcquisitionDate,

                        CapitalizationDate =
                            locationAsset?.AssetCapitalizationDate
                    });

                // ==========================
                // CUSTODIAN TRANSFER
                // ==========================

                case "Custodian Transfer":

                    var custodian =
                        _context.CustodianChangeRequests
                        .FirstOrDefault(
                            x => x.CustodianChangeID ==
                                 requestID);

                    if (custodian == null)
                        return NotFound();

                    var custodianAsset =
                        _assetContext.AssetModels
                        .FirstOrDefault(
                            x => x.AssetID ==
                                 custodian.AssetID);

                    return Ok(new
                    {
                        RequestType = "Custodian Transfer",

                        EmployeeID =
                            custodian.EmployeeID,

                        RequestBy =
                            custodian.RequestBy,

                        Department =
                            custodian.CustodianDepartment,

                        Designation =
                            custodian.CustDesignation,

                        RequestedCustodian =
                            custodian.RequestedChangeCustodian,

                        CustodianComments =
                            custodian.CustodianComments,

                        RequestDate =
                            custodian.Date,

                        ApproverID =
                            custodian.ApproverID,

                        ApproverName =
                            custodian.ApproverName,

                        ApproverDepartment =
                            custodian.ApproverDepartment,

                        ApproverDesignation =
                            custodian.ApproverDesignation,

                        ApproverComments =
                            custodian.ApproverComments,

                        AdminComments =
                            custodian.AdminComments,

                        Status =
                            custodian.Status,

                        AssetName =
                            custodianAsset?.AssetDesc,

                        MainAssetNumber =
                            custodianAsset?.MainAssetNumber,

                        AssetSubNumber =
                            custodianAsset?.AssetSubNumber,

                        AssetClass =
                            custodianAsset?.AssetClass,

                        AssetStatus =
                            custodianAsset?.Status,

                        CurrentLocation =
                            custodianAsset?.Location,

                        Component =
                            custodianAsset?.Component,

                        FirstAcquisitionDate =
                            custodianAsset?.FirstAcquisitionDate,

                        CapitalizationDate =
                            custodianAsset?.AssetCapitalizationDate
                    });

                default:
                    return BadRequest("Invalid Request Type");
            }
        }
    }
    }