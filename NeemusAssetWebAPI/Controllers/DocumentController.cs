using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class DocumentController : Controller
    {
        private readonly PostgreDBContext _context;
        public DocumentController(PostgreDBContext context)
        {
            _context = context;
        }
        //Get
        [HttpGet]
        [Route("api/DocumentDetails")]
        public IActionResult GetDocumentDetails()
        {
            //var data = _context.DocumentModels.ToList();
            var data = _context.DocumentModels
                      .Where(x => x.Status == "Active")
                      .ToList();
            return Ok(data);
        }
        //Add
        [HttpPost]
        [Route("api/InsertDocumentDetails")]
        public IActionResult InsertDocumentDetails([FromBody] DocumentModel model)
        {
            try
            {
                DocumentModel obj = new DocumentModel()
                {
                    DocumentName = model.DocumentName,
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                };

                _context.DocumentModels.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Asset Class Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
        // edit
        [HttpPut]
        [Route("api/UpdateDocumentDetails")]
        public IActionResult UpdateDocumentDetails([FromBody] DocumentModel model)
        {
            var data = _context.DocumentModels
              .FirstOrDefault(x => x.DocumentID == model.DocumentID);

            if (data == null)
            {
                return NotFound();
            }

            data.DocumentName = model.DocumentName;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok();
        }


        //delete
        [HttpDelete]
        [Route("api/DeleteDocument/{id}")]
        public IActionResult DeleteDocument(int id)
        {
            var data = _context.DocumentModels
                               .FirstOrDefault(x => x.DocumentID == id);

            if (data == null)
            {
                return NotFound();
            }

            data.Status = "InActive";

            _context.SaveChanges();

            return Ok();
        }


    }
}
