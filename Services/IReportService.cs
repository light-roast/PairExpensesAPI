using PairExpensesAPI.Entities;

namespace PairXpensesAPI.Services
{
	public interface IReportService
	{
		ReportRes? GenerateReport(string pairRole, int percentageA);
	}
}
