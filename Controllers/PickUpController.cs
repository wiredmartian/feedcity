using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using feeddcity.Data;
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
        [HttpPost]
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
                return StatusCode(500, new {message = e.Message});
            }
        }
        [HttpGet]
        [Route("{pickupId}")]
        public ActionResult<PickUpRequest> GetPickUpRequest(int pickupId)
        {
            try
            {
                PickUpRequest pickUpRequest = _pickUp.GetSinglePickupRequest(pickupId);
                return Ok(pickUpRequest);
            }
            catch (Exception e)
            {
                return StatusCode(500, new {message = e.Message});
            }
        }

        [HttpGet]
        [Route("active-requests")]
        public ActionResult<List<PickUpRequest>> GetActiveRequests()
        {
            try
            {
                List<PickUpRequest> requests = _pickUp.GetActiveRequests();
                return Ok(requests);
            }
            catch (Exception e)
            {
                return StatusCode(500, new {message = e.Message});
            }
        }

        [HttpPost]
        [Route("cancel-request/{id}")]
        public IActionResult CancelPickUpRequest(int id)
        {
            try
            {
                int cancelled = _pickUp.CancelPickUpRequest(id);
                if (cancelled == 0)
                {
                    return BadRequest(new {error = "Failed to cancel request" });
                }
                return Ok(new {message = "Request cancelled!"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new {message = e.Message});
            }
        }
    }
}