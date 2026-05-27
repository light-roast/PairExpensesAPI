using Microsoft.AspNetCore.Mvc;
using PairXpensesAPI.Services;

namespace PairExpensesAPI.Auth
{
	public static class PairAuthExtensions
	{
		public static string? GetCallerPair(this ControllerBase controller)
			=> controller.User.IsInRole("pair1") ? "pair1"
			 : controller.User.IsInRole("pair2") ? "pair2"
			 : null;

		public static bool IsAllowedForUser(this ControllerBase controller, IUserService userService, int userId)
		{
			var pair = controller.GetCallerPair();
			if (pair is null) return false;
			var target = userService.GetUserById(userId);
			return target != null && target.PairRole == pair;
		}
	}
}
