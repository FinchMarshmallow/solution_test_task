using Core;
using LayerDataAccess.Model;
using Microsoft.EntityFrameworkCore;

namespace LayerDataAccess.Migrations
{
	public class AppDbContext : DbContext
	{
		public DbSet<User> Users { get; set; }
		public DbSet<ProductCard> ProductCards { get; set; }
		public DbSet<Comment> Comments { get; set; }
		public DbSet<Purchase> Purchases { get; set; }

		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
		{
			Massage.LogGood("AppDbContext has been created");
		}

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			modelBuilder
			.Entity<ProductCard>()
			.HasOne(product => product.Seller)
			.WithMany(user => user.ProductCards)
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

			base.OnModelCreating(modelBuilder);
		}
	}
}
