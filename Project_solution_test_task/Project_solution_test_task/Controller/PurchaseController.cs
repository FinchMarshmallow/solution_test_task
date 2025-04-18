using Microsoft.AspNetCore.Mvc;
using Project_solution_test_task.Model;
using Project_solution_test_task.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Controller
{
	[Route("api/purchase")]
	[ApiController]
	public class PurchaseController : ControllerBase
	{
		[HttpGet("history/{token}")]
		public IActionResult GetUserPurchases(string token)
		{
			User? user = ManagerJWT.ValidateToken(token);
			if (user == null) return Unauthorized();

			return Ok
			(
				DatabaseManager.Сontext.Purchases
				.Where(purchase => purchase.BuyerId == user.Id)
				.Select(purchase => new
				{
					purchase.Id,
					purchase.PurchaseDate,
					TotalPrice = purchase.ProductCards.Sum(pc => pc.Price),
					ProductsCount = purchase.ProductCards.Count
				})
				.ToList()
			);
		}

		[HttpGet("{id}/{token}")]
		public IActionResult GetPurchaseDetails(int id, string token)
		{
			User? user = ManagerJWT.ValidateToken(token);
			if (user == null) return Unauthorized();

			Purchase? purchase = DatabaseManager.Сontext.Purchases.FirstOrDefault(p => p.Id == id);

			if (purchase == null || (user.Role != Role.Admin && purchase.BuyerId != user.Id))
				return NotFound();

			return Ok(purchase);
		}

		[HttpGet("admin/filter/{token}")]
		public IActionResult FilterPurchases(
			string token,
			[FromQuery] decimal? minPrice = null,
			[FromQuery] decimal? maxPrice = null,
			[FromQuery] DateTime? fromDate = null,
			[FromQuery] int? productId = null)
		{
			var admin = ManagerJWT.ValidateToken(token);
			if (admin?.Role != Role.Admin) return Unauthorized();


			List<Purchase> result = DatabaseManager.Сontext.Purchases.ToList();


			if (fromDate != null)
			{
				result = result.Where(purchases => purchases.PurchaseDate == fromDate).ToList();
			}


			if (productId != null)
			{
				result = result.Where(productId => productId.PurchaseDate == fromDate).ToList();
			}

			if (minPrice != null)
			{
				result = result.Where
				(purchases =>
				{
					decimal buffer = 0;
					foreach(ProductCard card in purchases.ProductCards)
					{
						buffer += card.Price;
					}
					return buffer > minPrice;
				}
				).ToList();
			}


			if (maxPrice != null)
			{
				result = result.Where
				(purchases =>
				{
					decimal buffer = 0;
					foreach (ProductCard card in purchases.ProductCards)
					{
						buffer += card.Price;
					}
					return buffer < minPrice;
				}
				).ToList();
			}

			return Ok(result);
		}


		[HttpPost("create")]
		public IActionResult CreatePurchase([FromBody] PurchaseData data)
		{
			User? user = ManagerJWT.ValidateToken(data.Token);
			if (user == null) return Unauthorized();

			var product = DatabaseManager.Сontext.ProductCards
				.FirstOrDefault(p => p.Id == data.ProductId);

			if (product == null) return NotFound("Product not found");

			Purchase purchase = new Purchase
			{
				BuyerId = user.Id,
				ProductCards = new List<ProductCard> { product },
				PurchaseDate = DateTime.UtcNow
			};

			DatabaseManager.Сontext.Purchases.Add(purchase);
			DatabaseManager.Сontext.SaveChanges();

			return Ok();
		}

		public class PurchaseData
		{
			public string Token { get; set; }
			public int ProductId { get; set; }
		}
	}
}
