using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PairXpensesAPI
{
	public class User
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Name { get; set; }

		[JsonIgnore]
		public List<Debt>? Debts { get; set; }

		[JsonIgnore]
		public List<Payment>? Payments { get; set; }
	}
}
