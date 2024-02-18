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

		protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
		{
			if (!optionsBuilder.IsConfigured)
				optionsBuilder.UseSqlite("Data Source=Database.db");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			// Configura datos iniciales para la entidad Customer.
			modelBuilder
				.Entity<User>()
				.HasData(
					new User { Id = 1, Name = "Daniel" },
					new User { Id = 2, Name = "Maritza" }
				);
		}
	}
}