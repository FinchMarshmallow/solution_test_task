using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Project_solution_test_task.Model
{
	[Table("purchases")]
	public class Purchase
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public int Quantity { get; set; }

		[Required]
		public DateTime PurchaseDate { get; set; } = DateTime.UtcNow;

		public DateTime? DeliveryDate { get; set; }

		[Required]
		public bool IsDelivered { get; set; }


		public int BuyerId { get; set; }
		[ForeignKey(nameof(BuyerId))]
		public User Buyer { get; set; } = null!;


		public int ProductCardId { get; set; }
		[ForeignKey(nameof(ProductCardId))]
		public ProductCard Product { get; set; } = null!;
	}
}
