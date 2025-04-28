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
using static LayerPresentation.Server.Server;
using System;
using Core.Interfaces.Models;
using Microsoft.AspNetCore.Authentication.OAuth;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using PresentationLayer.Services;

namespace LayerPresentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class LoginController : ControllerBase
	{

		[HttpPost("Login/{email}/{password}")]
		public IActionResult Login(string email, string password)
		{
			bool? result = UnitOfWork.Users.PasswordCheck(email, password);

			if (result == null)
			{

				return NotFound("incorrect email");
			}
			else if (!result.HasValue)
			{
				return BadRequest("good email, incorrect password");
			}

			IUser? user = UnitOfWork.Users.GetByEmail(email);

			if (user == null) return BadRequest();


			var token = ServiceJWT.GenerateJwtToken(user);

			return Ok(new
			{
				access_token = token,
				username = user.Email,
				role = user.Role.ToString()
			});
		}

		[HttpPost("SungUp/{email}/{password}/{role}")]
		public IActionResult SungUp(string email, string password, string role)
		{
			IUser? user = UnitOfWork.Users.GetByEmail(email);

			if (user != null)
			{
				return BadRequest("this email is busy");
			}

			Role roleUser = Role.Default;

			if (role == "Admin")
				roleUser = Role.Admin;

			UnitOfWork.Users.AddUser(email, password, roleUser);

			user = UnitOfWork.Users.GetByEmail(email);

			if (user == null)
			{
				return BadRequest("email was not created");
			}

			var token = ServiceJWT.GenerateJwtToken(user);

			return Ok(new
			{
				access_token = token,
				username = user.Email,
				role = user.Role.ToString()
			});
		}
	}
}
