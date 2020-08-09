using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Index()
        {
            return Ok(new {Message = "Welcome"});
        }
    }
}