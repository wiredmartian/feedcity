using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class HomeController : ControllerBase
    {
        public IActionResult Index()
        {
            return Ok(new {Message = "Welcome"});
        }
    }
}