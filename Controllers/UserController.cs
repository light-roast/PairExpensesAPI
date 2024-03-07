using IdentityModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PairExpensesAPI.Entities;
using PairXpensesAPI.Services;

namespace PairXpensesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class UserController : ControllerBase
	{
		private readonly IUserService _userService;

		public UserController(IUserService userService)
		{
			_userService = userService;
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
            	return Ok(users);
            }
			else if(isPair2)
			{
				var users = _userService.GetAllUsers("pair2");
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
        [Authorize]
        public IActionResult UpdateUserById([FromBody] UserReq user)
		{
            var pairRoleClaim = HttpContext.User.Claims.FirstOrDefault(c => c.Type == JwtClaimTypes.Role)?.Value;
            if (pairRoleClaim == null)
            {
                return Unauthorized("The user's role could not be determined.");
            }
            var userToUpdate = _userService.GetAllUsers(pairRoleClaim).FirstOrDefault(u => u.Id == user.Id);
			if (userToUpdate == null)
				return NotFound("User not found.");

			var updatedUser = _userService.UpdateUserById(userToUpdate, user);
			if (updatedUser == null)
			{
				return BadRequest("Problem updating the user");
			}
			else
			{
				return Ok(updatedUser);
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
