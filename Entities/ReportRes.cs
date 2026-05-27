namespace PairExpensesAPI.Entities
{
	public class ReportUserBreakdown
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public long TotalPayment { get; set; }
		public long TotalDebt { get; set; }
		public long Share { get; set; }
	}

	public class ReportSettlement
	{
		public int FromUserId { get; set; }
		public int ToUserId { get; set; }
		public long Amount { get; set; }
	}

	public class ReportRes
	{
		public bool HasData { get; set; }
		public int PercentageA { get; set; }
		public long TotalExpense { get; set; }
		public ReportUserBreakdown UserA { get; set; } = null!;
		public ReportUserBreakdown UserB { get; set; } = null!;
		public ReportSettlement? Settlement { get; set; }
	}
}
