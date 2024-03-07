using Microsoft.EntityFrameworkCore;
using PairXpensesAPI;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

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
			modelBuilder
				.Entity<User>()
				.HasData(
					new User { Id = 1, Name = "Cody1", Username="codyone", Password="default_diffindb", PairRole="pair1" },
					new User { Id = 2, Name = "Cody2", Username = "codytwo", Password = "default_diffindb", PairRole = "pair1" },
                    new User { Id = 3, Name = "Daniel", Username = "lightroast", Password = "default_diffindb", PairRole = "pair2" },
                    new User { Id = 4, Name = "Mari", Username = "maritza.30.r", Password = "default_diffindb", PairRole = "pair2" }
                );
		}
	}
}