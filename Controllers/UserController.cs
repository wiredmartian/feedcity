using System;
using System.Linq;
using System.Security.Authentication;
using feeddcity.Data;
using feeddcity.Interfaces;
using feeddcity.Models.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace feeddcity.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUser _userSvc;
        public UserController(IUser userSvc)
        {
            this._userSvc = userSvc;
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
                User userExists = _userSvc.GetUser(userModel.EmailAddress);
                if (userExists != null)
                {
                    return BadRequest(new {error = $"A user with email {userModel.EmailAddress} is already exists"});
                }
                int affectedRows = _userSvc.CreateUser(userModel);
                if (affectedRows == 0)
                {
                    return BadRequest(new {error = "Failed to create user account"});
                }

                return Ok(new {message = "User created"});
            }
            catch (MySqlException sqlException)
            {
                Console.WriteLine(sqlException);
                return StatusCode(500, new {error = "Something went horribly wrong"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new {error = e.Message });

            }
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("token")]
        public IActionResult AuthUser([FromBody] LoginUserModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new { message = "Incorrect email or password" });
            }

            try
            {
                User currentUser = _userSvc.AuthenticateUser(model.EmailAddress, model.Password);
                if (currentUser == null)
                {
                    return BadRequest(new {message = "Incorrect email or password"});
                }
                if (currentUser.Disabled)
                {
                    return BadRequest(new {message = "This user is disabled"});
                }

                string token = _userSvc.GenerateAuthToken(currentUser);
                _userSvc.LogLastSignIn(currentUser.Id);
                return Ok(new {token});
            }
            catch (MySqlException sqlException)
            {
                Console.WriteLine(sqlException);
                return StatusCode(500, new {error = "Something went horribly wrong"});
            }
            catch (Exception e)
            {
                return StatusCode(500, new {error = e.Message});
            }
            
        }

        [HttpPatch]
        [Route("reset-password")]
        public IActionResult ResetPassword([FromBody] ResetPasswordModel model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values.SelectMany(err => err.Errors[0].ErrorMessage));
            }

            try
            {
                var resetPassword = _userSvc.ResetPassword(model.oldPassword, model.newPassword);
                if (resetPassword == 0)
                {
                    return BadRequest(new { error = "Failed to reset password. Unknown error has occured" });
                }

                return Ok(new {message = "Password reset successful!"});
            }
            catch (UnauthorizedAccessException e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new { error = e.Message });
            }
            catch (AuthenticationException e)
            {
                Console.WriteLine(e);
                return StatusCode(400, new { error = e.Message });
            }
            catch (MySqlException sqlException)
            {
                Console.WriteLine(sqlException);
                return StatusCode(500, new { error = "Something went horribly wrong" });
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, new { error = e.Message });
            }
        }
    }
}