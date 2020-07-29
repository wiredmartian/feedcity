using System;
using System.Linq;
using feeddcity.Data;
using feeddcity.Interfaces;
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
        private readonly IUser userSvc;
        public UserController(IUser userSvc)
        {
            this.userSvc = userSvc;
        }
        [AllowAnonymous]
        [HttpPost]
        public IActionResult CreateUser([FromBody] CreateUserModel userModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(err => err.Errors[0].ErrorMessage));
            }

            try
            {
                User userExists = userSvc.GetUser(userModel.EmailAddress);
                if (userExists != null)
                {
                    return BadRequest(new {message = $"A user with email {userModel.EmailAddress} is already exists"});
                }

                int affectedRows = userSvc.CreateUser(userModel);
                if (affectedRows == 0)
                {
                    return BadRequest(new {message = "Failed to create user account"});
                }

                return Ok(new {message = "User created"});
            }
            catch (Exception e)
            {
                return BadRequest(new {message = e.Message });

            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("/api/user/token")]
        public IActionResult AuthUser([FromBody] LoginUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Incorrect email or password" });
            }
            try
            {
                User currentUser = userSvc.AuthenticateUser(model.EmailAddress, model.Password);
                if (currentUser == null)
                {
                    return BadRequest(new { message = "Incorrect email or password" });
                }

                string token = userSvc.GenerateAuthToken(currentUser);
                return Ok(new { token = token });
            }
            catch (Exception e)
            {
                return BadRequest(new { message = e.Message });
            }
        }
    }
}