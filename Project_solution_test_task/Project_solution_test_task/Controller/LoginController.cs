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
	[Route("api/auth")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		[HttpPost("sign in")]
		public IActionResult SignIn([FromBody] LoginData data)
		{
			Console.WriteLine("SignIn");

			if (!ModelState.IsValid) return Unauthorized();

			string? accessToken = LoginAd(data);

			if (accessToken == null)
				return Unauthorized();
			else
				return Ok(new { accessToken });
		}

		[HttpPost("sign up")]
		public IActionResult SingUp([FromBody] LoginData data)
		{
			Console.WriteLine("SignUp");
			DatabaseModel.UserData buffer = new DatabaseModel.UserData(data.Email, data.Password, DatabaseModel.UserRole.Observer);

			switch (data.Role)
			{
				case "Observer":
					buffer.role = DatabaseModel.UserRole.Observer;
					break;

				case "Default":
					buffer.role = DatabaseModel.UserRole.Default;
					break;

				case "Admin":
					buffer.role = DatabaseModel.UserRole.Admin;
					break;
			}

			DatabaseModel.SignUp(buffer);
			
			string? accessToken = LoginAd(data);

			if (accessToken == null)
				return Unauthorized();
			else
				return Ok(new { accessToken });
		}

		private string? LoginAd(LoginData data)
		{
			string
				accessToken = string.Empty,
				refreshToken = string.Empty;

			if (DatabaseModel.TrySignIn(data.Email, data.Password))
			{
				accessToken = ManagerJWT.GenerateToken(data.Email, ManagerJWT.TypeToken.Access);
				refreshToken = ManagerJWT.GenerateToken(data.Email, ManagerJWT.TypeToken.Refresh);
			}
			else
			{
				Program.ConsoleColorError();
				Console.WriteLine("\nfail Try Login");
				Console.ResetColor();

				return null;
			}

			Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(ManagerJWT.LifeRefreshToken)
			});

			Program.ConsoleColorGood();
			Console.WriteLine($"\nuser successfully login: \n\ntokens:\n\n{accessToken}\n\n{refreshToken}");
			Console.ResetColor();

			return accessToken;
		}

		[HttpGet("refresh")]
		public IActionResult UpdateAccessToken()
		{
			string? 
				refreshToken = Request.Cookies["refreshToken"],
				email;

			if (refreshToken == null || (email = ManagerJWT.ValidateToken(refreshToken)) == null || email == null)
			{

				Program.ConsoleColorError();
				Console.WriteLine("\nInvalid refresh token");
				Console.ResetColor();

				Console.WriteLine($"\ntoken\n{refreshToken}");
				
				return Unauthorized();
			}


			string newAccessToken = ManagerJWT.GenerateToken(email, ManagerJWT.TypeToken.Access);

			Program.ConsoleColorWarning();
			Console.WriteLine($"\nuser update access token:\n{newAccessToken}");
			Console.ResetColor();

			return Ok(new { accessToken = newAccessToken });
		}

		public class LoginData
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
			public string Role { get; set; } = string.Empty;
		}
	}
}