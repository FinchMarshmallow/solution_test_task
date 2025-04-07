using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
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
			if (!ModelState.IsValid)
			{
				return BadRequest("error data");
			}

			Console.WriteLine($"\n data user: {data.Email.Length}, {data.Password.Length}");

			return Ok(new { message = "good data" });
		}
		public class LoginData
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
		}
	}
}