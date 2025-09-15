using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi42.Bussiness;
using WebApi42.DAO;
using WebApi42.DTO;


namespace WebApi42.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IAuthService authService) : ControllerBase
    {

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegisterAuthDTO user)
        {
            try
            {
                var guid = await authService.RegisterAsync(user);
                return Ok(guid.Id);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);

            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<TokenResponseDTO>> Login(UserLoginAuthDTO userDTO)

        {
            try
            {
                var user1 = await authService.LoginAsync(userDTO);
                return Ok(user1);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);

            }


        }
        [HttpPost("refresh-token")]
        public async Task<ActionResult<TokenResponseDTO>> RefreshToken(RefreshTokenRequestDTO userDTO)
        {
            try
            {
                var user1 = await authService.RefreshTokenAsync(userDTO);
                if(user1 == null || user1.RefreshToken is null || user1.AccessToken is null)
                {
                    return Unauthorized("Invalid user or refresh token");
                }
                return Ok(user1);
            }
            catch (InvalidDataException e)
            {
                return BadRequest(e.Message);
            }
        }

        [Authorize]
        [HttpGet("actionResultMustBeAuthenticated")]
        public IActionResult actionResultMustBeAuthenticated()
        {
            return Ok("ok, you are authenticated");
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("adminOnlyEndPoint")]
        public IActionResult adminOnlyEndPoint()
        {
            return Ok("ok, you are un admin");
        }

    }
}
