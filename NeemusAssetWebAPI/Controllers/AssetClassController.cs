using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class AssetClassController : Controller
    {
        private readonly AssetSAPDBContext _context;
        public AssetClassController(AssetSAPDBContext context)
        {
            _context = context;
        }
        //Get
        [HttpGet]
        [Route("api/AssetClassDetails")]
        public IActionResult GetAssetClass()
        {
            var data = _context.AssetClasss.Where(x => x.Status == "Active").ToList();

            return Ok(data);
        }

        //Add
        [HttpPost]
        [Route("api/InsertAssetClassDetails")]
        public IActionResult InsertAssetClassDetails([FromBody] AssetClass model)
        {
            try
            {
                AssetClass obj = new AssetClass()
                {
                    AssetClassName = model.AssetClassName,
                    AssetClassCode = model.AssetClassCode,
                    Depreciation = model.Depreciation,
                    Status = "Active",
                    CreatedDate = DateTime.Now
                };

                _context.AssetClasss.Add(obj);
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
        [Route("api/UpdateAssetClassDetails")]
        public IActionResult UpdateAssetClassDetails([FromBody] AssetClass model)
        {
            var data = _context.AssetClasss
              .FirstOrDefault(x => x.AssetClassID == model.AssetClassID);

            if (data == null)
            {
                return NotFound();
            }

            data.AssetClassName = model.AssetClassName;
            data.AssetClassCode = model.AssetClassCode;
            data.Depreciation = model.Depreciation;
            data.Status = model.Status;

            _context.SaveChanges();

            return Ok();
        }

        //delete
        [HttpDelete]
        [Route("api/DeleteAssetClass/{id}")]
        public IActionResult DeleteAssetClass(int id)
        {
            var data = _context.AssetClasss
                               .FirstOrDefault(x => x.AssetClassID == id);

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
