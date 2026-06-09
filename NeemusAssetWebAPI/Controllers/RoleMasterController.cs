using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class RoleMasterController : Controller
    {
        private readonly PostgreDBContext _context;

        public RoleMasterController(PostgreDBContext context)
        {
            _context = context;
        }

        // Get 
        [HttpGet]
        [Route("api/RoleDetails")]
        public IActionResult GetRoles()
        {
            var data = _context.RoleMasterModels
                .Where(x => x.ROLE_STATUS == "Active")
                .ToList();

            return Ok(data);
        }

        // Add Role
        [HttpPost]
        [Route("api/AddRole")]
        public IActionResult AddRole([FromBody] RoleMasterModel model)
        {
            try
            {
                RoleMasterModel obj = new RoleMasterModel()
                {
                    ROLE_NAME = model.ROLE_NAME,
                    CustodianID = model.CustodianID,
                    ROLE_STATUS = "Active",
                    CREATE_DATE = DateTime.Now
                };

                _context.RoleMasterModels.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Role Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/UpdateRole")]
        public IActionResult UpdateRole(RoleMasterModel model)
        {
            var data = _context.RoleMasterModels
              .FirstOrDefault(x => x.ROLE_ID == model.ROLE_ID);

            if (data == null)
            {
                return NotFound();
            }

            data.ROLE_NAME = model.ROLE_NAME;
            data.CustodianID = model.CustodianID;
            data.ROLE_STATUS = model.ROLE_STATUS;

            _context.SaveChanges();

            return Ok();
        }
        //delete
        [HttpDelete]
        [Route("api/DeleteRoles/{id}")]
        public IActionResult DeleteRoles(int id)
        {
            var data = _context.RoleMasterModels
                               .FirstOrDefault(x => x.ROLE_ID == id);

            if (data == null)
            {
                return NotFound();
            }

            data.ROLE_STATUS = "InActive";

            _context.SaveChanges();

            return Ok();
        }

    }
}
