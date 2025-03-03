using Admin.Data;
using Admin.Helpers;
using Admin.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

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
                return BadRequest(new { Message = "Données invalides." });

            // Recherche de l'utilisateur par son nom
            var user = await _authContext.Users.FirstOrDefaultAsync(x => x.Name == userObj.Name);

            if (user == null)
                return BadRequest(new { Message = "Nom d'utilisateur ou mot de passe incorrect." });

            // Vérification du mot de passe haché
            if (!PasswordHasher.VerifyPassword(userObj.Password, user.Password))
            {
                return BadRequest(new { Message = "Nom d'utilisateur ou mot de passe incorrect." });
            }

            user.Token = CreatJwt(user);

            return Ok(new
            {
                Message = "Login réussi !",
                Token = user.Token
            });
        }
        private string CreatJwt(User user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes("SuperSecretKeyForJWTGeneration2025.....");
            var identity = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name,user.Name)

            });
            var credentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = identity,
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials =credentials
            };
            var token = jwtTokenHandler.CreateToken(tokenDiscriptor);
            return jwtTokenHandler.WriteToken(token);
        }
    }
}
