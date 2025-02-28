using Admin.Data;
using Admin.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserContext _authContext;
        public AuthController(UserContext UserContext)

        {
            _authContext = UserContext;
        }
        [HttpPost("authenticate")]
        public async Task<IActionResult> Authenticate([FromBody] User userObj)
        {
            if (userObj == null)
                return BadRequest();
            var user = await _authContext.Users.FirstOrDefaultAsync ( x => x.Name == userObj.Name && x.Password==userObj.Password) ;
            if (user == null)
                return NotFound(new { message = "l'ustilisateur n'exist pas!" });
            return Ok(new

            {
                Message = "login Success!"
            });

        }
    }
}
