using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main.Core.Interfaces.Models
{
	public interface IProductCard
	{
		public int Id { get; }
		public string Title { get; }
		public string? Description { get; }
		public byte[]? Image { get; }
		public decimal Price { get; }
		public int InStock { get; }
		public int SellerId { get; }
		public IUser ISeller { get; }
		public List<IPurchase> IPurchases { get; }
		public List<IComment> IComments { get; }
	}
}
