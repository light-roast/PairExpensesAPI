using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
		public IActionResult GetAllUsers()
		{
			var users = _userService.GetAllUsers();
			return Ok(users);
		}

		[HttpPost]
		public IActionResult CreateUser(User user)
		{
			_userService.CreateUser(user);
			return Ok("User created successfully.");
		}

		[HttpPut("{id}")]
		public IActionResult UpdateUserById(int id, User user)
		{
			var userToUpdate = _userService.GetAllUsers().FirstOrDefault(u => u.Id == id);
			if (userToUpdate == null)
				return NotFound("User not found.");

			var updatedUser = _userService.UpdateUserById(userToUpdate, user);
			return Ok(updatedUser);
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteUser(int id)
		{
			var userToDelete = _userService.GetAllUsers().FirstOrDefault(u => u.Id == id);
			if (userToDelete == null)
				return NotFound("User not found.");

			_userService.DeleteUser(userToDelete);
			return Ok("User deleted successfully.");
		}
	}
}
