using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data.Entity;
using System.Text;

namespace Presentation_Layer.Controller
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

			string? accessToken = Login(data);

			if (accessToken == null)
				return Unauthorized();
			else
				return Ok(new { accessToken });
		}


		[HttpPost("sign up")]
		public IActionResult SingUp([FromBody] LoginData data)
		{
			Console.WriteLine("SignUp");
			Role bufferRole;

			switch (data.Role)
			{
				case "Default":
					bufferRole = Role.Default;
					break;

				case "Admin":
					bufferRole = Role.Admin;
					break;

				default:
					return Unauthorized();
			}

			User newUser = new();

			newUser.Email = data.Email;
			newUser.Password = Program.ConvertToHash(data.Password);
			newUser.Role = bufferRole;

			DatabaseManager.Сontext.Users.Add(newUser);
			
			string? accessToken = Login(data);

			if (accessToken == null)
				return Unauthorized();
			else
				return Ok(new { accessToken });
		}


		private string? Login(LoginData data)
		{
			string
				accessToken = string.Empty,
				refreshToken = string.Empty;

			if (DatabaseManager.Сontext.Users.FirstOrDefault(user => user.Email == data.Email)?.Password 
				== Program.ConvertToHash(data.Password))
			{
				accessToken = ManagerJWT.GenerateToken(data.Email, ManagerJWT.TypeToken.Access);
				refreshToken = ManagerJWT.GenerateToken(data.Email, ManagerJWT.TypeToken.Refresh);
			}
			else
			{
				Program.ConsoleColorError("fail Try Login");
				return null;
			}

			Response.Cookies.Append("refreshToken", refreshToken, new CookieOptions
			{
				HttpOnly = true,
				Secure = true,
				SameSite = SameSiteMode.Strict,
				Expires = DateTime.UtcNow.AddDays(ManagerJWT.LifeRefreshToken)
			});

			Program.ConsoleColorGood($"user successfully login: \n\ntokens:\n\n{accessToken}\n\n{refreshToken}");

			return accessToken;
		}


		[HttpGet("refresh")]
		public IActionResult UpdateAccessToken()
		{
			string? refreshToken = Request.Cookies["refreshToken"];

			if(refreshToken == null)
			{
				Program.ConsoleColorError("LoginController: UpdateAccessToken: invalid refresh token");
				return Unauthorized("invalid refresh token");
			}

			User? user = ManagerJWT.ValidateToken(refreshToken);
			
			if (user == null)
			{
				Program.ConsoleColorError($"LoginController: UpdateAccessToken: invalid data user {refreshToken}");
				return Unauthorized("invalid data user");
			}

			string newAccessToken = ManagerJWT.GenerateToken(user.Email, ManagerJWT.TypeToken.Access);

			Program.ConsoleColorWarning($"user update access token:\n{newAccessToken}");

			return Ok(new { newAccessToken });
		}


		public class LoginData
		{
			public string Email { get; set; } = string.Empty;
			public string Password { get; set; } = string.Empty;
			public string Role { get; set; } = string.Empty;
		}
	}
}