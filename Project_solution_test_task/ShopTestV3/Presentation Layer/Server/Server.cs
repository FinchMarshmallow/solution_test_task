using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.Runtime.Intrinsics.X86;
using Microsoft.AspNetCore.Hosting.Server.Features;
using System.Net.Sockets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.Extensions.Options;
using System.Linq;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.SignalR;
using System.Data.Entity.Core.Common.CommandTrees.ExpressionBuilder;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Runtime.Intrinsics.Arm;
using System.Security.Policy;
using System;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Core.Interfaces;
using Core;
using LayerPresentation;

namespace LayerPresentation.Server
{
	public class Server
	{
		public static async void StartServer(string url, int port, string filePath, string passwordJWT)
		{
			Massage.Log($"Server starting...  \n\nurl http: http://{url}:{port}/ \n\nurl http: https://{url}:{port}/ \n\nfilePath: {filePath} \n\npasswordJWT: {passwordJWT}");



			// builder ================================================================================================================================================================================================================================================================
			var builder = WebApplication.CreateBuilder();

			builder.WebHost.ConfigureKestrel(options =>
			{
				options.ConfigureHttpsDefaults(https =>
				{
					https.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
				});
			})
			.UseUrls($"http://{url}:{port}/", $"https://{url}:{port+1}/")
			.UseWebRoot("wwwroot");

			builder.Services.AddControllers();
			builder.Services.AddControllers().AddJsonOptions(options =>
			{
				options.JsonSerializerOptions.PropertyNamingPolicy = null;
			});

			builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
			.AddJwtBearer(options =>
			{
				options.TokenValidationParameters = new TokenValidationParameters
				{
					// указывает, будет ли валидироваться издатель при валидации токена
					ValidateIssuer = true,
					// строка, представляющая издателя
					ValidIssuer = AuthOptions.ISSUER,
					// будет ли валидироваться потребитель токена
					ValidateAudience = true,
					// установка потребителя токена
					ValidAudience = AuthOptions.AUDIENCE,
					// будет ли валидироваться время существования
					ValidateLifetime = true,
					// установка ключа безопасности
					IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
					// валидация ключа безопасности
					ValidateIssuerSigningKey = true,
				};
			});


			builder.Services
			.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
			.AddCookie(options =>
			{
				options.Cookie.HttpOnly = true;
				options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
				options.Cookie.SameSite = SameSiteMode.Strict;
			})
			.Services
			.AddControllers();

			builder.Services.AddControllers();
			builder.Services.AddControllers().AddApplicationPart(typeof(Server).Assembly); ;



			// WebApp ================================================================================================================================================================================================================================================================
			var app = builder.Build();

			app.UseHttpsRedirection();

			app.UseStaticFiles(new StaticFileOptions
			{
				OnPrepareResponse = context =>
				{
					Massage.Log($"User GET static files: {context.File.PhysicalPath}");
				},
				FileProvider = new PhysicalFileProvider(FindFilePath() + "\\Presentation Layer\\View\\wwwroot\\"),
				RequestPath = "/wwwroot"
			});

			app.UseAuthentication();
			app.UseAuthorization();

			app.UseRouting();

			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});

			//app.MapGet("/", () => "AAAAAAAAAA");

			app.MapControllers();

			//app.Map("/", () => Massage.Log("Get"));

			await app.RunAsync();
		}


		public class AuthOptions
		{
			public const string ISSUER = "MyAuthServer";
			public const string AUDIENCE = "MyAuthClient";
			const string KEY = "mysupersecret_secretsecretsecretkey!123";
			public static SymmetricSecurityKey GetSymmetricSecurityKey() =>
				new SymmetricSecurityKey(Encoding.UTF8.GetBytes(KEY));
		}

		public static string FindFilePath()
		{
			string fullPart = AppDomain.CurrentDomain.BaseDirectory;
			int binPos = fullPart.IndexOf("_Main");

			fullPart = fullPart.Substring(0, binPos);

			Massage.Log("project position: \n" + fullPart);

			return fullPart;
		}

	}
}
