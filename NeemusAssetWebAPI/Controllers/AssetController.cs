using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetController : Controller
    {
        private readonly AssetSAPDBContext _context;
        private readonly PostgreDBContext _postgreDBContext;
        public AssetController(AssetSAPDBContext context, PostgreDBContext postgreDBContext)
        {
            _context = context;
            _postgreDBContext = postgreDBContext;
        }

        //Get
        [HttpGet]
        [Route("api/AssetDetails")]
        public IActionResult GetAssetDetails()
        {
            var data = _context.AssetModels.Where(x => x.Assetstatus == "Active").ToList();

            return Ok(data);
        }



        [HttpPost]
        [Route("api/InsertAsset")]
        public IActionResult InsertAsset([FromBody] AssetModel model)
        {
            try
            {
                AssetModel obj = new AssetModel()
                {
                    AssetID = model.AssetID,
                    MainAssetNumber = model.MainAssetNumber,
                    AssetSubNumber = model.AssetSubNumber,
                    CustodianDepartment = model.CustodianDepartment,
                    AssetClass = model.AssetClass,
                    AssetType = model.AssetType,
                    AssetDesc = model.AssetDesc,
                    SerialNumber = model.SerialNumber,
                    Model = model.Model,
                    Make = model.Make,
                    YearofPurchase = model.YearofPurchase,
                    FirstAcquisitionDate = model.FirstAcquisitionDate,
                    AssetCapitalizationDate = model.AssetCapitalizationDate,
                    WarrantyDate = model.WarrantyDate,
                    //   FirstAcquisitionDate = model.FirstAcquisitionDate.HasValue
                    //    ? DateTime.SpecifyKind(model.FirstAcquisitionDate.Value, DateTimeKind.Utc)
                    //  : (DateTime?)null,
                    //  AssetCapitalizationDate = model.AssetCapitalizationDate.HasValue
                    //? DateTime.SpecifyKind(model.AssetCapitalizationDate.Value, DateTimeKind.Utc)
                    //: (DateTime?)null,
                    Unit = model.Unit,
                    CustodianID = model.CustodianID,
                    Location = model.Location,
                    Cost = model.Cost,
                    Component = model.Component,
                    GRNumber = model.GRNumber,
                    Indentor = model.Indentor,
                    //WarrantyDate = model.WarrantyDate.HasValue
                    //  ? DateTime.SpecifyKind(model.WarrantyDate.Value, DateTimeKind.Utc)
                    //: (DateTime?)null,
                    Remarks = model.Remarks,
                    Status = model.Status,
                    Assetstatus = "Active",
                    CreationDate = System.DateTime.Today
                };

                _context.AssetModels.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Asset Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/BulkInsertAsset")]
        public IActionResult BulkInsertAsset([FromBody] List<AssetModel> models)
        {
            try
            {
                foreach (var model in models)
                {
                    model.FirstAcquisitionDate =
                    model.FirstAcquisitionDate?.ToLocalTime();

                    model.AssetCapitalizationDate =
                        model.AssetCapitalizationDate?.ToLocalTime();

                    model.WarrantyDate =
                        model.WarrantyDate?.ToLocalTime();

                    model.CreationDate = DateTime.Now;
                    model.Assetstatus = "Active";


                    _context.AssetModels.Add(model);
                }

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                //return Ok(0);
                return BadRequest(ex.ToString());
            }
        }


        //[HttpGet]
        //[Route("api/AssetMasterDetails")]
        //public async Task<IActionResult> AssetMasterDetails()
        //{

        //    var locations = await _postgreDBContext.LocationMasters
        //    .ToListAsync();
        //    var statuses = await _postgreDBContext.StatusMasters.ToListAsync();

        //    var data = await (
        //        from a in _context.AssetModels
        //        join c in _context.AssetClasss
        //            on a.AssetClass equals c.AssetClassID.ToString()
        //        select new
        //        {
        //            a.AssetID,
        //            a.MainAssetNumber,
        //            a.CustodianDepartment,
        //            a.AssetDesc,
        //            a.Status,
        //            a.FirstAcquisitionDate,
        //            a.AssetCapitalizationDate,
        //            a.Location,
        //            AssetClassName = c.AssetClassName
        //        }
        //    ).ToListAsync();

        //    var result = data.Select(a => new
        //    {
        //        a.AssetID,
        //        a.MainAssetNumber,
        //        a.CustodianDepartment,
        //        a.AssetDesc,
        //        StatusName = statuses
        //    .FirstOrDefault(s => s.StatusID.ToString() == a.Status)
        //    ?.StatusName,
        //        a.FirstAcquisitionDate,
        //        a.AssetCapitalizationDate,
        //        a.AssetClassName,
        //        LocationName = locations
        //            .FirstOrDefault(l => l.LocationID.ToString() == a.Location)
        //            ?.Location
        //    }).ToList();

        //    return Ok(result);




        //}
        [HttpGet]
        [Route("api/AssetMasterDetails")]
        public async Task<IActionResult> AssetMasterDetails()
        {
            try
            {
                var statuses = await _postgreDBContext.StatusMasters.ToListAsync();

                var assets = await (
                    from a in _context.AssetModels
                    join c in _context.AssetClasss
                        on a.AssetClass equals c.AssetClassID.ToString()
                    select new
                    {
                        a.AssetID,
                        a.MainAssetNumber,
                        a.CustodianDepartment,
                        a.AssetDesc,
                        a.Status,
                        AssetClassName = c.AssetClassName
                    }
                ).ToListAsync();

                var result = assets.Select(a => new
                {
                    a.AssetID,
                    a.MainAssetNumber,
                    a.CustodianDepartment,
                    a.AssetDesc,

                    StatusName = statuses
                        .FirstOrDefault(s => s.StatusID.ToString() == a.Status)
                        ?.StatusName,

                    a.AssetClassName
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet]
        [Route("api/AssetDashboardCount")]
        public IActionResult AssetDashboardCount()
        {
            var result = new
            {
                TotalAssets = _context.AssetModels.Count(),

                ActiveAssets = _context.AssetModels
                    .Count(x => x.Assetstatus == "Active"),

                InactiveAssets = _context.AssetModels
                    .Count(x => x.Assetstatus == "Inactive"),

                ScrappedAssets = _context.AssetModels
                    .Count(x => x.Assetstatus == "Scrapped" || x.Assetstatus == "Sold")
            };

            return Ok(result);
        }


        [HttpGet]
        [Route("api/GetMyAssets/{assetClassID}/{custodianID}")]
        public IActionResult GetMyAssets(string assetClassID, string custodianID)
        {
            try
            {
                Console.WriteLine($"Asset Class : {assetClassID}");
                Console.WriteLine($"Custodian ID : {custodianID}");

                var data = _context.AssetModels
                    .Where(x =>
                        x.Assetstatus == "Active" &&
                        x.AssetClass == assetClassID &&
                        x.CustodianID == custodianID)
                    .Select(x => new
                    {
                        x.AssetID,
                        x.MainAssetNumber,
                        x.AssetDesc
                    })
                    .ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.ToString());
            }
        }
    }
}
