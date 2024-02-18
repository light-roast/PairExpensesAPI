using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PairXpensesAPI
{
	public class Payment
	{
		[Key]
		public int Id { get; set; }

		[Required]
		public string? Name { get; set; }

		[Range(0, long.MaxValue, ErrorMessage = "Value must be greater than or equal to 0.")]
		public long Value { get; set; }

		public DateTime CreateDate { get; set; } = DateTime.Now;

		public DateTime UpdateDate { get; set; } = DateTime.Now;

		public int UserId { get; set; }

		[JsonIgnore]
		public User User { get; set; } = null!;
	}
}
