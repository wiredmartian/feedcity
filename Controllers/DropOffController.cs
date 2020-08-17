using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using feeddcity.Data;
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
        [HttpPost]
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
        
        [HttpGet]
        public ActionResult<List<DropOffZone>> GetDropZones()
        {
            try
            {
                List<DropOffZone> dropZones = _dropOff.GetAllDropOffZones();
                return Ok(dropZones);
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
        
        [HttpGet]
        [Route("{id}")]
        public ActionResult<DropOffZone> GetDropZone([FromRoute(Name = "id")] int id)
        {
            try
            {
               DropOffZone dropZone = _dropOff.GetDropOffZone(id);
                return Ok(dropZone);
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
        
        [HttpGet]
        [Route("search-zones")]
        public ActionResult<List<DropOffZone>> SearchDropOffByAddress([FromQuery(Name = "address")] string address)
        {
            try
            {
                List<DropOffZone> dropZones = _dropOff.SearchDropOffZones(address);
                return Ok(dropZones);
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
        
        [HttpGet]
        [Route("all-zones")]
        public ActionResult<List<DropOffZone>> GetAllDropOfZones()
        {
            try
            {
                List<DropOffZone> dropZones = _dropOff.GetAllDropOffZones();
                return Ok(dropZones);
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
        [HttpGet]
        [Route("province-zones/{id}")]
        public ActionResult<List<DropOffZone>> GetProvinceZones([FromRoute(Name = "id")] int  id)
        {
            try
            {
                List<DropOffZone> dropZones = _dropOff.GetProvincialDropOffZones(id);
                return Ok(dropZones);
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