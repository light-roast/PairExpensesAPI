﻿using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PairExpensesAPI.Controllers;
using PairExpensesAPI.Entities;
using PairXpensesAPI.Services;

namespace PairXpensesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public UserController(IUserService userService, ILogger<AccountController> logger)
		{
			_userService = userService;
			_logger = logger;
		}

        [HttpGet]
        [Authorize(Roles="pair1, pair2")] 
        public IActionResult GetAllUsers()
        {
            bool isPair1 = HttpContext.User.IsInRole("pair1");

    
    		bool isPair2 = HttpContext.User.IsInRole("pair2");
		    // Check if the PairRole claim exists
            if (isPair1)
            {
                var users = _userService.GetAllUsers("pair1");
                _logger.LogInformation("Get all user from pair 1");
                return Ok(users);
            }
			else if(isPair2)
			{
				var users = _userService.GetAllUsers("pair2");
                _logger.LogInformation("Get all user from pair 2");
                return Ok(users);
			}
			else
			{
				return Unauthorized("The user's role could not be determined.");
			}
      
            
        }

        //      [HttpPost]
        //public IActionResult CreateUser(User user)
        //{
        //	_userService.CreateUser(user);
        //	return Ok("User created successfully.");
        //}

        [HttpPatch("{id}")]
        [Authorize(Roles="pair1, pair2")]
        public IActionResult UpdateUserById([FromBody] UserReq user)
		{
            bool isPair1 = HttpContext.User.IsInRole("pair1");

    
    		bool isPair2 = HttpContext.User.IsInRole("pair2");

			if (isPair1)
            {
                var userToUpdate = _userService.GetAllUsers("pair1").FirstOrDefault(u => u.Id == user.Id);
				if (userToUpdate == null)
					return NotFound("User not found.");

				var updatedUser = _userService.UpdateUserById(userToUpdate, user);
				if (updatedUser == null)
				{
					return BadRequest("Problem updating the user");
				}
				else
				{
                    _logger.LogInformation("Update user from pair 1");
                    return Ok(updatedUser);
				}
            }
			else if(isPair2)
			{
				var userToUpdate = _userService.GetAllUsers("pair2").FirstOrDefault(u => u.Id == user.Id);
				if (userToUpdate == null)
					return NotFound("User not found.");

				var updatedUser = _userService.UpdateUserById(userToUpdate, user);
				if (updatedUser == null)
				{
					return BadRequest("Problem updating the user");
				}
				else
				{
                    _logger.LogInformation("Update user from pair 1");
                    return Ok(updatedUser);
				}
			}
			else
			{
				return Unauthorized("The user's role could not be determined.");
			}




            
			
		}

       

  //      [HttpDelete("{id}")]
		//public IActionResult DeleteUser(int id)
		//{
		//	var userToDelete = _userService.GetAllUsers().FirstOrDefault(u => u.Id == id);
		//	if (userToDelete == null)
		//		return NotFound("User not found.");

		//	_userService.DeleteUser(userToDelete);
		//	return Ok("User deleted successfully.");
		//}
	}
}
