using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Models;
using NeemusAssetWebAPI.Data;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly PostgreDBContext _context;

        public EmployeeController(PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/CustodianDetails")]
        public IActionResult GetCustodians()
        {
            var data = _context.EmployeeMasters.ToList();
            return Ok(data);
        }

        [HttpPost]
        [Route("api/InsertCustodian")]
        public IActionResult InsertCustodian([FromBody] EmployeeMaster model)
        {
            try
            {
                EmployeeMaster obj = new EmployeeMaster()
                {
                    CustodianID = model.CustodianID,
                    CustodianDepartmentCode = model.CustodianDepartmentCode,
                    CustodianName = model.CustodianName,
                    Designation = model.Designation,
                    ReportingStaffNo = model.ReportingStaffNo,
                    Email = model.Email,
                    CustodianStatus = model.CustodianStatus,
                    CreateDate = DateTime.Now,
                    LdapUserId = model.LdapUserId,
                    InternalNumber = model.InternalNumber,
                    LdapPwd = model.LdapPwd
                };

                _context.EmployeeMasters.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Custodian Inserted Successfully",
                    Data = obj
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        [Route("api/UpdateCustodian/{id}")]
        public IActionResult UpdateCustodian(string id, [FromBody] EmployeeMaster model)
        {
            var data = _context.EmployeeMasters
                               .FirstOrDefault(x => x.CustodianID == id);

            if (data == null)
            {
                return NotFound();
            }

            data.CustodianDepartmentCode = model.CustodianDepartmentCode;
            data.CustodianName = model.CustodianName;
            data.Designation = model.Designation;
            data.ReportingStaffNo = model.ReportingStaffNo;
            data.Email = model.Email;
            data.CustodianStatus = model.CustodianStatus;
            data.CreateDate = model.CreateDate;
            data.LdapUserId = model.LdapUserId;
            data.InternalNumber = model.InternalNumber;
            data.LdapPwd = model.LdapPwd;

            _context.SaveChanges();

            return Ok("Updated Successfully");
        }

        [HttpDelete]
        [Route("api/DeleteCustodian/{id}")]
        public IActionResult DeleteCustodian(string id)
        {
            var data = _context.EmployeeMasters
                               .FirstOrDefault(x => x.CustodianID == id);

            if (data == null)
            {
                return NotFound();
            }

            _context.EmployeeMasters.Remove(data);
            _context.SaveChanges();

            return Ok("Deleted Successfully");
        }
    }
}