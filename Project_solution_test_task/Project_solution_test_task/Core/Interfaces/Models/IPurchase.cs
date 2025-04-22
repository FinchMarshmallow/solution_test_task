using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Core.Interfaces.Models
{
	public interface IPurchase
	{
		public int Id { get; }
		public int Quantity { get; }
		public DateTime PurchaseDate { get; }
		public DateTime? DeliveryDate { get; }
		public bool IsDelivered { get;}
		public int BuyerId { get; }
		public IUser IBuyer { get; }
		public List<IProductCard> IProductCards { get; }
	}
}
