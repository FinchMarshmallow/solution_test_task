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
using Project_solution_test_task.Services.Interface;
using Project_solution_test_task.Services.Implementations;


namespace Project_solution_test_task
{
	static class Program
	{
		private static void Main(string[] args)
		{
			bool isUseAutomaticPort = QuestionUserYesOrNot("use automatic port ?");
			int port;

			if (!isUseAutomaticPort)
			{
				port = GetInputInt("Enter port: ");
			}
			else
			{
				port = GetFreePort();
			}

			string url = $"https://localhost:{port}/";
			RaiseServer(url);

			Console.Clear();
			Console.Write("Server -> ");

			ConsoleColorGood();
			Console.Write(url + "\n");
			Console.ResetColor();

			while (true) 
			{ 
				Task.Delay(10); // кастыль, чтоб сервак не закрылся
			}
		}

		static private async void RaiseServer(string url)
		{
			IWebHost host = new WebHostBuilder()
			.UseKestrel()
			.UseUrls(url)
			.ConfigureServices(services =>
			{
				services.AddScoped<IServiceAddition, ServiceAddition>();
				services.AddScoped<IServiceDivision, ServiceDivision>();
				services.AddScoped<IServiceMultiplication, ServiceMultiplication>();
				services.AddScoped<IServiceSubtraction, ServiceSubtraction>();
				services.AddControllers();
			})
			.Configure(app =>
			{
				app.UseRouting();
				app.UseEndpoints(endpoints =>
				{
					endpoints.MapGet("/", async context =>
					{
						await context.Response.WriteAsync("Server is running");
					});

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

		private static string CreatePassword(int port)
		{
			int corn = DateTime.UtcNow.Day * DateTime.UtcNow.Month * DateTime.UtcNow.Month * port;
			Random rnd = new Random(corn);
			int linght = rnd.Next(16, 64);

			char[] chars = new char[linght];

			for (int i = 0; i < linght; i++)
			{
				chars[i] = (char)rnd.Next(0, 256);
			}

			return new string(chars);
		}

		static private int GetInputInt(string header = "Enter int: ", bool isClearConsol = true)
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
					ConsoleColorError();
					Console.WriteLine($"{exception}\n");
					Console.ResetColor();
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

		static public void ConsoleColorError()
		{
			Console.BackgroundColor = ConsoleColor.Red;
			Console.ForegroundColor = ConsoleColor.White;
		}

		static public void ConsoleColorWarning()
		{
			Console.BackgroundColor = ConsoleColor.DarkYellow;
			Console.ForegroundColor = ConsoleColor.White;
		}

		static public void ConsoleColorGood()
		{
			Console.BackgroundColor = ConsoleColor.DarkGreen;
			Console.ForegroundColor = ConsoleColor.White;
		}
	}
}
