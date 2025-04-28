using Core;
using Core.Interfaces.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PresentationLayer.Services
{
	public static class ServiceJWT
	{
		public static string GenerateJwtToken(IUser user)
		{
			var claims = new List<Claim>
			{
				new(ClaimTypes.Name, user.Email),
				new(ClaimTypes.Role, user.Role.ToString())
			};

			var jwt = new JwtSecurityToken
			(
				issuer: AuthOptions.ISSUER,
				audience: AuthOptions.AUDIENCE,
				claims: claims,
				expires: DateTime.UtcNow.AddMinutes(AuthOptions.LIFETIME_MINUTES),
				signingCredentials: new SigningCredentials
				(
					AuthOptions.GetSymmetricSecurityKey(),
					SecurityAlgorithms.HmacSha256
				)
			);

			return new JwtSecurityTokenHandler().WriteToken(jwt);
		}
	}

	public static class AuthOptions
	{
		public const string ISSUER = "MyAuthServer";
		public const string AUDIENCE = "MyAuthClient";
		public const int LIFETIME_MINUTES = 30;
		const string KEY = Config.passwordJWT;

		private static SymmetricSecurityKey SymmetricKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
		public static SymmetricSecurityKey GetSymmetricSecurityKey() => SymmetricKey;


	}
}
