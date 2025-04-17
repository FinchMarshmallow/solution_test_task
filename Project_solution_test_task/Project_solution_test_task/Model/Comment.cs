using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Model
{
	[Table("comments")]
	public class Comment
	{
		[Key]
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }

		[Required]
		public string Message { get; set; } = null!;

		public int Rating { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


		public int AuthorId { get; set; }
		[ForeignKey(nameof(AuthorId))]
		public User Author { get; set; } = null!;


		public int ProductCardId { get; set; }
		[ForeignKey(nameof(ProductCardId))]
		public ProductCard Product { get; set; } = null!;
	}
}
