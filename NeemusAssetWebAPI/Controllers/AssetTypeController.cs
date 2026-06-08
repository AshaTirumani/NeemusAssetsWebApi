using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetTypeController : Controller
    {
        private readonly AssetSAPDBContext _context;
        public AssetTypeController(AssetSAPDBContext context)
        {
            _context = context;
        }
        //Get
        [HttpGet]
        [Route("api/AssetTypeDetails")]
        public IActionResult GetAssetTypeDetails()
        
        {
            //var data = _context.AssetTypeModels.ToList();
            var data = _context.AssetTypeModels.Where(x => x.Status == "Active").ToList();
            return Ok(data);
        }
        //Add
        [HttpPost]
        [Route("api/InsertAssetTypeDetails")]
        public IActionResult InsertAssetTypeDetails([FromBody] AssetTypeModel model)
        {
            try
            {
                AssetTypeModel obj = new AssetTypeModel()
                {
                    AssetTypeName = model.AssetTypeName,
                    AssetTypeCode = model.AssetTypeCode,
                    AssetClassName = model.AssetClassName,
                    AssetClassID = model.AssetClassID,
                    Status = "Active",
                    CreatedDate = DateTime.Now,
                };

                _context.AssetTypeModels.Add(obj);
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
        [Route("api/UpdateAssetTypeDetails")]
        public IActionResult UpdateAssetTypeDetails([FromBody] AssetTypeModel model)
        {
            var data = _context.AssetTypeModels
              .FirstOrDefault(x => x.AssetTypeID == model.AssetTypeID);

            if (data == null)
            {
                return NotFound();
            }

            data.AssetTypeName = model.AssetTypeName;
            data.AssetTypeCode = model.AssetTypeCode;
            data.AssetClassName = model.AssetClassName;
            data.AssetClassID = model.AssetClassID;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok();
        }
        //delete
        [HttpDelete]
        [Route("api/DeleteAssetTypeDetails/{id}")]
        public IActionResult DeleteAssetTypeDetails(int id)
        {
            var data = _context.AssetTypeModels
                               .FirstOrDefault(x => x.AssetTypeID == id);
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
