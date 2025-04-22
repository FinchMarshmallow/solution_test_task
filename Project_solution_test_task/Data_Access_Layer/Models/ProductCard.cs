using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main.Core.Interfaces.Models;

namespace Data_Access_Layer.Model
{
	[Table("product cards")]
	public class ProductCard : IProductCard
	{
		[Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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


		public IUser ISeller => Seller;
		public List<IPurchase> IPurchases => Purchases.Cast<IPurchase>().ToList();
		public List<IComment> IComments => Comments.Cast<IComment>().ToList();


		public int SellerId { get; set; }
		[ForeignKey("SellerId")]
		public User Seller { get; set; } = null!;
		public List<Comment> Comments { get; set; } = new();
		public List<Purchase> Purchases { get; set; } = new();
	}
}
