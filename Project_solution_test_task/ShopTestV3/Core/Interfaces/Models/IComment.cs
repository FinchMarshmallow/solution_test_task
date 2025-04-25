using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Models
{
	public interface IComment
	{
		public int Id { get; }
		public string Message { get; }
		public DateTime CreatedAt { get; }
		public int AuthorId { get; }
		public IUser IAuthor { get; }
		public int ProductCardId { get; }
		public IProductCard IProductCard  { get; }
	}
}
