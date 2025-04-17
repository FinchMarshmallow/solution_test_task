using Microsoft.EntityFrameworkCore;
using Project_solution_test_task.Model;

namespace Project_solution_test_task.Service
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<ProductCard> ProductCards { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Purchase> Purchases { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			Program.ConsoleColorWarning("AppDbContext created");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			/* Связи:
			 * ProductCard -> User (Seller)
			 * Comment -> User (Author)
			 * Purchase -> ProductCard
			 */

			modelBuilder
			.Entity<ProductCard>()
			.HasOne(product => product.Seller)
			.WithMany(user => user.Products)
			.HasForeignKey(product => product.SellerId)
			.OnDelete(DeleteBehavior.Cascade);

			modelBuilder
			.Entity<Comment>()
			.HasOne(comment => comment.Author)
			.WithMany()
			.HasForeignKey(comment => comment.AuthorId)
			.OnDelete(DeleteBehavior.Restrict);

			modelBuilder
			.Entity<Purchase>()
			.HasOne(product => product.Buyer)
			.WithMany(user => user.Purchases)
			.HasForeignKey(product => product.BuyerId);
		}
	}
}
