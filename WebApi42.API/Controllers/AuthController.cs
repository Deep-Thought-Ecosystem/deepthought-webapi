using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WebApi42.Bussiness;
using WebApi42.DTO;


namespace WebApi42.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UsersService userService) : ControllerBase
    {
       
        [HttpPost("register")]
        public async Task<ActionResult> Register(UserRegisterAuthDTO user)
        {

            

            var guid=await userService.Add(user);
            return Ok(guid);

        }
        [Authorize]
        [HttpGet("login")]
        public async Task<ActionResult> Login(UserLoginAuthDTO userDTO)

        {
            //chack username agains usernmaeDTO
            //chashHash

            return Ok();

        }

        
        private ActionResult GenerateTocken()
        {

            return Ok();

        }
        
    }
}
