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
using Core;

namespace LayerPresentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class LoginController : ControllerBase
	{

		[HttpGet("login/{email}/{password}")]
		public IActionResult Login(string email, string password)
		{
			bool? result = UnitOfWork.Users.PasswordCheck(email, password);

			Massage.LogWarning(result.ToString());

			if (result == null)
			{
				return Ok("email not found");
			}
			else if (result.HasValue)
			{
				Massage.Log(UnitOfWork.Users.GetByEmail(email).Password);
				Massage.Log(UnitOfWork.Users.HashPassword(password, email, UnitOfWork.Users.GetByEmail(email).Id));
			return Ok("How did you guess?!");
			}

				return Ok("password invalid");
		}
	}
}