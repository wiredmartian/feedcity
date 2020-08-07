using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        [Route("home")]
        public IActionResult Index()
        {
            return Ok("Welcome");
        }
    }
}