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
	public class Server : IServer
	{
		public bool StartServer(string url, string filePath, string passwordJWT)
		{
			IWebHost host = new WebHostBuilder()
			.UseKestrel(options =>
			{
				options.ConfigureHttpsDefaults(httpsOptions =>
				{
					httpsOptions.SslProtocols = System.Security.Authentication.SslProtocols.Tls12 | System.Security.Authentication.SslProtocols.Tls13;
				});
			})
			.UseUrls(url)
			.UseWebRoot("wwwroot")
			.ConfigureServices(services =>
			{
				services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
				.AddCookie(options =>
				{
					options.Cookie.HttpOnly = true;
					options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
					options.Cookie.SameSite = SameSiteMode.Strict;
				});

				services.AddControllers();
			})
			.Configure(app =>
			{
				app.UseHttpsRedirection();
				app.UseStaticFiles(new StaticFileOptions
				{
					OnPrepareResponse = file =>
					{
						Massage.Log($"user Get static files: \n\n{file.File.PhysicalPath}\n");
					},

					FileProvider = new PhysicalFileProvider
					(
						Path.Combine(Directory.GetCurrentDirectory(), Config.filePath + "Layer_Presentation/View")
					),
					RequestPath = Config.filePath
				});
				app.UseRouting();
				app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});

			})
			.SuppressStatusMessages(true)
			.Build();

			host.RunAsync();

			return true;
		}
	}
}
