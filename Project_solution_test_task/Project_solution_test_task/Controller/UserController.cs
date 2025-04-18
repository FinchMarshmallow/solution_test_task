using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Project_solution_test_task.Model;
using Project_solution_test_task.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_solution_test_task.Controller
{
	[Route("api/user")]
	[ApiController]
	public class UserController : ControllerBase
	{
		[HttpGet("all/{token}")]
		public IActionResult GetAllUser(string token)
		{
			if (!ModelState.IsValid) return Unauthorized();

			User? user = ManagerJWT.ValidateToken(token);

			if(user?.Role != Role.Admin) return Unauthorized("you don Admin, get out!");

			return Ok(JsonConvert.SerializeObject(DatabaseManager.Сontext.Users));
		}

		[HttpGet("{id}/{token}")]
		public IActionResult GetUser(string token, int id)
		{
			if (!ModelState.IsValid) return Unauthorized();

			User? user = ManagerJWT.ValidateToken(token);

			if (user?.Role != Role.Admin) return Unauthorized("you don Admin, get out!");

			return Ok(DatabaseManager.Сontext.Users.FirstOrDefault(user => user.Id == id));
		}

		[HttpGet("my history/{token}")]
		public IActionResult GetMyHistory(string token)
		{
			if (!ModelState.IsValid) return Unauthorized();

			User? user = ManagerJWT.ValidateToken(token);

			if (user?.Role != Role.Default) return Unauthorized("you don Default, admin not can shopping!");

			return Ok(user.Purchases);
		}
	}
}
