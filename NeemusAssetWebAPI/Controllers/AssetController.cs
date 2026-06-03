using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetController : Controller
    {
        private readonly AssetSAPDBContext _context;

        public AssetController(AssetSAPDBContext context)
        {
            _context = context;
        }

        //Get
        [HttpGet]
        [Route("api/AssetDetails")]
        public IActionResult GetAssetDetails()
        {
            var data = _context.AssetModels.Where(x => x.Status == "AVAL").ToList();

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
                     SerialNumber=model.SerialNumber,
                    Model=model.Model,
                    Make=model.Make,
                    YearofPurchase=model.YearofPurchase,
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
                    Cost=model.Cost,
                    Component=model.Component,
                    GRNumber=model.GRNumber,
                    Indentor=model.Indentor,
                    //WarrantyDate = model.WarrantyDate.HasValue
                    //  ? DateTime.SpecifyKind(model.WarrantyDate.Value, DateTimeKind.Utc)
                    //: (DateTime?)null,
                    Remarks=model.Remarks,
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
                    model.Status = "AVAL";


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
    }
}
