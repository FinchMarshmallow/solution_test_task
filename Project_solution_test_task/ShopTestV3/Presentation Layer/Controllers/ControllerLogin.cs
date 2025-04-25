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


namespace LayerPresentation.Controllers
{
	[Route("api/auth")]
	[ApiController]
	public class LoginController : ControllerBase
	{
		private readonly UserManager<IdentityUser> userManager;
		private readonly IConfiguration configuration;

		public LoginController
		(
			UserManager<IdentityUser> userManager,
			IConfiguration configuration
		)
		{
			this.userManager = userManager;
			this.configuration = configuration;
		}



		[HttpPost("signin")]
		public async Task<IActionResult> SignIn([FromBody] LoginData data)
		{
			IdentityUser? user = await userManager.FindByEmailAsync(data.Email);

			if (user == null || !await userManager.CheckPasswordAsync(user, data.Password))
			{
				return Unauthorized();
			}

			return Ok(new 
			{
				accessToken = GenerateJwtToken(user)
			});
		}



		[HttpPost("signup")]
		public async Task<IActionResult> SignUp([FromBody] LoginData data)
		{
			IdentityUser user = new IdentityUser
			{
				UserName = data.Email, // в тз про имя ничего нет )
				Email = data.Email
			};

			IdentityResult result = await userManager.CreateAsync(user, data.Password);

			if (!result.Succeeded) return BadRequest(result.Errors);

			return Ok(new
			{ 
				accessToken = GenerateJwtToken(user)
			});
		}



		private string? GenerateJwtToken(IdentityUser user)
		{
			JwtSecurityTokenHandler tokenHandler = new();

			string? bufferKey = configuration["JwtKey"];

			if (bufferKey == null || user.Email == null) return null;

			byte[] key = Encoding.ASCII.GetBytes(bufferKey);

			SecurityTokenDescriptor tokenDescriptor = new()
			{
				Subject = new ClaimsIdentity(new Claim[]
				{
					new Claim(ClaimTypes.Name, user.Email),
					new Claim(ClaimTypes.NameIdentifier, user.Id)
				}),
				Expires = DateTime.UtcNow.AddHours(1),
				SigningCredentials = new SigningCredentials(
				new SymmetricSecurityKey(key),
				SecurityAlgorithms.HmacSha256Signature)
			};

			return tokenHandler.WriteToken
			(
				tokenHandler.CreateToken(tokenDescriptor)
			);
		}



		public class LoginData
		{
			public string Email = string.Empty;
			public string Password = string.Empty;
		}
	}
}