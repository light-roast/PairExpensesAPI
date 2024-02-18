using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PairExpensesAPI.Entities;
using PairXpensesAPI.Services;

namespace PairXpensesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PaymentController : ControllerBase
	{
		private readonly IPaymentService _paymentService;

		public PaymentController(IPaymentService paymentService)
		{
			_paymentService = paymentService;
		}

		[HttpGet("user/{userId}")]
		public IActionResult GetAllPaymentsByUserId(int userId)
		{
			var payments = _paymentService.GetAllPaymentsByUserId(userId);
			if (payments != null)
			{
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
			return Ok("Payment created successfully.");
		}

		[HttpGet("{id}")]
		public IActionResult GetPaymentById(int id)
		{
			var payment = _paymentService.GetPaymentById(id);
			if (payment == null)
				return NotFound("Payment not found.");
			return Ok(payment);
		}

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
			return Ok("Payment deleted successfully.");
		}
	}
}
