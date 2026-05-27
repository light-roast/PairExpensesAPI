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
	[Authorize(Roles="pair1, pair2")]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
		private readonly IUserService _userService;
        private readonly ILogger<AccountController> _logger;

        public PaymentController(IPaymentService paymentService, IUserService userService, ILogger<AccountController> logger)
		{
			_paymentService = paymentService;
			_userService = userService;
            _logger = logger;
        }

		private string? GetCallerPair()
			=> HttpContext.User.IsInRole("pair1") ? "pair1"
			 : HttpContext.User.IsInRole("pair2") ? "pair2"
			 : null;

		private bool IsAllowedForUser(int userId)
		{
			var pair = GetCallerPair();
			if (pair is null) return false;
			var target = _userService.GetUserById(userId);
			return target != null && target.PairRole == pair;
		}

		[HttpGet("user/{userId}")]
		public IActionResult GetAllPaymentsByUserId(int userId)
		{
			if (!IsAllowedForUser(userId)) return Forbid();

			var payments = _paymentService.GetAllPaymentsByUserId(userId);
			if (payments != null)
			{
                _logger.LogInformation("Payments of a user returned");
                return Ok(payments);
			}
			else
			{
				return BadRequest("No user id found");
			}
		}

		[HttpGet("total/{userId}")]
		public IActionResult GetTotalPaymentValueByUserId(int userId)
		{
			if (!IsAllowedForUser(userId)) return Forbid();

			long totalPaymentValue = _paymentService.GetTotalPaymentValueByUserId(userId);
			if (totalPaymentValue != -1)
			{
                _logger.LogInformation("Total payment amount of a user returned");
                return Ok(totalPaymentValue);
			}
			else
			{
				return BadRequest("No user id found");
			}
		}

		[HttpPost]
		public IActionResult CreatePayment(Payment payment)
		{
			if (!IsAllowedForUser(payment.UserId)) return Forbid();

			_paymentService.CreatePayment(payment);
            _logger.LogInformation("Payment created");
            return Ok("Payment created successfully.");
		}

		[HttpPatch("{id}")]
		public IActionResult UpdatePaymentById(int id, [FromBody] PaymentReq payment)
		{
			var paymentToUpdate = _paymentService.GetPaymentById(id);
			if (paymentToUpdate == null)
				return NotFound("Payment not found.");

			if (!IsAllowedForUser(paymentToUpdate.UserId)) return Forbid();

			var updatedPayment = _paymentService.UpdatePaymentById(paymentToUpdate, payment);
			if(updatedPayment == null)
			{
				return BadRequest("Problem updating payment");
			}
			else
			{
                _logger.LogInformation("Payment edited");
                return Ok(updatedPayment);
			}
		}

		[HttpDelete("{id}")]
		public IActionResult DeletePayment(int id)
		{
			var paymentToDelete = _paymentService.GetPaymentById(id);
			if (paymentToDelete == null)
				return NotFound("Payment not found.");

			if (!IsAllowedForUser(paymentToDelete.UserId)) return Forbid();

			_paymentService.DeletePayment(paymentToDelete);
            _logger.LogInformation("Payment deleted");
            return Ok("Payment deleted successfully.");
		}

		[HttpDelete("deleteall")]
		public IActionResult DeleteAllPayments()
		{
			var pair = GetCallerPair();
			if (pair is null) return Unauthorized("The user's role could not be determined.");

			_paymentService.DeleteAllPayments(pair);
			_logger.LogInformation("All payments deleted for {Pair}", pair);
			return Ok("All payments deleted successfully.");
		}
	}
}
