using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Model
{
	[Table("product cards")]
	public class ProductCard
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Title { get; set; } = null!;

		public string? Description { get; set; }

		[Column("ImageByte")]
		public byte[]? Image { get; set; } = null;

		[Required]
		public decimal Price { get; set; }

		[Required]
		public int InStock { get; set; }

		public int SellerId { get; set; }
		[ForeignKey("SellerId")]
		public User Seller { get; set; } = null!;

		public List<Purchase> Purchases { get; set; } = new();
		public List<Comment> Comments { get; set; } = new();
	}
}
