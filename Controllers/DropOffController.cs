using System;
using System.Data.SqlClient;
using System.Linq;
using feeddcity.Interfaces;
using feeddcity.Models.DropOff;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
    public class DropOffController : Controller
    {
        private readonly IDropOff _dropOff;

        public DropOffController(IDropOff dropOff)
        {
            _dropOff = dropOff;
        }

        public IActionResult CreateDropOffZone([FromBody] DropOffZoneModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(err => err.Errors[0].ErrorMessage));
                }

                var affectedRows = _dropOff.CreateDropOff(model);
                if (affectedRows == 0)
                {
                    return BadRequest(new {error = "Failed to create zone"});
                }

                return Ok(new { message = "New drop off zone created!"});
            }
            catch (SqlException sqlException)
            {
                Console.WriteLine(sqlException);
                return StatusCode(500, new {error = "Something went horribly wrong"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new {message = e.Message});
            }
        }
    }
}