using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
using static NeemusAssetWebAPI.Models.DocumentMappingModel;
namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class DocumentMappingController : Controller
    {
        private readonly PostgreDBContext _context;
        public DocumentMappingController(PostgreDBContext context)
        {
            _context = context;
        }
       
        [HttpGet]
        [Route("api/AssetDocumentMappings")]
        public IActionResult GetAssetDocumentMappings()
        {
            var data = _context.AssetDocumentMappings
                               .Where(x => x.Status == "Active")
                               .ToList();

            return Ok(data);
        }


        [HttpPost]
        [Route("api/InsertAssetDocument")]
        public async Task<IActionResult> InsertAssetDocument(
    [FromBody] List<AssetDocumentMappingDto> models)
        {
            foreach (var model in models)
            {
                var data = new AssetDocumentMapping
                {
                    DocumentID = model.DocumentID,
                    Status = "Active",
                    ImageLocation = model.ImageLocation,
                    Date = DateTime.Now.ToString("yyyy-MM-dd"),
                    MainAssetNumber = model.MainAssetNumber,
                    AssetID = model.AssetID
                };

                _context.AssetDocumentMappings.Add(data);
            }

            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Documents Saved Successfully"
            });
        }
    }
}
