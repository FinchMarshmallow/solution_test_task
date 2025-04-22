using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Newtonsoft.Json;
using Main.Core.Model;
using Main.Service;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presentation_Layer.Controller
{
	[Route("api/product card")]
	[ApiController]
	public class ControllerProductCard : ControllerBase
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

			byte[] imageBytes = File.ReadAllBytes(imagePath + buffer.Image);
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
				rating,
				price = buffer.Price,
				inStock = buffer.InStock,
			});
		}

		[HttpPost("create/{token}")]
		public IActionResult CreateProductCard([FromBody] DataProductCard product, string token)
		{
			if (!ModelState.IsValid) return Unauthorized();

			User? user = ManagerJWT.ValidateToken(token);

			if (user?.Role != Role.Default) return Unauthorized("you don Default, admin not can shopping!");

			ProductCard productCard = new ProductCard()
			{
				Title = product.title,
				Description = product.description,
				Image = product.image,
				Price = product.price,
				InStock = product.inStock,
				SellerId = user.Id,
				Seller = user
			};

			DatabaseManager.Сontext.ProductCards.Add(productCard);
			user.ProductCards.Add(productCard);

			return Ok(productCard.Id);
		}

		public class DataProductCard 
		{
			public string
				title			= string.Empty,
				description		= string.Empty;

			public byte[]? image = null;

			public decimal price;
			public int inStock;

		}
	}
}
