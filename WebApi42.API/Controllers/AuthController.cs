using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Win32;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebApi42.Bussiness;
using WebApi42.DAO;
using WebApi42.DTO;


namespace WebApi42.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(AuthService userService, IConfiguration configuration) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterAuthDTO user)
        {
            try
            {
                var guid = await userService.RegisterAsync(user);
                return Ok(guid.Id);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);

            }



        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(UserLoginAuthDTO userDTO)

        {
            //chack username agains usernmaeDTO
            //chashHash
            try
            {
                var user1 = await userService.LoginAsync(userDTO);
                return Ok(GenerateTocken(user1));
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);

            }


        }


        private string GenerateTocken(User user)
        {
            var claims = new List<Claim>() {
            new Claim(ClaimTypes.Email,user.Email.ToString()),
            new Claim(ClaimTypes.NameIdentifier,user.Id.ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
            var tokenDescriptor = new JwtSecurityToken
            (
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds

            );

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);

        }

        [Authorize]
        [HttpGet("actionResultMustBeAuthenticated")]
        public IActionResult actionResultMustBeAuthenticated()
        {
            return Ok("ok");
        }

    }
}
