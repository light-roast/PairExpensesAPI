using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PairExpensesAPI.Auth;
using PairXpensesAPI.Services;

namespace PairXpensesAPI.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	[Authorize(Roles = "pair1, pair2")]
	public class ReportController : ControllerBase
	{
		private readonly IReportService _reportService;
		private readonly ILogger<ReportController> _logger;

		public ReportController(IReportService reportService, ILogger<ReportController> logger)
		{
			_reportService = reportService;
			_logger = logger;
		}

		[HttpGet]
		public IActionResult GetReport([FromQuery] int percentageA)
		{
			if (percentageA < 0 || percentageA > 100)
				return BadRequest("percentageA must be between 0 and 100.");

			var pair = this.GetCallerPair();
			if (pair is null) return Unauthorized("The user's role could not be determined.");

			var report = _reportService.GenerateReport(pair, percentageA);
			if (report is null) return NotFound("Pair users not found.");

			_logger.LogInformation("Report generated for {Pair} at {Percentage}%", pair, percentageA);
			return Ok(report);
		}
	}
}
