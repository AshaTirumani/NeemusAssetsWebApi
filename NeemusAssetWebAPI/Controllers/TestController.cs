using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    //[Route("api/[controller]")]
    [Route("api/TestController/ViewDetails")]
    public class TestController : ControllerBase
    {

        //private readonly AppDbContext _context;


        //public TestController(AppDbContext context)
        //{
        //    _context = context;
        //}

        private readonly AssetCommonDBContext _commonContext;
        private readonly PostgreDBContext _amsContext;

        public TestController(
            AssetCommonDBContext commonContext,
            PostgreDBContext amsContext)
        {
            _commonContext = commonContext;
            _amsContext = amsContext;
        }


        //[HttpGet]
        //public IActionResult Get()
        //{
        //    var departments = _commonContext.DepartmentMasters.ToList();

        //    var locations = _amsContext.LocationMasters.ToList();

        //    var data = (from d in departments
        //                join e in locations
        //                on (d.DepartmentCode ?? "").Trim().ToLower()
        //                equals (e.DepartmentCode ?? "").Trim().ToLower()
        //                select new
        //                {
        //                    d.DepartmentName,
        //                    e.DepartmentCode,
        //                    e.LocationCode,
        //                    e.Block
        //                }).ToList();
        //    return Ok(data);
        //}
    }



}