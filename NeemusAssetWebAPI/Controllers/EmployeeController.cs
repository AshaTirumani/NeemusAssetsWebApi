using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Helpers;
using NeemusAssetWebAPI.Models;
namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class EmployeeController : Controller
    {
        private readonly PostgreDBContext _context;
        private readonly ClsGlobal _clsGlobal = new ClsGlobal();
        public EmployeeController(PostgreDBContext context)
        {
            _context = context;
        }
      

        [HttpGet]
        [Route("api/CustodianDetails")]
        public IActionResult GetCustodians()
        {
            //var data = _context.EmployeeMasters.ToList();
            var data = _context.EmployeeMasters
                      .Where(x => x.CustodianStatus == "Active")
                      .ToList();
            ClsGlobal obj = new ClsGlobal();

            foreach (var item in data)
            {
                if (!string.IsNullOrEmpty(item.LdapPwd))
                {
                    try
                    {
                        item.LdapPwd = obj.DecryptAES(item.LdapPwd);
                    }
                    catch
                    {
                        // Old records stored as plain text
                        item.LdapPwd = item.LdapPwd;
                    }
                }
            }
        
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
                    CustodianStatus = "Active",
                    CreateDate = DateTime.Now,
                    LdapUserId = model.LdapUserId,
                    InternalNumber = model.InternalNumber,
                    
                    LdapPwd = _clsGlobal.EncryptAES(model.LdapPwd ?? "")
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
            data.CreateDate = model.CreateDate;
            data.LdapUserId = model.LdapUserId;
            data.InternalNumber = model.InternalNumber;
            data.LdapPwd = _clsGlobal.EncryptAES(model.LdapPwd ?? "");

            _context.SaveChanges();

            return Ok();
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

            return Ok();
        }

        [HttpPost]
        [Route("api/BulkInsertCustodian")]
        public IActionResult BulkInsertCustodian([FromBody] List<EmployeeMaster> models)
        {
            try
            {
                foreach (var model in models)
                {
                    //model.FirstAcquisitionDate =
                    //model.FirstAcquisitionDate?.ToLocalTime();

                    //model.AssetCapitalizationDate =
                    //    model.AssetCapitalizationDate?.ToLocalTime();

                    //model.WarrantyDate =
                    //    model.WarrantyDate?.ToLocalTime();

                    model.CreateDate = DateTime.Now;
                    model.CustodianStatus = "Active";


                    _context.EmployeeMasters.Add(model);
                }

                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                //return Ok(0);
                return BadRequest(ex.ToString());
            }
        }



    }
}