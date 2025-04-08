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
		private readonly IConfiguration config;

		public LoginController(IConfiguration config)
		{
			this.config = config;
		}

		[HttpPost("/api/login")]
		public IActionResult Post([FromBody] LoginData data)
		{
			if (!ModelState.IsValid) return BadRequest("error data");

			string? token = "";

			try
			{
				token = DatabaseModel.Login(data.Email, data.Password);
			}
			catch (Exception e)
			{
				Program.ConsoleColorError();
				Console.WriteLine("\n"+ e.Message);
				Console.ResetColor();

				return Unauthorized(new { massage = "server problem" });
			}

			if (token == null ||  token.Length <= 3)
			{
				Program.ConsoleColorWarning();
				Console.WriteLine("\nIncorrect email or password");
				Console.ResetColor();

				return BadRequest(new { message = "Incorrect email or password" });
			}
			else
			{
				Console.WriteLine($"\ndata user: email= {data.Email}, password= {data.Password}, token= {token}");

				return Ok(new { massage = "good login" });
			}
		}

		public class LoginData
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}
	}
}