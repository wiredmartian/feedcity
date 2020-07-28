using feeddcity.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserModel userModel)
        {
            return Ok(userModel);
        }
    }
}