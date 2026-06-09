using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Models;
using NeemusAssetWebAPI.Data;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly PostgreDBContext _context;
        public DepartmentController(PostgreDBContext context)
        {
            _context = context;
        }
        //Get
        [HttpGet]
        [Route("api/DepartmentDetails")]
        public IActionResult GetDepartments()
        {
            var data = _context.Departments
                      .Where(x => x.DepartmentStatus == "Active")
                      .ToList();

            return Ok(data);
        }

        //Add
        [HttpPost]
        [Route("api/InsertDepartments")]
        public IActionResult InsertDepartment([FromBody] Department model)
        {
            try
            {
                Department obj = new Department()
                {
                    DepartmentCode = model.DepartmentCode,
                    DepartmentName = model.DepartmentName,
                    DepartmentStatus = "Active"
                };

                _context.Departments.Add(obj);
                _context.SaveChanges();

                return Ok(new
                {
                    Message = "Department Inserted Successfully",
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
        [Route("api/UpdateDepartments")]
        public IActionResult UpdateDepartment([FromBody] Department model)
        {
            var data = _context.Departments
              .FirstOrDefault(x => x.DepartmentID == model.DepartmentID);

            if (data == null)
            {
                return NotFound();
            }

            data.DepartmentCode = model.DepartmentCode;
            data.DepartmentName = model.DepartmentName;
            data.DepartmentStatus = model.DepartmentStatus;

            _context.SaveChanges();

            return Ok();
        }

        //delete
        [HttpDelete]
        [Route("api/DeleteDepartment/{id}")]
        public IActionResult DeleteDepartment(int id)
        {
            var data = _context.Departments
                               .FirstOrDefault(x => x.DepartmentID == id);

            if (data == null)
            {
                return NotFound();
            }

            data.DepartmentStatus = "InActive";

            _context.SaveChanges();

            return Ok();
        }

        [HttpGet]
        [Route("api/GetCustodiansByDepartmentName/{departmentName}")]
        public IActionResult GetCustodiansByDepartmentName(string departmentName)
        {
            try
            {
                var data = (from c in _context.EmployeeMasters
                            join d in _context.Departments
                            on c.CustodianDepartmentCode equals d.DepartmentCode
                            where d.DepartmentName == departmentName
                            && c.CustodianStatus == "Active"
                            select c).ToList();

                return Ok(data);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }




    }
}
