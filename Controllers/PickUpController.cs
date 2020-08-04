using System;
using System.Linq;
using feeddcity.Interfaces;
using feeddcity.Models.PickUp;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace feeddcity.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class PickUpController : ControllerBase
    {
        private readonly IPickUp _pickUp;
        
        public PickUpController(IPickUp pickUp)
        {
            _pickUp = pickUp;
        }
        public IActionResult CreatePickUpRequest([FromBody] PickUpRequestModel model)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState.Values.SelectMany(err => err.Errors[0].ErrorMessage));
                }
                int result = _pickUp.RequestPickUp(model);
                if (result == 0)
                {
                    return BadRequest(new { message = "Failed to create a pickup"});
                }
                return Ok(new { message = "Pick up requested created!" });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
            return Ok();
        }
    }
}