using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Project_solution_test_task.Model;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Controller
{
	[ApiController]
	[Route("[controller]")]
	public class LoginController : ControllerBase
	{
		[HttpPost("/api/login")]
		public IActionResult Post([FromBody] LoginData data)
		{
			if (!ModelState.IsValid) return BadRequest("error data");

			string
				accessToken = string.Empty,
				refreshToken = string.Empty;

			if (DatabaseModel.TryLogin(data.Email, data.Password))
			{
				accessToken = ManagerJWT.GenerateToken(data.Email, ManagerJWT.TypeToken.AccessToke);
				refreshToken = ManagerJWT.GenerateToken(data.Email, ManagerJWT.TypeToken.RefreshToken);
			}
			else
			{
				Program.ConsoleColorError();
				Console.WriteLine("\nfail Try Login");
				Console.ResetColor();

				return Unauthorized(new { massage = "fail Try Login" });
			}

			Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(7)
			});

			Program.ConsoleColorGood();
			Console.WriteLine($"\nuser successfully login: \n\n token:\n{accessToken}\n\n{refreshToken}");
			Console.ResetColor();

			return Ok(new { accessToken });
		}

		public class LoginData
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}
	}
}