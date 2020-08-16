using feeddcity.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DropOffController : Controller
    {
        private readonly IDropOff _dropOff;

        public DropOffController(IDropOff dropOff)
        {
            _dropOff = dropOff;
        }
    }
}