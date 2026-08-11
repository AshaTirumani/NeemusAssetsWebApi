using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ErpAssetController : Controller
    {
        private readonly ERPDBContext _erpContext;
        private readonly AssetSAPDBContext _sapContext;

        public ErpAssetController(ERPDBContext erpContext, AssetSAPDBContext sapContext)
        {
            _erpContext = erpContext;
            _sapContext = sapContext;
        }

        // ====================== GET ERP ASSETS ======================
        [HttpGet]
        [Route("api/ERPAssetDetails")]
        public IActionResult GetERPAssetDetails()
        {
            var data = _erpContext.ErpAssets
                                  .Where(x => x.Assetstatus == "Active")
                                  .ToList();

            return Ok(data);
        }

        // ====================== IMPORT ERP TO SAP ======================
        [HttpPost]
        [Route("api/ImportERPAssets/{assetClass}")]
        public IActionResult ImportERPAssets(string assetClass)
        {
            try
            {
                var data = _erpContext.ErpAssets
                                      .Where(x => x.AssetClass == assetClass &&
                                                  x.Assetstatus == "Active")
                                      .ToList();

                foreach (var model in data)
                {
                    var exists = _sapContext.AssetModels
                        .FirstOrDefault(x =>
                            x.MainAssetNumber == model.MainAssetNumber &&
                            x.AssetSubNumber == model.AssetSubNumber);

                    if (exists != null)
                        continue;

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
                        Unit = model.Unit,
                        CustodianID = model.CustodianID,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Location = model.Location,
                        Cost = model.Cost,
                        Component = model.Component,
                        GRNumber = model.GRNumber,
                        Indentor = model.Indentor,
                        WarrantyDate = model.WarrantyDate,
                        Remarks = model.Remarks,
                        Status = model.Status,
                        Assetstatus = "Active",
                        CreationDate = DateTime.Now
                    };

                    _sapContext.AssetModels.Add(obj);
                }

                _sapContext.SaveChanges();

                return Ok(new
                {
                    Message = "ERP Assets Imported Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // ====================== EXPORT SAP TO ERP ======================
        [HttpPost]
        [Route("api/ExportERPAssets/{assetClass}")]
        public IActionResult ExportERPAssets(string assetClass)
        {
            try
            {
                var data = _sapContext.AssetModels
                                      .Where(x => x.AssetClass == assetClass &&
                                                  x.Assetstatus == "Active")
                                      .ToList();

                foreach (var model in data)
                {
                    var exists = _erpContext.ErpAssets
                        .FirstOrDefault(x =>
                            x.MainAssetNumber == model.MainAssetNumber &&
                            x.AssetSubNumber == model.AssetSubNumber);

                    if (exists != null)
                        continue;

                    ErpAssetModel obj = new ErpAssetModel()
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
                        //FirstAcquisitionDate = model.FirstAcquisitionDate,
                        FirstAcquisitionDate = model.FirstAcquisitionDate.HasValue
    ? DateTime.SpecifyKind(model.FirstAcquisitionDate.Value, DateTimeKind.Utc)
    : null,
                        //AssetCapitalizationDate = model.AssetCapitalizationDate,
                        AssetCapitalizationDate = model.AssetCapitalizationDate.HasValue
    ? DateTime.SpecifyKind(model.AssetCapitalizationDate.Value, DateTimeKind.Utc)
    : null,
                        Unit = model.Unit,
                        CustodianID = model.CustodianID,
                        Latitude = model.Latitude,
                        Longitude = model.Longitude,
                        Location = model.Location,
                        Cost = model.Cost,
                        Component = model.Component,
                        GRNumber = model.GRNumber,
                        Indentor = model.Indentor,
                        WarrantyDate = model.WarrantyDate.HasValue
    ? DateTime.SpecifyKind(model.WarrantyDate.Value, DateTimeKind.Utc)
    : null,
                        Remarks = model.Remarks,
                        Status = model.Status,
                        Assetstatus = "Active",
                        //CreationDate = DateTime.Now
                        CreationDate = DateTime.UtcNow
                    };

                    _erpContext.ErpAssets.Add(obj);
                }

                _erpContext.SaveChanges();

                return Ok(new
                {
                    Message = "Assets Exported Successfully"
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    Message = ex.Message,
                    InnerException = ex.InnerException?.Message
                });
            }
        }
    }
}