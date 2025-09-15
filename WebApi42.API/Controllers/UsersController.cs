using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApi42.Bussiness;

namespace WebApi42.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController(AuthService userService) : ControllerBase
    {

       

        [HttpGet]
        public ActionResult Get() {

            return Ok();

        }

        [HttpPost]
        public ActionResult Add()
        {

            return Ok();

        }

        [HttpDelete]
        public ActionResult Delete()
        {

            return Ok();

        }


        [HttpPatch]
        public ActionResult Patch()
        {

            return Ok();

        }
    }
}
