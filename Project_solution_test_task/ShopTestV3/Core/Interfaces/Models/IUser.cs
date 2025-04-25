using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Interfaces.Models
{
	public interface IUser
	{
		public int Id { get; }
		public string Email { get; }
		public Role Role { get; }
		public string Password { get; set; }
		public decimal Money { get; }
		public List<IComment> IComments { get; }
		public List<IPurchase> IPurchases { get; }
		public List<IProductCard> IProductCards { get; }
	}

	public enum Role
	{
		Default,
		Admin,
	}
}
