using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetParkingController : Controller
    {
        private readonly PostgreDBContext _context;
        private readonly AssetSAPDBContext _assetContext;

        public AssetParkingController(
            PostgreDBContext context,
            AssetSAPDBContext assetContext)
        {
            _context = context;
            _assetContext = assetContext;
        }

        [HttpGet]
        [Route("api/AssetParkingDetails")]
        public IActionResult AssetParkingDetails()
        {
            try
            {
                var data = _context.AssetParkings
                    .OrderByDescending(x => x.AssetParkingID)
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/GetAssetsForParking/{assetTypeID}")]
        public IActionResult GetAssetsForParking(int assetTypeID)
        {
            try
            {
                var assetType = _assetContext.AssetTypeModels
                    .FirstOrDefault(x => x.AssetTypeID == assetTypeID);

                if (assetType == null)
                {
                    return Ok(new List<AssetModel>());
                }

                var data = _assetContext.AssetModels
     .Where(x =>
         x.Status != null &&
         x.Status.Trim().ToUpper() == "AVAL" &&

         x.AssetType != null &&
         x.AssetType.Trim().ToLower() ==
         assetType.AssetTypeName.Trim().ToLower()
     )
     .ToList();

                Console.WriteLine($"AssetTypeName = {assetType.AssetTypeName}");
                Console.WriteLine($"Records Found = {data.Count}");

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        [HttpGet]
        [Route("api/ViewAssetParking")]
        public IActionResult ViewAssetParking()
        {
            try
            {
                var data = _context.AssetParkings
                    .Where(x => x.Status == "Active")
                    .GroupBy(x => x.CustodianID)
                    .Select(g => new
                    {
                        RequestType = "Asset Parking",
                        CustodianID = g.Key,
                        CustodianName = g.First().RequestBy,
                        DepartmentName = g.First().CustodianDepartment,
                        Designation = g.First().CustDesignation
                    })
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        //    [HttpPost]
        //    [Route("api/CreateAssetParking")]
        //    public IActionResult CreateAssetParking(
        //[FromBody] AssetParkingModel model)
        //    {
        //        try
        //        {
        //            var lastId = _context.AssetParkings
        //  .OrderByDescending(x => x.AssetParkingID)
        //  .Select(x => x.AssetParkingID)
        //  .FirstOrDefault();

        //            var obj = new AssetParkingModel()
        //            {
        //                AssetParkingID = lastId + 1,

        //                AssetID = model.AssetID,
        //                MainAssetNumber = model.MainAssetNumber,

        //                CustodianID = model.CustodianID,
        //                RequestBy = model.RequestBy,
        //                CustodianDepartment = model.CustodianDepartment,
        //                CustDepartmentCode = model.CustDepartmentCode,
        //                CustDesignation = model.CustDesignation,

        //                AssetTypeID = model.AssetTypeID,
        //                AssetClassID = model.AssetClassID,

        //                Location = model.Location,
        //                LocationCode = model.LocationCode,

        //                AdminID = model.AdminID,

        //                Date = DateTime.Now,

        //                Status = "Active"
        //            };

        //            _context.AssetParkings.Add(obj);

        //            _context.SaveChanges();
        //            Console.WriteLine($"AssetID = {obj.AssetID}");
        //            Console.WriteLine($"MainAssetNumber = {obj.MainAssetNumber}");
        //            Console.WriteLine($"CustodianID = {obj.CustodianID}");
        //            Console.WriteLine($"AssetTypeID = {obj.AssetTypeID}");
        //            Console.WriteLine($"AssetClassID = {obj.AssetClassID}");
        //            Console.WriteLine($"Location = {obj.Location}");
        //            return Ok("Asset Parked Successfully");
        //        }
        //        catch (Exception ex)
        //        {
        //            return BadRequest(ex.ToString);
        //        }
        //    }
        [HttpPost]
        [Route("api/CreateAssetParking")]
        public IActionResult CreateAssetParking(
     [FromBody] AssetParkingModel model)
        {
            try
            {
                var obj = new AssetParkingModel()
                {
                    AssetID = model.AssetID,
                    MainAssetNumber = model.MainAssetNumber,

                    CustodianID = model.CustodianID,
                    RequestBy = model.RequestBy,
                    CustodianDepartment = model.CustodianDepartment,
                    CustDepartmentCode = model.CustDepartmentCode,
                    CustDesignation = model.CustDesignation,

                    AssetTypeID = model.AssetTypeID,
                    AssetClassID = model.AssetClassID,

                    Location = model.Location,
                    LocationCode = model.LocationCode,

                    AdminID = model.AdminID,

                    Date = DateTime.UtcNow,
                    Status = "Active"
                };

                _context.AssetParkings.Add(obj);

                _context.SaveChanges();

                var asset = _assetContext.AssetModels
                    .FirstOrDefault(x =>
                        x.AssetID == model.AssetID);

                if (asset != null)
                {
                    asset.Status = "AssetParked";

                    _assetContext.SaveChanges();
                }

                return Ok(new
                {
                    Message = "Asset Parked Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}