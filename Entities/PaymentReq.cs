namespace PairExpensesAPI.Entities
{
	public class PaymentReq
	{
		public string? Name { get; set; }
		public int Value { get; set; }
		public DateTime UpdateDate { get; set; } = DateTime.Now;

		public DateTime CreateDate {get; set; } = DateTime.Now;


	}
}
