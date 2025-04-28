using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using LayerDataAccess;
using LayerDataAccess.Repositories;
using LayerDataAccess.UnitOfWork;

namespace LayerPresentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class LoginController : ControllerBase
	{

		[HttpGet("login{email}/{password}")]
		public IActionResult Login(string email, string password)
		{
			bool? result = UnitOfWork.Users.PasswordCheck(email, password);

			if (result == null)
			{
				return Ok("email not found");
			}
			else if (result.HasValue)
				return Ok("password invalid");

			return Ok("How did you guess?!");
		}
	}
}