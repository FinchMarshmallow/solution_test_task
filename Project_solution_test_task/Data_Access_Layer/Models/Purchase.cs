using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

using Main.Core.Interfaces.Models;

namespace Data_Access_Layer.Model
{
	[Table("purchases")]
	public class Purchase : IPurchase
	{
		[Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int Quantity { get; set; }
		[Required]
		public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

		public DateTime? DeliveryDate { get; set; }
		[Required]
		public bool IsDelivered { get; set; }


		public IUser IBuyer => Buyer;
		public List<IProductCard> IProductCards => ProductCards.Cast<IProductCard>().ToList();
		

		public int BuyerId { get; set; }
		[ForeignKey(nameof(BuyerId))]
		public User Buyer { get; set; } = null!;
		public List<ProductCard> ProductCards { get; set; } = new();
	}
}
