using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ExportHistoryController : ControllerBase
    {
        private readonly AssetSAPDBContext _context;

        public ExportHistoryController(AssetSAPDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/ExportHistory")]
        public IActionResult GetExportHistory()
        {
            var data = _context.SAPUpdateLogInfos
                .OrderByDescending(x => x.PerformedDate)
                .ToList();

            return Ok(data);
        }
    }
}