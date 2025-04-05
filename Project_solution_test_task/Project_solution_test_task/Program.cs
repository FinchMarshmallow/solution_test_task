using Microsoft.AspNetCore;
using System.Net;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;


namespace Project_solution_test_task
{
	static class Program
	{
		static private void Main(string[] args)
		{
			int port = GetInputInt("Enter port: ");

			string httpsPassword = CreatePassword(port);

			RaiseServer(port);
		}

		static private void RaiseServer(int port)
		{
			var host = new WebHostBuilder()
			.UseKestrel()
			.UseUrls("https://localhost:5000")
			.Configure(app =>
			{
				app.Run(async context =>
				{
					await context.Response.WriteAsync("Hello from Kestrel");
				});
			})
			.Build();

			host.Run();
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
