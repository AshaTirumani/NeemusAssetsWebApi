using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI;
using NeemusAssetWebAPI.Data;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetMapController : ControllerBase
    {
        private readonly AssetSAPDBContext _context;


        private readonly PostgreDBContext _postgreDBContext;

        public AssetMapController(
            AssetSAPDBContext context,
            PostgreDBContext postgreDBContext)
        {
            _context = context;
            _postgreDBContext = postgreDBContext;
        }

        [HttpGet]
        [Route("api/GetAssetMapData")]
        public IActionResult GetAssetMapData()
        {
            var locations =
                _postgreDBContext.LocationMasters.ToList();

            var statuses =
                _postgreDBContext.StatusMasters.ToList();

            var assetClasses =
                _context.AssetClasss.ToList();

            var assetTypes =
                _context.AssetTypeModels.ToList();

            var data = _context.AssetModels
                .Where(x =>
                    x.Latitude != null &&
                    x.Longitude != null)
                .ToList();

            var result = data.Select(x => new
            {
                x.AssetID,
                x.MainAssetNumber,
                x.AssetDesc,

                AssetType =
                    assetTypes.FirstOrDefault(a =>
                        a.AssetTypeID.ToString() == x.AssetType)
                    ?.AssetTypeName ?? x.AssetType,

                AssetClass =
                    assetClasses.FirstOrDefault(a =>
                        a.AssetClassID.ToString() == x.AssetClass)
                    ?.AssetClassName ?? x.AssetClass,

                Location =
                    locations.FirstOrDefault(l =>
                        l.LocationID.ToString() == x.Location)
                    ?.Location ?? x.Location,

                Status =
                    statuses.FirstOrDefault(s =>
                        s.StatusID.ToString() == x.Status)
                    ?.StatusName ?? x.Status,

                x.CustodianDepartment,
                x.Latitude,
                x.Longitude
            });

            return Ok(result);
        }
    }
}