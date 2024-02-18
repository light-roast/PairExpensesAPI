using Microsoft.EntityFrameworkCore;
using PairXpensesAPI;

namespace PairExpensesAPI.Data
{
	public class DataContext : DbContext
	{
		public DbSet<User> Users { get; set; } = null!;
		public DbSet<Debt> Debts { get; set; } = null!;
		public DbSet<Payment> Payments { get; set; } = null!;

		public DataContext(DbContextOptions<DataContext> options) : base(options)
		{
		}
	}
}