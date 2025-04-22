using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Main;
using Main.Core.Interfaces.Models;

namespace Data_Access_Layer.Model
{
	[Table("comments")]
	public class Comment : IComment
	{
		[Key] [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		public int Id { get; set; }
		[Required]
		public string Message { get; set; } = null!;

		public int Rating { get; set; }

		public DateTime CreatedAt { get; set; } = DateTime.UtcNow;


		public int AuthorId { get; set; }
		public int ProductCardId { get; set; }


		public IProductCard IProductCard => ProductCard;
		public IUser IAuthor => Author;


		[ForeignKey(nameof(AuthorId))]
		public User Author { get; set; } = null!;

		[ForeignKey(nameof(ProductCardId))]
		public ProductCard ProductCard { get; set; } = null!;
	}
}
