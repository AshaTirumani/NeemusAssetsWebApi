using Microsoft.AspNetCore.Mvc;
using NeemusAssetWebAPI.Data;
using NeemusAssetWebAPI.Models;
using NeemusAssetWebAPI.Helpers;

namespace NeemusAssetWebAPI.Controllers
{
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly PostgreDBContext _context;

        private readonly ClsGlobal _clsGlobal = new ClsGlobal();

        public LoginController(
            PostgreDBContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Route("api/TestCustodian")]
        public IActionResult TestCustodian()
        {
            var data = _context.EmployeeMasters.ToList();

            return Ok(data);
        }


        [HttpGet]
        [Route("api/TestDecrypt")]
        public IActionResult TestDecrypt()
        {
            var decrypted =
                _clsGlobal.DecryptAES(
                    "1VupZ9ltRLckFxCiAYlTkQ=="
                );

            return Ok(decrypted);
        }

        [HttpPost]
        [Route("api/Login")]
        public IActionResult Login(
            [FromBody] LoginModel model)
        {
            // Encrypt entered password
            string encryptedPassword =
      _clsGlobal.EncryptAES(model.Password ?? "");

            Console.WriteLine(
                "Entered Password : " +
                model.Password
            );

            Console.WriteLine(
     "Encrypted Password : " +
     encryptedPassword
 );

            var dbUser =
                _context.EmployeeMasters
                .FirstOrDefault(x =>
                    x.LdapUserId == model.UserID);

            if (dbUser != null)
            {
                Console.WriteLine(
                    "DB Password : " +
                    dbUser.LdapPwd
                );

                Console.WriteLine(
                    "DB UserID : " +
                    dbUser.LdapUserId
                );
            }
            else
            {
                Console.WriteLine(
                    "User not found in DB"
                );
            }

            var user =
                _context.EmployeeMasters
                .FirstOrDefault(x =>
                    x.LdapUserId == model.UserID &&
                    x.LdapPwd == encryptedPassword);

        
            if (user == null)
            {
                return BadRequest(
                    "Invalid User ID or Password"
                );
            }

            return Ok(new
            {
                CustodianID = user.CustodianID,
                UserName = user.CustodianName,
                Designation = user.Designation,
                Email = user.Email
            });
        }
    }
}