using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Win32;
using WebApi42.Bussiness;


namespace WebApi42.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(UsersService userService) : ControllerBase
    {
       
        [HttpGet("register")]
        public async Task<ActionResult> Register()
        {

            var user = new DTO.UserDTO
            {
                Email = ""
            ,
                Name = " "
            ,
                Password = " "
            ,
                UserName = " "
            };

            var guid=await userService.Add(user);
            return Ok(guid);

        }
        [Authorize]
        [HttpGet("login")]
        public async Task<ActionResult> Login()
        {

            return Ok();

        }

        
        private ActionResult GenerateTocken()
        {

            return Ok();

        }
        
    }
}
