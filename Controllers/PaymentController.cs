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
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;
        private readonly ILogger<AccountController> _logger;

        public PaymentController(IPaymentService paymentService, ILogger<AccountController> logger )
		{
			_paymentService = paymentService;
            _logger = logger;
        }

		[HttpGet("user/{userId}")]
		public IActionResult GetAllPaymentsByUserId(int userId)
		{
			bool isPair1 = HttpContext.User.IsInRole("pair1");

    
    		bool isPair2 = HttpContext.User.IsInRole("pair2");
			
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
			_paymentService.CreatePayment(payment);
            _logger.LogInformation("Payment created");
            return Ok("Payment created successfully.");
		}

		// [HttpGet("{id}")]
		// public IActionResult GetPaymentById(int id)
		// {
		// 	var payment = _paymentService.GetPaymentById(id);
		// 	if (payment == null)
		// 		return NotFound("Payment not found.");
		// 	return Ok(payment);
		// }

		[HttpPatch("{id}")]
		public IActionResult UpdatePaymentById(int id, [FromBody] PaymentReq payment)
		{
			var paymentToUpdate = _paymentService.GetPaymentById(id);
			if (paymentToUpdate == null)
				return NotFound("Payment not found.");

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

			_paymentService.DeletePayment(paymentToDelete);
            _logger.LogInformation("Payment deleted");
            return Ok("Payment deleted successfully.");
		}

		[HttpDelete("deleteall")]
		[Authorize(Roles="pair1, pair2")]
		public IActionResult DeleteAllPayments()
		{
			bool isPair1 = HttpContext.User.IsInRole("pair1");

    
    		bool isPair2 = HttpContext.User.IsInRole("pair2");
			
			if (isPair1)
			{
				_paymentService.DeleteAllPayments("pair1");
                _logger.LogInformation("All debts from piar1 deleted");
                return Ok("All payments deleted successfully.");
			}
			else if(isPair2)
			{
				_paymentService.DeleteAllPayments("pair2");
                _logger.LogInformation("All debts from piar1 deleted");
                return Ok("All payments deleted successfully.");
			}
			else
			{
				return Unauthorized("The user's role could not be determined.");
			}

			
		}
	}
}
