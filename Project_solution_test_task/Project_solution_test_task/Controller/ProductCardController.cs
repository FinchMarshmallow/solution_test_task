using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Project_solution_test_task.Model;
using Project_solution_test_task.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Controller
{
	[Route("api/product card")]
	[ApiController]
	public class ProductCardController : ControllerBase
	{
		private readonly string imagePath = Program.filePath + "/View/Product images/";

		[HttpGet("all")]
		public IActionResult GetAllProductCard()
		{
			return Ok(JsonConvert.SerializeObject(DatabaseManager.Сontext.ProductCards));
		}

		[HttpGet("{id}")]
		public IActionResult GetProductCard(int id)
		{
			ProductCard? buffer = DatabaseManager.Сontext.ProductCards.FirstOrDefault(product => product.Id == id);
				
			if (buffer == null)
				return NotFound();

			byte[] imageBytes = System.IO.File.ReadAllBytes(imagePath + buffer.Image);
			int bufferRating = 0;

			foreach (Comment comment in buffer.Comments)
			{
				bufferRating += comment.Rating;
			}

			float rating = bufferRating / buffer.Comments.Count;

			return Ok(
			new
			{
				title = buffer.Title,
				description = buffer.Description,
				image = Convert.ToBase64String(imageBytes),
				seller = buffer.Seller.Email,
				rating = rating,
				price = buffer.Price,
				inStock = buffer.inStock,
			});
		}
	}
}
