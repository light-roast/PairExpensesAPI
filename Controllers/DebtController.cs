using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PairExpensesAPI.Entities;
using PairXpensesAPI.Services;
using System.Reflection.Metadata;

namespace PairXpensesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DebtController : ControllerBase
	{
		private readonly IDebtService _debtService;

		public DebtController(IDebtService debtService)
		{
			this._debtService = debtService;
		}

		[HttpGet("{id}")]
		public IActionResult GetDebtById(int id)
		{
			var debt = this._debtService.GetDebtById(id);
			if (debt != null)
			{
				return Ok(debt);
			}
			else
			{ return NotFound("Debt user id found"); }
		}

		[HttpGet("user/{userId}")]
		public IActionResult GetDebtsByUSerId(int userId)
		{
			List<Debt> deudas = this._debtService.GetAllDebtsByUserId(userId);
			if (deudas != null)
			{
				return Ok(deudas);
			}
			else { return BadRequest("No debts found for the user"); }
		}

		[HttpPost]
		public IActionResult CreateDebt([FromBody] Debt debt)
		{
			this._debtService.CreateDebt(debt);
			return Ok("Debt created successfully");
		}

		[HttpDelete]
		public IActionResult DeleteDebt(int id)
		{
			var debt = this._debtService.GetDebtById(id);
			if (debt != null)
			{
				this._debtService.DeleteDebt(debt);
				return Ok("Debt deleted successfully");
			}
			else
			{
				return NotFound("Debt not found");
			}


		}

		[HttpGet("user/total/{userId}")]
		public IActionResult GetTotalDebtByUser(int userId)
		{
			long totalDebt = this._debtService.GetTotalDebtValueByUserId(userId);
			if(totalDebt != -1)
			{
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
			var debtToUpdate = this._debtService.GetDebtById(id);
			if (debtToUpdate != null)
			{
				var updated = this._debtService.UpdateDebtById(debtToUpdate, debt);
				if(updated != null)
				{
					return Ok("Debt updated succesfully");
				}
				else
				{
					return BadRequest("Error while updating");
				}
			}
			else
			{
				return BadRequest("No debt found");
			}
		}

		



	}
}
