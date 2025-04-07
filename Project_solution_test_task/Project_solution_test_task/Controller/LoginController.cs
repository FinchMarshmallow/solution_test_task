using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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
		private readonly IConfiguration config;

		public LoginController(IConfiguration config)
		{
			this.config = config;
		}

		[HttpPost("/api/login")]
		public IActionResult Post([FromBody] LoginData data)
		{
			if (!ModelState.IsValid) return BadRequest("error data");

			string token = "";

			try
			{
				token = GenerateJwtToken(data.Email);
				Console.WriteLine(token);
			}
			catch (Exception e)
			{
				Program.ConsoleColorError();
				Console.WriteLine("\n"+ e.Message);
				Console.ResetColor();
			}

			Console.WriteLine($"\ndata user: email= {data.Email}, password= {data.Password}, token= {token}");

			return Ok(new { message = "good data" });
		}

		public class LoginData
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}

		private string GenerateJwtToken(string email)
		{
			string?
				keyJwt = config["Jwt:Key"],
				issuerJwt = config["Jwt:Issuer"],
				audienceJwt = config["Jwt:Audience"];

			if (keyJwt == null)
			{
				throw new Exception("\nconfig get key return null");
			}

			SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
			SigningCredentials credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var claims = new[]
			{
				new Claim(JwtRegisteredClaimNames.Email, email),
				new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
			};

			JwtSecurityToken token = new JwtSecurityToken
			(
				issuer: config["Jwt:Issuer"],
				audience: config["Jwt:Audience"],
				claims: claims,
				expires: DateTime.UtcNow.AddDays(30),
				signingCredentials: credentials
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}