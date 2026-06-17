using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class ViewImportedAssetsController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        public ViewImportedAssetsController(PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/ViewImportedAssets")]
        public IActionResult ViewImportedAssets()
        {
            var data = _context.AssetParkings
                .ToList();

            return Ok(data);
        }
    }
}