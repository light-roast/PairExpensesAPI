namespace PairExpensesAPI.Entities
{
	public class PaymentReq
	{
		public int Id { get; set; }
		public string? Name { get; set; }
		public long Value { get; set; }
		public DateTime UpdateDate { get; set; } = DateTime.Now;

		public DateTime CreateDate {get; set; } = DateTime.Now;


	}
}
