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
using Microsoft.EntityFrameworkCore;
using Project_solution_test_task.Model.Db;
using Microsoft.Extensions.Options;
using Npgsql;
using System.Linq;


namespace Project_solution_test_task
{
	static class Program
	{
		public static string

			filePath	= string.Empty,
			url			= string.Empty,

			strOptions	= string.Empty,
			nameDb		= "test_shop",
			userId		= "postgres",
			passwordDb	= "Ql^73#91Lop@4";

		public static int port;

		public static AppDbContext? 
			users_Db,
			ProductCards_Db,
			purchases_Db;

		private static void Main(string[] args)
		{
			DataBaseInit();

			bool isUseAutomaticPort =
				true;
				//QuestionUserYesOrNot("use automatic port ?");

			if (!isUseAutomaticPort)
			{
				port = GetInputInt("Enter port: ");
			}
			else
			{
				port = GetFreePort();
			}

			url = 
				"https://localhost:51785/";
				//url = $"https://localhost:{port}/";

			RaiseServer(url);

			Console.Write("Server -> ");
			ConsoleColorGood(url);

			filePath = FindFilePath();




			//User user = new User
			//{
			//	Email = "admin@store.com",
			//	PasswordHash = "qwer1234"
			//};

			//DatabaseManager.Сontext.Users.Add(user);
			//DatabaseManager.Сontext.SaveChanges();

			//var products = DatabaseManager.Сontext.Users.ToList();

			//foreach (var product in products)
			//{
			//	Console.WriteLine($"User email: {product.Email}");
			//}





			while (true) 
			{ 
				Task.Delay(100); // кастыль, чтоб сервак не закрылся
			}
		}



		private static string FindFilePath()
		{
			string fullPart = AppDomain.CurrentDomain.BaseDirectory;
			int binPos = fullPart.IndexOf("bin");
			return fullPart.Substring(0, binPos);
		}

		private static async void RaiseServer(string url)
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
				//services.AddScoped<IServiceAddition, ServiceAddition>();
				//services.AddScoped<IServiceDivision, ServiceDivision>();
				//services.AddScoped<IServiceMultiplication, ServiceMultiplication>();
				//services.AddScoped<IServiceSubtraction, ServiceSubtraction>();

				// кука для JWT
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
						ConsoleColorGood("user Get static files:");
						Console.WriteLine($"\n{file.File.PhysicalPath}");
					},

					FileProvider = new PhysicalFileProvider
					(
						Path.Combine(Directory.GetCurrentDirectory(), filePath)
					),
					RequestPath = filePath
				});
				app.UseRouting();
				app.UseEndpoints(endpoints =>
				{
					endpoints.MapControllers();
				});

			})
			.SuppressStatusMessages(true)
			.Build();

			await host.RunAsync();
		}

		public static bool QuestionUserYesOrNot(string question = "what is the meaning of life ?", bool isClear = true)
		{
			if (isClear) Console.Clear();

			Console.Write(question);

			Console.ResetColor();
			Console.Write(" ");

			ConsoleColorError();
			Console.Write("N");

			Console.ResetColor();
			Console.Write(" / ");

			ConsoleColorGood();
			Console.Write("Y");

			Console.ResetColor();
			Console.Write(" ");

			while (true)
			{
				ConsoleKey buffer = Console.ReadKey(true).Key;

				if (buffer == ConsoleKey.Y)
				{
					return true;
				}
				else if (buffer == ConsoleKey.N)
				{
					return false;
				}
			}
		}

		private static int GetFreePort() // кастыль
		{
			var listener = new TcpListener(IPAddress.Loopback, 0);
			listener.Start();
			int port = ((IPEndPoint)listener.LocalEndpoint).Port;
			listener.Stop();
			return port;
		}

		private static int GetInputInt(string header = "Enter int: ", bool isClearConsol = true)
		{
			string? input = null;
			string 
				exception = "",
				nullReference = "you not entered text";

			while (true)
			{
				if (isClearConsol) Console.Clear();

				if (exception.Length > 0)
				{
					ConsoleColorError($"{exception}\n");
				}
				if (header.Length > 0)
				{
					Console.Write(header);
				}

				try
				{
					input = Console.ReadLine();

					if (input == null)
					{
						throw new ArgumentNullException(nullReference);
					}
					else
					{
						return int.Parse(input);
					}

				}
				catch(Exception e)
				{
					exception = e.Message;
				}
			}
		}

		private static void DataBaseInit()
		{
			strOptions =
				$"Host=localhost;" +
				$"Port=5432;" +
				$"Database={nameDb};" +
				$"User Id={userId};" +
				$"Password={passwordDb};";
		}


		#region Beautiful Console

		public static void ConsoleColorError(string? massage = null)
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;

			if (massage == null) return;

			Console.WriteLine("\n" + massage);
			Console.ResetColor();
		}

		public static void ConsoleColorWarning(string? massage = null)
		{
			Console.BackgroundColor = ConsoleColor.DarkYellow;
			Console.ForegroundColor = ConsoleColor.White;

			if (massage == null) return;

			Console.WriteLine("\n" + massage);
			Console.ResetColor();
		}

		public static void ConsoleColorGood(string? massage = null)
		{
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.ForegroundColor = ConsoleColor.White;

			if (massage == null) return;

			Console.WriteLine("\n" + massage);
			Console.ResetColor();
		}

		public static void ConsoleColorBeautiful(string? massage = null)
		{
			Console.BackgroundColor = ConsoleColor.Magenta;
			Console.ForegroundColor = ConsoleColor.White;

			if (massage == null) return;

			Console.WriteLine("\n" + massage);
			Console.ResetColor();
		}

		#endregion
	}
}
