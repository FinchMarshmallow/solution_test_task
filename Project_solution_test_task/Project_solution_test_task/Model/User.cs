using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Model
{
	[Table("Users")]
	public class User
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		[EmailAddress]
		public string Email { get; set; } = null!;

		[Required]
		public Role Role { get; set; } = Role.Default;

		[Required]
		public string Password { get; set; } = null!;

		[Required]
		public decimal Money { get; set; }


		public List<Comment> Comments { get; set; } = new();
		public List<Purchase> Purchases { get; set; } = new();
		public List<ProductCard> ProductCards { get; set; } = new();
	}

	public enum Role
	{
		Default,
		Admin,
	}
}
