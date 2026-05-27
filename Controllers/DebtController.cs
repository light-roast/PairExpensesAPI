using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairExpensesAPI.Auth;
using PairExpensesAPI.Entities;
using PairXpensesAPI.Services;
using PairExpensesAPI.Controllers;

namespace PairXpensesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles="pair1, pair2")]
	public class DebtController : ControllerBase
	{
		private readonly IDebtService _debtService;
		private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public DebtController(IDebtService debtService, IUserService userService, ILogger<AccountController> logger)
		{
			_debtService = debtService;
			_userService = userService;
            _logger = logger;
        }

		[HttpGet("{id}")]
		public IActionResult GetDebtById(int id)
		{
			var debt = _debtService.GetDebtById(id);
			if (debt == null) return NotFound("Debt id no found");
			if (!this.IsAllowedForUser(_userService, debt.UserId)) return Forbid();
			return Ok(debt);
		}

		[HttpGet("user/{userId}")]
		public IActionResult GetDebtsByUSerId(int userId)
		{
			if (!this.IsAllowedForUser(_userService, userId)) return Forbid();

			List<Debt> deudas = _debtService.GetAllDebtsByUserId(userId);
			if (deudas != null)
			{
                _logger.LogInformation("Debts of a user returned");
                return Ok(deudas);
			}
			else { return BadRequest("No debts found for the user"); }
		}

		[HttpPost]
		public IActionResult CreateDebt([FromBody] Debt debt)
		{
			if (!this.IsAllowedForUser(_userService, debt.UserId)) return Forbid();

            _debtService.CreateDebt(debt);
            _logger.LogInformation("Debt created");
            return Ok("Debt created successfully");
		}

		[HttpDelete("{id}")]
		public IActionResult DeleteDebt(int id)
		{
			var debt = _debtService.GetDebtById(id);
			if (debt == null) return NotFound("Debt not found");
			if (!this.IsAllowedForUser(_userService, debt.UserId)) return Forbid();

			_debtService.DeleteDebt(debt);
            _logger.LogInformation("Debt deleted");
            return Ok("Debt deleted successfully");
		}

		[HttpGet("total/{userId}")]
		public IActionResult GetTotalDebtByUser(int userId)
		{
			if (!this.IsAllowedForUser(_userService, userId)) return Forbid();

			long totalDebt = _debtService.GetTotalDebtValueByUserId(userId);
			if(totalDebt != -1)
			{
                _logger.LogInformation("Total debt amount of a user returned");
                return Ok(totalDebt);
			}
			else
			{
				return BadRequest("No debt was found");
			}
		}

		[HttpPatch("{id}")]
		public IActionResult UpdateDebtById(int id, [FromBody] DebtReq debt)
		{
			var debtToUpdate = _debtService.GetDebtById(id);
			if (debtToUpdate == null) return BadRequest("No debt found");
			if (!this.IsAllowedForUser(_userService, debtToUpdate.UserId)) return Forbid();

			var updated = _debtService.UpdateDebtById(debtToUpdate, debt);
			if (updated != null)
			{
                _logger.LogInformation("Debt updated");
                return Ok(updated);
			}
			else
			{
				return BadRequest("Error while updating");
			}
		}

		[HttpDelete("deleteall")]
		public IActionResult DeleteAllDebts()
		{
			var pair = this.GetCallerPair();
			if (pair is null) return Unauthorized("The user's role could not be determined.");

			_debtService.DeleteAllDebts(pair);
			_logger.LogInformation("All debts deleted for {Pair}", pair);
			return Ok("All debts deleted successfully.");
		}
	}
}
